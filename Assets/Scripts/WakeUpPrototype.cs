using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

// Origem: Assets/Scripts/WakeUpPrototype.cs
// Script de prototipo narrativo para orquestrar o despertar do protagonista no Prologo
public class WakeUpPrototype : MonoBehaviour
{
    [Header("Arquivos da Cutscene")]
    // Arquivo JSON do dialogo que toca o pensamento inicial antes de abrir os olhos
    [FormerlySerializedAs("dialogopensamento")]
    public TextAsset dialogoPensamento;
    // Arquivo JSON do dialogo com o celular tocando que leva a escolha Levantar/Dormir
    public TextAsset dialogoCelularTocou;

    void Start()
    {
        StartCoroutine(RotinaAcordar());
    }

    private IEnumerator RotinaAcordar()
    {
        if (CutsceneManager.Instance == null)
        {
            Debug.LogError("[WakeUpPrototype] CutsceneManager nao encontrado na cena!");
            yield break;
        }

        // REVIEW: nao checa flag nenhuma (ex: acordouPrimeiroDia); com o save carregado a cutscene toca toda vez que a cena abre.
        // 1. Pausa o jogador e nao deixa ele se mexer
        CutsceneManager.Instance.TravarJogador();

        if (dialogoPensamento != null)
        {
            yield return CutsceneManager.Instance.TocarDialogoEEsperar(dialogoPensamento);
        }
        
        // REVIEW: nesta pausa o InteractionSystem segue ativo (so checa o Panel aberto): Enter num objeto abre outro dialogo,
        // o DialogueTrigger devolve podeMover=true e o player anda no meio da cutscene.
        // 2. Pausa dramatica de 2 segundos de silencio
        yield return new WaitForSeconds(2f);

        // 3. Toca o dialogo do celular
        if (dialogoCelularTocou != null)
        {
            yield return CutsceneManager.Instance.TocarDialogoEEsperar(dialogoCelularTocou);
        }

        // 4. Libera o jogador explicitamente
        CutsceneManager.Instance.LiberarJogador();
        
        Debug.Log("[WakeUpPrototype] Cutscene de abertura finalizada com sucesso!");
    }
}
