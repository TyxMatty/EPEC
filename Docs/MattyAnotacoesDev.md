# EPEC: Code Review e Anotacoes de Qualidade e Arquitetura

Historico de revisoes tecnicas, decisoes de design e melhorias aplicadas no projeto EPEC pela equipe e agentes de IA.

---

## 1. Arquitetura e Desacoplamento (Lego vs God Object)

**Diagnostico Inicial:**
O script `Interactable.cs` concentrava muitas responsabilidades (dialogos, flags, inventario e escolhas), correndo o risco de se tornar um "God Object".

**Solucao Implementada:**
- Criacao de componentes especializados:
  - `Interactable.cs`: Componente base que interage com o Raycast do jogador e aceita extensoes via `IInteractAction`.
  - `DialogueTrigger.cs`: Focado exclusivamente em disparar dialogos data-driven em JSON.
  - `ItemPickup.cs`: Responsavel unicamente pela coleta de itens e adicao a mochila.
- Como resultado, os objetos de cena funcionam como blocos modulares (estilo Lego), mantendo o codigo limpo e facil de testar.

---

## 2. Estruturas de Dados Sem Dicionarios (Anti-Spaghetti)

**Diretriz Central:**
O uso de `Dictionary<TKey, TValue>` nativo do C# nao e serializado no Inspector do Unity e dificulta a visualizacao e o salvamento.

**Solucao Implementada:**
- Todas as estruturas de estado foram convertidas para `List<T>` com classes serializaveis (`[System.Serializable]`):
  - `GlobalStateManager`: Gerencia `List<GameFlag>` (flags booleanas) e `List<Reputacao>` (afinidades numericas).
  - `SaveManager`: Serializa as listas e o inventario em formato JSON limpo e direto (`Application.persistentDataPath/savegame.json`).
  - Metodos utilitarios utilizam busca com predicados seguros (`Find`) e criacao dinamica sob demanda caso a flag ou reputacao ainda nao existam na lista.

---

## 3. Modernizacao do Sistema de Input (Unity Input System)

**Evolucao:**
Anteriormente, o `MovementScript` e o `InteractionSystem` liam o teclado diretamente via polling de hardware (`Keyboard.current`).

**Melhorias Implementadas:**
- Migracao completa para `InputActionReference`:
  - `moveAction` (Vector2 para WASD e controles analogicos).
  - `sprintAction` (Botao para corrida com multiplicador).
  - `interactAction` (Botao para interacao fisica).
- Criacao do `InputRebindManager.cs` em `Assets/Scripts/SistemasBase/Input/`:
  - Suporte a remapeamento dinamico de teclas em tempo de execucao.
  - Salvamento e carregamento de sobreposicoes de vinculacao (binding overrides) em JSON.
  - Cancelamento e descarte de operacoes pendentes em `OnDisable()` e `OnDestroy()` para eliminar memory leaks.

---

## 4. Sistema de Reputacao e Relacionamento

**Implementacao:**
- Criado o `ReputationSystem.cs` como singleton (`ReputationSystem.Instance`).
- Integrado ao `GlobalStateManager` atraves da classe serializavel `Reputacao`:
  - Armazena nome do personagem/faccao e valor numerico inteiro.
  - Metodos dedicados para somar e consultar reputacao (`AdicionarReputacao`, `ObterReputacao`).
- Integrado ao `SaveManager`:
  - Reputacoes salvas e restauradas com sucesso durante o ciclo de save/load.

---

## 5. Estabilidade do Sistema de Dialogos e Cutscenes

**Refinamentos Tecnicos:**
1. **Efeito Typewriter Retro Suave:**
   - O uso de `maxVisibleCharacters` do TextMeshPro foi substituido pela concatenacao progressiva caractere a caractere (`dialogueText.text += letter`).
   - O texto e limpo imediatamente no inicio (`dialogueText.text = ""`), eliminando o artefato visual ("New Text") antes da primeira fala.
2. **Prevencao de Softlock em Cutscenes:**
   - Adicionada chamada de garantia ao evento `onDialogueComplete` no `OnDisable()` do `DialogueSystemGlobal`, assegurando que corrotinas de cutscenes nao fiquem travadas caso o painel seja desativado abruptamente.
   - Adicionado tempo limite de seguranca em `CutsceneManager.Instance.TocarDialogoEEsperar` para destravar o fluxo em caso de falha de fechamento.
3. **Preservacao de Referencias no Inspector:**
   - Uso de `[FormerlySerializedAs]` em campos renomeados (ex: `dialogoPensamento` em `WakeUpPrototype.cs`) para evitar a perda de ligacoes em cenas salvas (`0.unity`).

---

## 6. Padroes de Seguranca de Codigo (Regras do Projeto)

- **Null Checks em Campos Serializados:** Todos os campos `[SerializeField]` passam por checagem nula antes do uso, evitando `NullReferenceException` quando componentes nao forem configurados no Inspector.
- **Limpeza de Singletons:** Todo singleton MonoBehaviour (`ReputationSystem`, `CutsceneManager`) limpa sua referencia estatica no `OnDestroy()`.
- **Codificacao e Comentarios:** Todo o codigo C# e comentarios utilizam ASCII puro para garantir estabilidade multiplataforma e evitar corrupcao de arquivos no Windows.

---

## 7. Code Review Geral (06/11/2026)

Revisao de todos os scripts de `Assets/Scripts/` e `Assets/Tests/`, da cena `0.unity`, dos JSONs de dialogo e de todos os docs. Nenhuma logica foi alterada: os achados estao marcados no codigo com `// REVIEW:` (busque por `REVIEW` no VS Code) e campos/metodos sem comentario ganharam comentario no padrao do projeto.

### 7.1. Bugs (corrigir antes de montar o Prologo)
1. **Itens somem no load** (`SaveManager.cs`): o codigo busca `Resources/Itens`, mas a pasta e `Assets/Resources/Items`. Todo item salvo e descartado e a mochila volta vazia. Correcao: renomear a pasta para `Itens` no Unity.
2. **Estado de teste vaza para o jogo** (`GlobalStateManager`, `InventoryData`): ScriptableObjects alterados em Play Mode ficam salvos no asset. `MemoriaDoJogo.asset` ja esta commitado com `portaAberta = true` e `aceitaAjudarTobias = true`, e `Mochila 1` com a Chave: a build comecaria com a porta aberta. Correcao: limpar os assets e criar um `Resetar()` chamado no Novo Jogo.
3. **O Enter da ultima fala pode escolher a opcao 0 sozinho** (`ChoiceSystemGlobal` + `DialogueSystemGlobal`): os dois usam o mesmo action `Submit` e nao ha ordem de execucao definida. Correcao: ignorar o submit no frame em que a tela de escolha abriu.
4. **A trava de 0.2s so vale no primeiro dialogo** (`DialogueSystemGlobal.podeAvancar`): nunca volta a `false`. Como `Interact` e `Submit` estao ambos no Enter, o Enter que abre um dialogo pode pular a 1a fala. Correcao: `podeAvancar = false` em `IniciarDialogo`.
5. **Jogador interage durante cutscene** (`InteractionSystem`): so bloqueia com o Panel aberto. Na pausa de 2s do `WakeUpPrototype` o jogador pode abrir outro dialogo e o `DialogueTrigger` destrava o movimento; se a cutscene abrir o proximo dialogo por cima, as duas digitacoes escrevem no mesmo texto. Correcao: checar `podeMover` no `InteractionSystem`.
6. **NullReferenceException ao destruir durante um rebind** (`InputRebindManager.CancelarRebindAtivo`): `Cancel()` ja zera o campo pelo callback. Correcao: `activeRebindOperation?.Dispose()`.
7. **A build provavelmente quebra por causa dos testes** (`Assets/Tests/GlobalStateManagerTests.cs`): sem asmdef nem pasta Editor, entra no Assembly-CSharp com `using NUnit`. Correcao: mover para `Assets/Tests/Editor/` (confirmar com um Build).

### 7.2. Riscos e decisoes de design
- `SaveManager` carrega no `Start`, junto com outros `Start` que leem estado (ex: `FlowerStateListener`). Mover o load para o `Awake`.
- Save corrompido e sobrescrito no `OnApplicationQuit` sem backup. No Editor cada Play continua o anterior (load no Start + save ao parar) e nao ha como testar do zero.
- `GlobalStateManager` mantem dicionarios paralelos as listas: editar uma flag no Inspector durante o Play nao tem efeito. Com ~30 flags, `List.Find` basta.
- O timeout da escolha grava `escolhaNeutra` na mesma flag bool: "nao respondeu" fica igual a Sim/Nao e a Rota N nao da para detectar.
- `FlowerStateListener` so tem 2 sprites e so le no `Start`; as 6 flags `florXxxDia1` precisam de uma lista flag->sprite e de alguem chamando `AtualizarEstadoFlor`.
- Varios componentes dao `Enable/Disable` no mesmo InputAction (`Move` e compartilhado por `MovementScript` e `InteractionSystem`): desligar um desliga o outro. Melhor habilitar o mapa `Gameplay` uma vez.
- Cena `0.unity`: sem `ReputationSystem` (a reputacao dos JSONs e ignorada sem aviso), sem `InputRebindManager` (binds salvos nao carregam), Panel de dialogo comeca ativo, e o Build Settings so tem a `SampleScene`.
- `Interactable` nao e lido por nenhum script.

### 7.3. Docs desatualizados em relacao ao codigo
| Doc diz | Codigo real |
|---|---|
| `GameFlag { flagName, isTrue }`, `DefinirFlag/ObterFlag` | `VariavelNarrativa { nome, valor }`, `SetVariavel/GetVariavel/AddReputacao/GetReputacao`, `Reputacao.pontos` |
| "Nao usa Dicionarios" | usa dicionarios privados como cache |
| `InventoryData.AdicionarItem/RemoverItem/ContemItem/Limpar` | `Additem/AddItem/RemoveItem` |
| `ItemData { nomeItem, descricao, icone }` | `itemName, itemDescription, itemIcon` |
| `MovementScript { speed, runMultiplier }`, `MovementScript.podeMover = false` | `baseSpeed, shiftMultiplier`; `podeMover` e de instancia (usar `CutsceneManager.Instance.TravarJogador()`) |
| `InteractionSystem.rayDistance`, tecla E / Gamepad South | `interactionRange`, so Enter no `PlayerControls` |
| `ReputationSystem.globalState` | `stateManager` |
| `DialogueTrigger.dialogueJSON`, `onlyOnce` | `dialogueJson`, sem `onlyOnce` |
| `FlowerStateListener { targetFlag, activeSprite, defaultSprite, AtualizarEstadoVisual() }` | `variavelFlor, spriteViva, spriteMurcha, AtualizarEstadoFlor()` |
| `CameraFollow` com limites min/max | nao tem limites |
| `CutsceneManager` com tempo limite | nao tem timeout, so checa o Panel inativo |
| `InputRebindManager` cancela no `OnDisable` | so no `OnDestroy` |
| `PostProcessingManager` "Concluido" | classe vazia |
| JSON do `Docs/README.md`: `variavelParaAlterar, textoPergunta, variavelDaEscolha, temTimer` | `variavelAlteradaPorDialogo, variavelParaSalvar, temTempoLimite` (o JsonUtility ignora o resto sem aviso) |
| `.agents/rules`: pasta `Resources/Itens`; "DialogueSystemGlobal destrava o jogador" | a pasta e `Items`; quem destrava e o callback do `DialogueTrigger` |
