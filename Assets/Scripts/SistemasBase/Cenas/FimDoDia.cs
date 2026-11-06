using UnityEngine;

// Origem: Assets/Scripts/SistemasBase/Cenas/FimDoDia.cs
// Encerra o dia: calcula o resumo (flor do dia) pelas regras do JSON, salva e vai para a proxima cena com fade.
// Um componente por saida: fim normal, desmaio do ataque de panico e Rota R profunda podem apontar cenas diferentes.
// Chamar EncerrarDia() por UnityEvent, cutscene ou pelo QTE.
public class FimDoDia : MonoBehaviour
{
    [Header("Estado")]
    public GlobalStateManager estado;
    // Opcional: so precisa se alguma regra usar {"item": ...}
    public InventoryData mochila;

    [Header("Resumo do dia")]
    // Ex: Assets/Dados/Regras/flor_dia1.json; vazio = nao calcula nada
    public TextAsset regras;

    [Header("Destino")]
    public string proximaCena;
    public string spawn;

    public void EncerrarDia()
    {
        if (estado != null && regras != null)
        {
            ListaDeRegras lista = JsonUtility.FromJson<ListaDeRegras>(regras.text);
            Regra escolhida = lista != null ? lista.Aplicar(estado, mochila) : null;
            if (escolhida != null) Debug.Log($"[FimDoDia] Resumo do dia: {escolhida.nome}");
            else Debug.LogWarning($"[FimDoDia] Nenhuma regra de '{regras.name}' bateu com o estado atual; nada foi ligado.");
        }

        // Procurado na hora (e nao ligado no Inspector) porque mora no Nucleo: a copia da cena pode ter sido apagada
        SaveManager saveManager = FindAnyObjectByType<SaveManager>();
        if (saveManager != null) saveManager.SalvarJogo();
        else Debug.LogWarning("[FimDoDia] Nenhum SaveManager ativo; o dia terminou sem salvar.");

        if (SceneTransitionManager.Instance != null) SceneTransitionManager.Instance.IrPara(proximaCena, spawn);
        else Debug.LogError("[FimDoDia] Nenhum SceneTransitionManager (prefab Nucleo) na cena.");
    }
}
