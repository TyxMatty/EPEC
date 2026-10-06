# Manual do Desenvolvedor - Projeto EPEC
Bem-vindo ao manual do projeto EPEC! Se você é um novato na equipe, este documento é a sua Bíblia. Ele explica como a arquitetura Anti-Spaghetti do jogo funciona, onde cada script deve ser colocado no Unity e como "ligar os fios" no Inspector para que nada quebre.

---

## 1. O Jogador (Player)
O objeto do jogador na cena é o centro da ação. Ele exige a configuração correta de fìsica e inputs.
### Scripts para Anexar no Player:
1. **`MovementScript`**
   - **O que faz:** Controla a física e o Input (WASD) do jogador.
   - **O que exige:** O jogador **precisa** ter um componente `Rigidbody2D` (com Gravidade em 0) e um `BoxCollider2D`.
   - **Atributos:** Você pode ajustar a `speed` base no Inspector.
2. **`SpriteChange`**
   - **O que faz:** Troca os frames da animação sem usar o Animator do Unity.
   - **O que exige:** O jogador precisa ter um componente `SpriteRenderer`.
   - **Atributos:** Arraste os sprites das caminhadas (frente, costas, lados) nas listas do Inspector.
3. **`InteractionSystem`**
   - **O que faz:** Dispara um raio invisível (Raycast) na frente do jogador para detectar objetos interagíveis.
   - **Atributos:** Defina a `InteractableLayer` no Inspector. Apenas objetos que estiverem nessa Layer (camada) específica serão detectados pelo jogador.

---

## 2. O Cérebro do Jogo (Game Managers)
Os Managers não têm forma física. Crie um **GameObject Vazio** (Empty Object) na cena e chame-o de `GameManager`. Anexe os scripts abaixo nele:

### Scripts para Anexar no GameManager:

1. **`SaveManager`**

   - **O que faz:** Salva as variáveis e o inventário em um JSON no disco do jogador.
   - **Lidando no Inspector:**
     - `stateManager`: Arraste o seu arquivo Asset `GlobalStateManager` que está na pasta do projeto.
     - `inventory`: Arraste o seu arquivo Asset `InventoryData` (Mochila).

2. **`ChoiceSystemGlobal`**

   - **O que faz:** Exibe a UI de escolhas e altera as variáveis quando o jogador toma uma decisão.
   - **Lidando no Inspector:**
     - `stateManager` / `inventarioGlobal`: Arraste os mesmos assets citados acima.
     - `choiceUI`: Arraste o painel (Panel) da Canvas que guarda os botões de escolha.
     - `choiceTexts`: Arraste os componentes TextMeshPro dos botões de escolha.

3. **`DialogueSystemGlobal`**

   - **O que faz:** Escreve as falas na tela como uma máquina de escrever.
   - **Lidando no Inspector:**
     - `dialogueText`: Arraste o componente de Texto (TextMeshPro) onde a fala vai aparecer.
     - `canvasGroup`: Arraste o componente CanvasGroup do painel principal de diálogo (usado para fazer o fade in/out).
     - `GameManager`: Arraste **este próprio GameObject** do GameManager (porque ele precisa falar com o `ChoiceSystemGlobal` que está anexado ao lado dele).

---

## 3. Os Dados Base (Scriptable Objects)
Estes não ficam na Cena. Eles são **Arquivos** que ficam salvos na sua pasta `Assets`.
- **`GlobalStateManager`**: A memória do jogo. Lá você cria uma lista inicial de variáveis (ex: `portaAberta = false`, `rotaR = false`). Os Managers consultam esse arquivo.
- **`InventoryData`**: A mochila. Contém a lista de `ItemData` que o jogador pegou.
- **`ItemData`**: O "Molde" de um item (ex: a `Chave.asset`).
  - **⚠ REGRA DE OURO DO SAVE:** Todo `ItemData` **TEM** que estar salvo dentro da pasta exata `Assets/Resources/Itens`. Se o arquivo não estiver nessa pasta, o `SaveManager` não consegue achar a imagem do item quando o jogador carregar o jogo!

---

## 4. O Mundo Físico (Objetos Interagíveis)
Como criar um guarda-roupa, porta ou NPC que fala?
1. Crie o Sprite na cena.
2. Adicione um componente `BoxCollider2D`.
3. Mude a **Layer** no canto superior direito do Inspector para a mesma Layer que você configurou no `InteractionSystem` do jogador (ex: `Interagivel`).
4. Anexe um dos dois scripts abaixo:

### A - Para Falas / Eventos (Usar `DialogueTrigger`)

- **Como configurar:** Crie um arquivo JSON com as falas e condições (na pasta `DialoguesData`). Arraste esse JSON para o slot vazio que vai aparecer no script `DialogueTrigger` no Inspector.

### B - Para Coletar Itens (Usar `ItemPickup`)

- **Como configurar:** Arraste o `ItemData` (ex: a Chave) da sua pasta `Resources/Itens` para o slot do script `ItemPickup`. Quando o jogador interagir, o item some da cena e vai para a Mochila.

---
--- 
## 5. Como Configurar Paineis de UI

Se precisar criar uma nova tela (ex: Escolhas, Dialogo):
1. Crie um Canvas e um Panel dentro dele.
2. **Ancoras:** No Inspector do Rect Transform, clique no icone do quadrado. Segure Alt e clique na opcao do canto inferior direito (Stretch). Isso faz o painel esticar com a tela.
3. Se for uma caixa de texto na base da tela, use a ancora Inferior-Centro (Bottom-Center) e ajuste o Pivot para Y = 0.
4. Os textos devem usar o TextMeshProUGUI.
5. Anexe os botoes ou paineis nas referencias publicas dos Managers (ex: choiceTexts no ChoiceSystemGlobal).

---


## 6. Cutscenes e Dialogos Desconectados (Standalone)

O sistema de Cutscenes (atraves do script `CutsceneManager.cs`) permite criar eventos scriptados e bloquear o jogador de forma global. Ele utiliza dialogos desconectados (standalone) para interagir com a interface.

### Dialogos Desconectados
Um dialogo desconectado e um arquivo JSON feito apenas para tocar um texto, sem disparar escolhas ou mudar variaveis no meio da cutscene (deixando essa logica para o script da cutscene em si).
Seu JSON DEVE ter as seguintes propriedades como falsas:
- `terminaEmEscolha: false`
- `alteraVariavel: false`

### Como usar o CutsceneManager
Para usar o CutsceneManager e tocar dialogos no meio da sua cutscene customizada (como no script `WakeUpPrototype.cs`), utilize **Coroutines** no Unity.

1. Crie uma Coroutine no seu script de cutscene (ex: `IEnumerator MinhaCutscene()`).
2. Desative o movimento do jogador.
3. Chame a funcao de dialogo e use o comando `yield return StartCoroutine` para esperar o dialogo acabar antes de continuar o script.
   Exemplo: `yield return StartCoroutine(CutsceneManager.Instance.TocarDialogoEEsperar(meuArquivoJson));`
4. Quando o jogador fechar o dialogo, o script vai automaticamente continuar para a proxima linha abaixo.
5. Termine a cutscene e devolva o controle ao jogador.


## 7. Como Criar um Diálogo em JSON (Guia Rápido)
Não escrevemos falas no Unity, escrevemos em JSON! Crie um arquivo `.json` em `Assets/Scripts/Interactions/Dialogos/DialoguesData/` com este formato:
```json
{
    "dialogueID": "porta_exemplo",
    "falas": [ "Hmm, trancada." ],
    "alteraVariavel": false,
    
    "temFrasesAlternativas": true,
    "itemDaCondicao": "Chave",
    
    "dialogoAlternativoAlteraVariavel": true,
    "variavelAlteradaPorDialogoAlternativo": "portaAberta",
    "falasAlternativas": [ "Usei a chave, abriu!" ],
    
    "terminaEmEscolha": false
} 
```
Tradução do código acima: "Ao interagir, se o jogador tiver a Chave no inventário, diga que abriu e mude a variável global portaAberta para true. Senão, diga apenas que está trancada."