using UnityEngine;
using UnityEngine.InputSystem;

public class InputRebindManager : MonoBehaviour
{
    [Header("Asset Central de Inputs")]
    public InputActionAsset inputActions;

    private void Start()
    {
        CarregarBindsSalvos();
    }

    /// Inicia o processo de rebind para uma acao especifica.
    /// Exemplo de chamada no UI Button: StartRebind("Gameplay/Move", 0, textComponent);
    public void StartRebind(string actionName, int bindingIndex, TMPro.TextMeshProUGUI buttonText)
    {
        InputAction actionToRebind = inputActions.FindAction(actionName);
        if (actionToRebind == null) 
        {
            Debug.LogError($"[InputRebind] Acao {actionName} nao encontrada.");
            return;
        }

        actionToRebind.Disable();

        buttonText.text = "Aguardando tecla...";

        var rebindOperation = actionToRebind.PerformInteractiveRebinding(bindingIndex)
            .WithControlsExcluding("Mouse") // Impede de colocar clique do mouse onde nao deve
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => 
            {
                operation.Dispose();
                actionToRebind.Enable();
                
                buttonText.text = InputControlPath.ToHumanReadableString(
                    actionToRebind.bindings[bindingIndex].effectivePath,
                    InputControlPath.HumanReadableStringOptions.OmitDevice);

                SalvarBinds();
            })
            .OnCancel(operation =>
            {
                operation.Dispose();
                actionToRebind.Enable();
            })
            .Start();
    }

    private void SalvarBinds()
    {
        string rebinds = inputActions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("rebinds", rebinds);
        PlayerPrefs.Save();
        Debug.Log("[InputRebind] Binds salvos com sucesso.");
    }

    private void CarregarBindsSalvos()
    {
        if (PlayerPrefs.HasKey("rebinds"))
        {
            string rebinds = PlayerPrefs.GetString("rebinds");
            inputActions.LoadBindingOverridesFromJson(rebinds);
        }
    }
}
