using UnityEngine;
using System.Collections;

public class WakeUpPrototype : MonoBehaviour
{
    [Header("Arquivos da Cutscene")]
    public TextAsset dialogoCelularTocou; // JSON com "Hmmm, um bipe?" -> vai para escolhas (Levantar / Dormir mais)
    public TextAsset dialogopensamento;
    // Roda no Start() para iniciar assim que a cena abre
    void Start()
    {
        StartCoroutine(RotinaAcordar());
    }

    private IEnumerator RotinaAcordar()
    {
        // 1. Pausa o jogador e nao deixa ele se mexer
        CutsceneManager.Instance.TravarJogador();

        yield return CutsceneManager.Instance.TocarDialogoEEsperar(dialogopensamento);
        
        // 2. Faz uma pausa dramatica de 2 segundos de silencio
        yield return new WaitForSeconds(2f);

        // (se quisermos colocar uma animacao no futuro: playerAnimator.Play("Acordar") e WaitForSeconds)

        // 3. Toca o JSON do dialogo
        yield return CutsceneManager.Instance.TocarDialogoEEsperar(dialogoCelularTocou);

        // 4. A cutscene acabou. Libera o jogador explicitamente!
        CutsceneManager.Instance.LiberarJogador();
        
        Debug.Log("Cutscene de abertura finalizada!");
    }
}
