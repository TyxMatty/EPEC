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

Revisao de todos os scripts de `Assets/Scripts/` e `Assets/Tests/`, da cena `0.unity`, dos JSONs de dialogo e de todos os docs. Os 7 bugs da 7.1 foram corrigidos; o resto continua marcado no codigo com `// REVIEW:` (busque por `REVIEW` no VS Code) e campos/metodos sem comentario ganharam comentario no padrao do projeto.

### 7.1. Bugs (todos corrigidos em 06/11/2026, branch `review-geral`)
1. **Itens sumiam no load** (`SaveManager.cs`): o codigo buscava `Resources/Itens`, mas a pasta era `Assets/Resources/Items`. **Corrigido:** pasta renomeada para `Itens` (GUID mantido).
2. **Estado de teste vazava para o jogo** (`GlobalStateManager`, `InventoryData`): ScriptableObjects alterados em Play Mode ficam salvos no asset, e `MemoriaDoJogo.asset` (`portaAberta`, `aceitaAjudarTobias`) e `Mochila 1` (Chave) estavam commitados assim. **Corrigido:** `GlobalStateManager.Resetar()` e `InventoryData.Limpar()`, chamados pelo `SaveManager` quando nao existe save (Novo Jogo); assets limpos; teste `Resetar_ApagaVariaveisEReputacoes`.
3. **O Enter da ultima fala podia escolher a opcao 0 sozinho** (`ChoiceSystemGlobal`): mesmo action `Submit` do dialogo, sem ordem de execucao definida. **Corrigido:** o Submit do frame em que a tela abriu e ignorado (`frameAbertura`).
4. **A trava de 0.2s so valia no primeiro dialogo** (`DialogueSystemGlobal.podeAvancar`). **Corrigido:** `IniciarDialogo` zera `podeAvancar` e chama `StopAllCoroutines()`, entao um dialogo aberto por cima de outro tambem nao mistura digitacoes.
5. **Jogador interagia durante cutscene** (`InteractionSystem`): so bloqueava com o Panel aberto. **Corrigido:** com `MovementScript.podeMover = false` nao ha interacao nem prompt.
6. **NullReferenceException ao destruir durante um rebind** (`InputRebindManager.CancelarRebindAtivo`). **Corrigido:** `activeRebindOperation?.Dispose()`.
7. **Testes no Assembly-CSharp quebravam a build do player** (`using NUnit`). **Corrigido:** movidos para `Assets/Tests/Editor/` (agora compilam no Assembly-CSharp-Editor).

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
**Corrigido em 06/11/2026:** `Docs/README.md` e `Assets/Scripts/README.md` foram reescritos com os nomes reais de classes, campos e metodos (GlobalStateManager, InventoryData, ItemData, MovementScript, InteractionSystem, ReputationSystem, DialogueTrigger, FlowerStateListener, CameraFollow, CutsceneManager, InputRebindManager), o `PostProcessingManager` voltou a constar como pendente e o JSON de exemplo usa os campos que existem. A regra em `.agents/rules` agora diz que quem destrava o jogador e o `DialogueTrigger`.

---

## 8. Fase 1 do Roteiro: Decisoes de Design (06/11/2026)

- **Regras como dados:** `Condicao` e `Efeito` sao classes serializaveis lidas do JSON. Qualquer sistema novo (flores, gatilhos, QTE) deve reusar `Condicao.Valem` e `Efeito.AplicarTodos` em vez de criar campos proprios como `variavelDaCondicao`.
- **Uma rota so:** a rota e um campo (`rota` = B/N/R) e nao tres bools, porque o roteiro anda nela ("na B vai para N, na N vai para R"). As flags `RotaB/N/R` continuam funcionando como leitura/escrita desse campo.
- **Pastas Resources por nome:** dialogos (`Resources/Dialogos`) e personagens (`Resources/Personagens`) sao achados pelo nome escrito no JSON, sem ligar nada no Inspector.
- **Formato antigo mantido:** os JSONs e cenas existentes continuam funcionando; o v2 so entra quando `linhas`/`opcoes` estao preenchidos.
- **Sprites:** decidido usar Animator com controllers base por tipo de corpo e um `AnimatorOverrideController` por personagem (Fase 2); retratos de dialogo ficam no `CharacterData`, sem Animator.
- **Nomes de flags:** a grafia oficial esta em `Docs/MattyNotations.md` 2.1 e no roteiro revisado (`abortedRotaR`, `florQuaseMorta` mantida como status de decisao negativa maxima, `protagonistaNumeroVeterinaria`, `auxiliouTobias`, `tobiasInterage`).
