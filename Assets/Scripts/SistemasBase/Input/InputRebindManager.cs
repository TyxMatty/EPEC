using UnityEngine;
using UnityEngine.InputSystem;

// Gerenciador de remapeamento de controles dinamico (Rebind UI) salvando em PlayerPrefs
public class InputRebindManager : MonoBehaviour
{
    [Header("Asset Central de Inputs")]
    // Referencia ao InputActionAsset contendo todos os Action Maps (ex: PlayerControls)
    public InputActionAsset inputActions;

    private InputActionRebindingExtensions.RebindingOperation activeRebindOperation;

    private void Start()
    {
        CarregarBindsSalvos();
    }

    /// Inicia o processo de rebind para uma acao especifica.
    /// Exemplo de chamada no UI Button: StartRebind("Gameplay/Move", 0, textComponent);

    private void OnDestroy()
    {
        CancelarRebindAtivo();
    }

    private void CancelarRebindAtivo()
    {
        if (activeRebindOperation != null)
        {
            activeRebindOperation.Cancel();
            activeRebindOperation.Dispose();
            activeRebindOperation = null;
        }
    }

    // Metodo publico chamado pela UI para remapear uma tecla
    // Exemplo: StartRebind("Gameplay/Move", 0, textComponent)
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

