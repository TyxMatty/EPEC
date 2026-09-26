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
        void Start()
        {
            if (nextDialogueBox != null)
            {
                nextDialogueBox.gameObject.SetActive(false);
            }
            StartDialogue();
        }
        void Update()
        {
            if(isTransitioning) return;
            var keyboard = Keyboard.current;
            if(keyboard == null) return;
            if (keyboard.zKey.wasPressedThisFrame)
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
        }
        
    }
    
