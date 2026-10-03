    using System.Collections;
    using TMPro;
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class DialogueSystemGlobal : MonoBehaviour

    {
        [Header("Configurações do Sistema de Diálogo")]
        [SerializeField] private float typingSpeed = 0.05f;
        [SerializeField] private Transform nextDialogueBox;
        [SerializeField] private TextMeshProUGUI dialogueText;
        [Header("Textos do Diálogo")]
        [TextArea(3, 5)]
        [SerializeField] private string[] dialogueLines; // armazena o dialogo escrito ali em cima.
        [Header("Efeitos de Transição")] //opcional
        [SerializeField] private CanvasGroup canvasGroup; // armazena o canvas group do objeto, para poder fazer a transição de fade in e fade out
        [SerializeField] private float fadeDuration = 0.5f; // tempo de duração do fade in e fade out
        private int letraAtual = 0; // armazena a letra atual
        private bool isTyping = false; // armazena se o sistema de dialogo está digitando ou não
        private bool isTransitioning = false; // armazena se o sistema de dialogo está fazendo a transição ou não
        private bool podeAvançar = false; // armazena se o sistema de dialogo pode avançar ou não, para evitar que o player avance antes do fade out terminar
        void Start()
        {
            if (nextDialogueBox != null)
            {
                nextDialogueBox.gameObject.SetActive(false);
            }
            // StartDialogue(); // com nosso sistema de interação, o StartDialogue() vai ser chamado pelo InteractionSystem, então não precisa mais do StartDialogue() aqui, mas vou deixar ele comentado para caso queira testar o sistema de diálogo sozinho.
            gameObject.SetActive(false); // desativa a caixa de diálogo no início, para só ativar quando o player interagir com um objeto
        }
        void Update()
        {
            if(isTransitioning || !podeAvançar) return;
            var keyboard = Keyboard.current;
            if(keyboard == null) return;
            if (keyboard.enterKey.wasPressedThisFrame)
            {
                if (isTyping)
                {
                    StopAllCoroutines();
                    dialogueText.text = dialogueLines[letraAtual];
                    isTyping = false;
                }
                else
                {
                    ProximoDialogo();   
                }
            }
        }
        private void StartDialogue()
        {
            letraAtual = 0;
            if (dialogueLines.Length > 0)
            {
                StartCoroutine(DigitarLinha());
            }
            Invoke("HabilitarAvanco", 0.2f); // delay para evitar que o player avance antes do fade out terminar
        }
        private void ProximoDialogo(){
        letraAtual++;
        if (letraAtual < dialogueLines.Length)
        {
            StartCoroutine(DigitarLinha());
        }   else
        {
            StartCoroutine(Transicao());
            }
        }
        private void HabilitarAvanco()
        {
            podeAvançar = true;
        }
        private IEnumerator DigitarLinha()
        {
        isTyping = true;
        dialogueText.text = "";
            foreach (char letter in dialogueLines[letraAtual].ToCharArray())
                {
                 dialogueText.text += letter;
                  yield return new WaitForSeconds(typingSpeed);
                }
        isTyping = false;
        }
        private IEnumerator Transicao()
        {
            isTransitioning = true;
            // Fade out
            if (canvasGroup != null)
            {
                float tempo = 0f;
                while (tempo < fadeDuration)
                {
                    canvasGroup.alpha = Mathf.Lerp(1f, 0f, tempo / fadeDuration);
                    tempo += Time.deltaTime;
                    yield return null;
                }
                canvasGroup.alpha = 0f;
            }
            // ativar a próxima caixa de diálogo
            if (nextDialogueBox != null)
            {
                nextDialogueBox.gameObject.SetActive(true);
            }
            gameObject.SetActive(false); // desativar a caixa de diálogo atual
            MovementScript movementScript = FindAnyObjectByType<MovementScript>();
            if (movementScript != null)
            {
                movementScript.podeMover = true; // ativa o movimento do player quando o sistema de diálogo terminar
            }
        }
        public void IniciarDialogo(string[] linhas) // integração com o InteractionSystem, ele vai usar essa função(método em C#) para transformar o dialogueLines no array dele. O que está nesse script é para teste, mas o que vai ser usado é o que está no InteractionSystem, que é o array de falas do Interactable. 
        // Então, quando o player interagir com um objeto, ele vai pegar as falas do Interactable e passar para o DialogueSystemGlobal.
        {
            dialogueLines = linhas; // linhas que a string nova vai receber, que é o array de falas do Interactable
            isTransitioning = false;
            if (canvasGroup != null)
                {
                    canvasGroup.alpha = 1f; // volta a tornar a caixa visível, após o fade out
                }
            StartDialogue();
        }
    }
    
