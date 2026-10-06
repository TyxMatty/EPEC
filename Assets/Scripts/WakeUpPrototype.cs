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
        // 1. Pausa o jogador e não deixa ele se mexer
        CutsceneManager.Instance.TravarJogador();

        yield return CutsceneManager.Instance.TocarDialogoEEsperar(dialogopensamento);
        CutsceneManager.Instance.TravarJogador();
        // 2. Faz uma pausa dramática de 2 segundos de silêncio
        yield return new WaitForSeconds(2f);

        // (se nois quiser colocar uma animação no futuro, seria algo como: playerAnimator.Play("Acordar") e outro WaitForSeconds)

        // 3. Toca o JSON do diálogo (que dentro dele tem a configuração pra chamar as Escolhas e atualizar o StateManager!)
        yield return CutsceneManager.Instance.TocarDialogoEEsperar(dialogoCelularTocou);

        // 4. Aqui o diálogo acabou e a variável global já foi salva pelo ChoiceSystem.
        // A cutscene acabou. O DialogueSystemGlobal solta a trava de movimento automaticamente quando acaba.
        Debug.Log("Cutscene de abertura finalizada!");
    }
}