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

    [Header("Jogo Novo (opcional)")]
    // Tela de nome mostrada antes do despertar quando o jogador ainda nao escolheu um nome
    public NomeDoJogadorUI telaDeNome;

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

        if (telaDeNome != null && telaDeNome.FaltaNome)
        {
            yield return telaDeNome.PedirNome();
        }

        if (dialogoPensamento != null)
        {
            yield return CutsceneManager.Instance.TocarDialogoEEsperar(dialogoPensamento);
        }
        
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
