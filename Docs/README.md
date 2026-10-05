# Documentacao de Sistemas e How-To - EPEC

Bem-vindo ao manual oficial de montagem do EPEC! Se voce e novo no projeto, este documento e o seu guia passo-a-passo para entender onde tudo fica, como criar cenas e como fazer o jogo funcionar sem quebrar.

Nossa arquitetura e focada em ser **Anti-Spaghetti**: usamos listas em ScriptableObjects e JSONs para que tudo seja modular e independente.

---

## 1. Onde Colocar Cada Script? (Mapa do Projeto)

- **Jogador (Player):** 
  - MovementScript: Cuida da movimentacao e inputs.
  - SpriteChange: Troca os sprites dependendo de pra onde ele anda.
  - InteractionSystem: Fica checando o que esta na frente do jogador e aciona os eventos.
- **Gerenciadores Globais (Managers):** Devem ficar em um objeto vazio na cena (ex: GameManager).
  - DialogueSystemGlobal: Cuida do painel de dialogo.
  - ChoiceSystemGlobal: Cuida das escolhas (fala com o StateManager).
  - SaveManager: Salva e carrega o jogo usando o GlobalStateManager e o InventoryData.
- **Objetos Interagiveis (Cenario):** 
  - DialogueTrigger (Implementa IInteractAction): Leve ele, arraste seu JSON de dialogo e coloque um BoxCollider2D.
  - ItemPickup (Implementa IInteractAction): Leve ele, de o nome do item e o sprite some quando coletado.
- **Assets Especiais:**
  - GlobalStateManager (ScriptableObject): A memoria do jogo. Guarda variaveis (otaR, portaAberta).
  - InventoryData (ScriptableObject): A mochila do jogador.
  - *Itens do jogo:* SEMPRE salve em Assets/Resources/Itens senao o save system nao acha eles!

---

## 2. Como Configurar Paineis de UI

Se precisar criar uma nova tela (ex: Escolhas, Dialogo):
1. Crie um Canvas e um Panel dentro dele.
2. **Ancoras:** No Inspector do Rect Transform, clique no icone do quadrado. Segure Alt e clique na opcao do canto inferior direito (Stretch). Isso faz o painel esticar com a tela.
3. Se for uma caixa de texto na base da tela, use a ancora Inferior-Centro (Bottom-Center) e ajuste o Pivot para Y = 0.
4. Os textos devem usar o TextMeshProUGUI.
5. Anexe os botoes ou paineis nas referencias publicas dos Managers (ex: choiceTexts no ChoiceSystemGlobal).

---

## 3. Como Fazer Dialogos e Condicoes (Passo a Passo)

Nosso dialogo abandonou aquele lixo de arrays no Inspector. Tudo e via JSON!

### Passo A: Criar o JSON
Crie um arquivo .json (ex: DialogoQuarto.json) em Assets/Scripts/Interactions/Dialogos/DialoguesData/.
Exemplo de formato:
{
    "dialogueID": "quarto_1",
    "falas": [ "Hmm, mais um dia." ],
    "alteraVariavel": false,
    "temFrasesAlternativas": true,
    "itemDaCondicao": "Chave",
    "dialogoAlternativoAlteraVariavel": true,
    "variavelAlteradaPorDialogoAlternativo": "portaAberta",
    "falasAlternativas": [ "Vou destrancar isso!" ],
    "terminaEmEscolha": false
}

### Passo B: Configurar o Objeto na Cena
1. Coloque um objeto (ex: Guarda-Roupa).
2. Adicione um **Collider2D** e certifique-se que o Player tem ele na interactableLayer do seu raycast.
3. Adicione o script DialogueTrigger.cs.
4. No campo do script, arraste o arquivo .json que voce criou.

---

## 4. O Sistema de Variaveis e Save (A Magica)

Nos abandonamos os dicionarios espaguete em favor do GlobalStateManager.cs.

**Como checar se o jogador fez algo:**
No codigo: GameManager.ChecarCondicao("acordouPrimeiroDia")

**Como salvar o jogo:**
Nao precisa! O SaveManager.cs salva tudo no Windows (%AppData%/LocalLow/EPEC team/EPEC/savegame.json) quando o jogo fecha ou quando voce manda.
*Regra de Ouro:* Seu ItemData chamado Chave (ou seja la qual item for) **PRECISA OBRIGATORIAMENTE** estar na pasta Assets/Resources/Itens. Se mudar o nome da pasta, o Save quebra, pois o Unity busca itens por la quando carrega o jogo.

---

## 5. Cutscenes (Sendo implementado)
Em breve: Em vez do InteractionSystem dar gatilho num dialogo, um CutsceneManager global podera bloquear o movimento (podeMover = false), disparar animacoes via SpriteChange modificado, e iniciar falas automaticamente (via eventos) chamando o DialogueSystemGlobal. Fique atento para atualizacoes!
