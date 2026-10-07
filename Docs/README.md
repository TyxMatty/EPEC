# Manual do Desenvolvedor - Projeto EPEC

Bem-vindo ao manual do projeto EPEC! Este documento e o guia definitivo da arquitetura do jogo. Ele explica como cada sistema funciona, onde cada script deve ser anexado no Unity e como conectar as referencias no Inspector para garantir estabilidade e extensibilidade sem spaghetti code.

---

## 1. Estrutura de Pastas de Scripts (`Assets/Scripts/`)

Para manter o projeto limpo e desacoplado, os scripts estao organizados nos seguintes modulos:

- `Assets/Scripts/`: Gerenciadores globais de alto nivel (`GlobalStateManager.cs`, `ReputationSystem.cs`, `CutsceneManager.cs`, `PostProcessingManager.cs`, `FlowerStateListener.cs`, `WakeUpPrototype.cs`).
- `Assets/Scripts/SistemasBase/`: Movimentacao basica e animacao manual (`MovementScript.cs`, `SpriteChange.cs`).
- `Assets/Scripts/SistemasBase/Input/`: Rebind e gerenciamento de entradas (`InputRebindManager.cs`).
- `Assets/Scripts/SistemasBase/Escolhas/`: Persistencia de dados em disco (`SaveManager.cs`).
- `Assets/Scripts/Interactions/`: Interacao fisica, inventario e escolhas (`InteractionSystem.cs`, `Interactable.cs`, `ItemPickup.cs`, `ChoiceSystemGlobal.cs`, `InventoryData.cs`, `ItemData.cs`, `IInteractAction.cs`).
- `Assets/Scripts/Interactions/Dialogos/`: Dialogos data-driven e maquina de escrever (`DialogueSystemGlobal.cs`, `DialogueTrigger.cs`, `DialogueData.cs`).
- `Assets/Scripts/Cameras/`: Modos de camera dinamica e por salas (`CameraFollow.cs`, `SceneFollow.cs`).

---

## 2. O Jogador (Player)

O GameObject do jogador na cena e o centro das acoes fisicas e de input.

### Scripts para Anexar no Player:

1. **`MovementScript`** (em `SistemasBase`)
   - **O que faz:** Controla a fisica de movimentacao 2D utilizando `Rigidbody2D` e o Unity Input System moderno (`InputActionReference`).
   - **O que exige:** Componentes `Rigidbody2D` (com Gravity Scale = 0 e Collision Detection = Continuous) e `BoxCollider2D`.
   - **Campos no Inspector:**
     - `speed`: Velocidade base de caminhada.
     - `runMultiplier`: Multiplicador de velocidade ao correr.
     - `moveAction`: Referencia para o `InputActionReference` de movimento (Vector2 / WASD / D-Pad).
     - `sprintAction`: Referencia para o `InputActionReference` de corrida (Shift / Botao de Sprint).
   - **Controle Externo:** A variavel publica `podeMover` (bool) e alterada por dialogos e cutscenes para travar o jogador instantaneamente quando necessario.

2. **`SpriteChange`** (em `SistemasBase`)
   - **O que faz:** Troca os frames de animacao diretamente no `SpriteRenderer` com base na velocidade vetorial, sem a sobrecarga do Animator do Unity.
   - **O que exige:** Componente `SpriteRenderer`.
   - **Campos no Inspector:**
     - Listas de Sprites para cada direcao (frente, costas, lados) nas velocidades parado, andando e correndo.
     - `transitionSpeed`: Intervalo de tempo entre a troca de frames.

3. **`InteractionSystem`** (em `Interactions`)
   - **O que faz:** Dispara um `Raycast2D` invisivel na direcao em que o jogador esta virado para detectar objetos interagiveis.
   - **Campos no Inspector:**
     - `interactableLayer`: LayerMask configurada para detectar objetos interativos (ex: Layer `Interagivel`).
     - `rayDistance`: Distancia maxima de alcance da interacao.
     - `interactionPrompt`: GameObject ou icone de UI (ex: botao "E") exibido na tela quando o jogador esta na frente de um objeto interativo.
     - `interactAction`: Referencia para o `InputActionReference` de interacao (tecla E / Enter / Gamepad South).

---

## 3. Os Gerenciadores Globais (Game Managers)

Os managers coordenam o estado do jogo e nao possuem corpo fisico no mundo. Recomenda-se criar um GameObject vazio chamado `GameManager` na cena inicial e anexar os gerenciadores necessarios:

1. **`GlobalStateManager`** (ScriptableObject)
   - **O que faz:** A memoria central do jogo. Armazena a lista de flags booleanas (`List<GameFlag>`) e a lista de reputacoes numericas (`List<Reputacao>`).
   - **Caracteristicas:**
     - Nao usa Dicionarios: Mantem listas serializaveis compativeis com o Inspector do Unity e com salvamento em JSON.
     - Auto-inicializacao: Garante inicializacao segura em `OnEnable()` e getters/setters/adders com busca segura e null checks.

2. **`ReputationSystem`** (MonoBehaviour Singleton)
   - **O que faz:** Interface dinamica para consulta e alteracao de reputacao de personagens e faccoes (ex: Tobias, Maria, Gi).
   - **Campos no Inspector:**
     - `globalState`: Referencia ao asset `GlobalStateManager`.
   - **Metodos Principais:**
     - `ReputationSystem.Instance.AdicionarReputacao(nome, valor)`
     - `ReputationSystem.Instance.ObterReputacao(nome)`

3. **`SaveManager`** (em `SistemasBase/Escolhas`)
   - **O que faz:** Serializa o estado completo do jogo (`GameSaveData`) em um arquivo JSON em `Application.persistentDataPath/savegame.json`.
   - **O que e salvo:**
     - Lista de Flags booleanas do `GlobalStateManager`.
     - Lista de Reputacoes numericas do `GlobalStateManager`.
     - Lista de IDs dos itens contidos na mochila (`InventoryData`).
   - **Campos no Inspector:**
     - `stateManager`: Referencia ao asset `GlobalStateManager`.
     - `inventory`: Referencia ao asset `InventoryData`.

4. **`ChoiceSystemGlobal`** (em `Interactions`)
   - **O que faz:** Exibe a UI de escolhas ramificadas com temporizadores opcionais e atualiza as variaveis no `GlobalStateManager` ou no `InventoryData`.
   - **Campos no Inspector:**
     - `choiceUI`: GameObject do painel da Canvas de escolhas.
     - `choiceTexts`: Array de `TextMeshProUGUI` dos botoes de escolha.
     - `stateManager`: Referencia ao asset `GlobalStateManager`.
     - `inventarioGlobal`: Referencia ao asset `InventoryData`.

5. **`DialogueSystemGlobal`** (em `Interactions/Dialogos`)
   - **O que faz:** Controla a exibicao de falas na tela com efeito retro suave de maquina de escrever (`dialogueText.text += letter`).
   - **Campos no Inspector:**
     - `dialogueText`: Componente `TextMeshProUGUI` onde as frases sao renderizadas.
     - `canvasGroup`: Componente `CanvasGroup` do painel de dialogo para transicoes suaves de alfa.
     - `stateManager`: Referencia ao `GlobalStateManager` para verificacao e alteracao de variaveis.
     - `inventarioGlobal`: Referencia ao `InventoryData` para checagem de itens requeridos em condicoes de dialogo.
     - `choiceSystem`: Referencia direta ao componente `ChoiceSystemGlobal`.

6. **`CutsceneManager`**
   - **O que faz:** Singleton responsavel por orquestrar sequencias narrativas, travar o jogador e fornecer corrotinas de espera (`TocarDialogoEEsperar`).
   - **Seguranca:** Inclui guardas contra deadlock caso um dialogo seja interrompido abruptamente, devolvendo o controle da corrotina.

---

## 4. Dados em Scriptable Objects (`Assets`)

Estes assets sao salvos diretamente nos arquivos do projeto Unity:

- **`GlobalStateManager`**: Asset criado via `Create -> Scriptable Objects -> GlobalStateManager`.
- **`InventoryData`**: Asset criado via `Create -> Scriptable Objects -> InventoryData`. Contem a lista `itens` (`List<ItemData>`) e metodos utilitarios como `AdicionarItem`, `RemoverItem`, `ContemItem` e `Limpar`.
- **`ItemData`**: Asset que define um item colecionavel (ID, Nome, Descricao, Icone).
  - **REGRA OBRIGATORIA:** Todos os assets de `ItemData` **DEVEM** ser salvos dentro da pasta `Assets/Resources/Itens/`. O `SaveManager` utiliza `Resources.Load<ItemData>("Itens/" + id)` para restaurar os itens apos recarregar o jogo.

---

## 5. Objetos Interagiveis no Cenario

Para criar um objeto que o jogador pode inspecionar, falar ou coletar:

1. Crie o GameObject com `SpriteRenderer` e `BoxCollider2D`.
2. Defina a **Layer** do GameObject para a mesma configurada no `InteractionSystem` do jogador (ex: `Interagivel`).
3. Anexe os componentes conforme o proposito do objeto:

### A - Para Dialogos e Eventos: `DialogueTrigger`
- Anexe o script `DialogueTrigger` ao objeto.
- Arraste o arquivo `TextAsset` (o `.json` criado na pasta `DialoguesData`) para o campo `dialogueJSON`.
- Opcionalmente, configure `onlyOnce` se o dialogo so puder ser disparado uma vez.

### B - Para Coleta de Itens: `ItemPickup`
- Anexe o script `ItemPickup` ao objeto.
- Arraste o asset `ItemData` correspondente (salvo em `Resources/Itens/`) para o campo `item`.
- Ao interagir, o item e adicionado ao `InventoryData` e o GameObject e destruido ou desativado.

### C - Para Cenarios Reativos: `FlowerStateListener`
- Anexe o script `FlowerStateListener` em flores ou elementos do cenario que reagem ao estado narrativo.
- Configure `targetFlag` (nome da flag no `GlobalStateManager`), `activeSprite` e `defaultSprite`.
- O script atualiza o sprite automaticamente no `Start()` e pode ser chamado a qualquer momento via `AtualizarEstadoVisual()`.

---

## 6. Sistema de Cutscenes e Dialogos Standalone

Para criar cutscenes scriptadas (como a do despertar em `WakeUpPrototype.cs`):

1. Crie uma Coroutine no seu script da cutscene (`IEnumerator ExecutarCutscene()`).
2. Trave o jogador: `MovementScript.podeMover = false;`
3. Dispare o dialogo e pause a execucao ate o fim da leitura:
   ```csharp
   yield return StartCoroutine(CutsceneManager.Instance.TocarDialogoEEsperar(meuTextAssetJson));
   ```
4. Execute animacoes, mudancas de camera ou fades.
5. Devolva o movimento ao jogador ao concluir: `MovementScript.podeMover = true;`

### Dialogos Desconectados (Standalone)
Um dialogo standalone e um arquivo `.json` utilizado em cutscenes sem ramificacao automatica:
- Defina `"terminaEmEscolha": false`
- Defina `"alteraVariavel": false`
Isso permite que a logica de escolhas e mudanca de estado seja controlada exclusivamente pelo script da cutscene.

---

## 7. Formato Padrao de Dialogos em JSON

Os arquivos de dialogo devem ser salvos em `Assets/Scripts/Interactions/Dialogos/DialoguesData/` com extensao `.json`:

```json
{
    "dialogueID": "porta_quarto",
    "falas": [
        "A porta esta trancada por fora."
    ],
    "alteraVariavel": false,
    "variavelParaAlterar": "",

    "temFrasesAlternativas": true,
    "itemDaCondicao": "ChaveAntiga",
    "falasAlternativas": [
        "Usei a Chave Antiga na fechadura.",
        "A porta se abriu com um estalo rangente."
    ],
    "dialogoAlternativoAlteraVariavel": true,
    "variavelAlteradaPorDialogoAlternativo": "portaQuartoAberta",

    "terminaEmEscolha": false,
    "textoPergunta": "",
    "textosDasOpcoes": [],
    "variavelDaEscolha": "",
    "temTimer": false,
    "tempoLimite": 0.0
}
```

---

## 8. Boas Praticas e Convencoes de Codigo

1. **Null Checks em Campos Serializados:** Sempre valide campos `[SerializeField]` com verificacao nula antes de acessar membros para prevenir `NullReferenceException` quando referencias forem esquecidas no Inspector.
2. **Preservacao de Nomes no Inspector:** Ao renomear variaveis serializadas em scripts C#, utilize o atributo `[FormerlySerializedAs("nomeAntigo")]` para evitar que as ligacoes nas cenas (`.unity`) e prefabs sejam perdidas.
3. **Comentarios e Codificacao:** Mantenha todos os comentarios de codigo e mensagens em caracteres ASCII puros para evitar problemas de compatibilidade e corrupcao de codificacao UTF-8 no Windows.