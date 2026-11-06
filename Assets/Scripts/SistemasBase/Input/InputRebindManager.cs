using UnityEngine;
using UnityEngine.InputSystem;

// Gerenciador de remapeamento de controles dinamico (Rebind UI) salvando em PlayerPrefs
public class InputRebindManager : MonoBehaviour
{
    [Header("Asset Central de Inputs")]
    // Referencia ao InputActionAsset contendo todos os Action Maps (ex: PlayerControls)
    public InputActionAsset inputActions;

    // Operacao de rebind em andamento (null quando nenhuma)
    private InputActionRebindingExtensions.RebindingOperation activeRebindOperation;

    // REVIEW: nao ha InputRebindManager na cena 0.unity, entao binds salvos nunca sao carregados.
    private void Start()
    {
        CarregarBindsSalvos();
    }

    // Cancela um rebind em andamento ao destruir o objeto
    private void OnDestroy()
    {
        CancelarRebindAtivo();
    }

    private void CancelarRebindAtivo()
    {
        if (activeRebindOperation != null)
        {
            activeRebindOperation.Cancel();
            // Cancel() roda o OnCancel na hora, que ja faz Dispose e zera o campo; o ?. evita a NullReferenceException
            activeRebindOperation?.Dispose();
            activeRebindOperation = null;
        }
    }

    // Metodo publico chamado pela UI para remapear uma tecla
    // Exemplo: StartRebind("Gameplay/Interact", 0, textComponent)
    // Em Move o indice 0 e o composite 2DVector, nao uma tecla: 1=W 2=S 3=A 4=D, 6 a 9 = setas
    public void StartRebind(string actionName, int bindingIndex, TMPro.TextMeshProUGUI buttonText)
    {
        if (activeRebindOperation != null) return; 
        if (inputActions == null)
        {
            Debug.LogError("[InputRebind] inputActions asset nao atribuido no Inspector!");
            return;
        }

        InputAction actionToRebind = inputActions.FindAction(actionName);
        if (actionToRebind == null) 
        {
            Debug.LogError($"[InputRebind] Acao {actionName} nao encontrada.");
            return;
        }

        if (bindingIndex < 0 || bindingIndex >= actionToRebind.bindings.Count)
        {
            Debug.LogError($"[InputRebind] Indice de binding {bindingIndex} fora dos limites para a acao {actionName}.");
            return;
        }

        actionToRebind.Disable();

        if (buttonText != null) buttonText.text = "Aguardando tecla...";

        activeRebindOperation = actionToRebind.PerformInteractiveRebinding(bindingIndex)
            .WithControlsExcluding("Mouse") // Impede de colocar clique do mouse onde nao deve
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => 
            {
                operation.Dispose();
                activeRebindOperation = null;
                actionToRebind.Enable();
                
                if (buttonText != null)
                {
                    buttonText.text = InputControlPath.ToHumanReadableString(
                        actionToRebind.bindings[bindingIndex].effectivePath,
                        InputControlPath.HumanReadableStringOptions.OmitDevice);
                }

                SalvarBinds();
            })
            .OnCancel(operation =>
            {
                operation.Dispose();
                activeRebindOperation = null;
                actionToRebind.Enable();
            })
            .Start();
    }

    private void SalvarBinds()
    {
        if (inputActions == null) return;

        string rebinds = inputActions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("rebinds", rebinds);
        PlayerPrefs.Save();
        Debug.Log("[InputRebind] Binds salvos com sucesso.");
    }

    private void CarregarBindsSalvos()
    {
        if (inputActions == null) return;

        if (PlayerPrefs.HasKey("rebinds"))
        {
            string rebinds = PlayerPrefs.GetString("rebinds");
            inputActions.LoadBindingOverridesFromJson(rebinds);
        }
    }
}

