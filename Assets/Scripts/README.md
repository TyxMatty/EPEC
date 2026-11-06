# Documentacao Tecnica de Scripts - Projeto EPEC

Este documento detalha a arquitetura tecnica, a organizacao de diretorios e o funcionamento dos scripts no diretorio `Assets/Scripts/`. Todos os sistemas foram desenvolvidos com foco em modularidade, desacoplamento e manutencao limpa (Anti-Spaghetti). O guia de uso no Inspector e o formato dos JSONs ficam em `Docs/README.md`.

---

## 1. Mapa de Diretorios

```text
Assets/Scripts/
|-- Cameras/
|   |-- CameraFollow.cs           # Camera suave com lerp seguindo o alvo
|   +-- SceneFollow.cs            # Camera por salas (grade do tamanho da tela)
|-- Interactions/
|   |-- ChoiceSystemGlobal.cs     # Painel de escolhas com timer; devolve a opcao escolhida
|   |-- Dialogos/
|   |   |-- CharacterData.cs      # ScriptableObject de personagem (nome, cor, retratos)
|   |   |-- DialogueData.cs       # Classes do JSON de dialogo (v2: Fala, Opcao)
|   |   |-- DialogueSystemGlobal.cs # Maquina de escrever retro, falante e encadeamento
|   |   +-- DialogueTrigger.cs    # Gatilho de dialogo baseado em JSON
|   |-- IInteractAction.cs        # Interface para acoes polimorficas de interacao
|   |-- Interactable.cs           # Marcador opcional (nenhum script le hoje)
|   |-- InteractionSystem.cs      # Raycast do jogador e disparo de acoes
|   |-- InventoryData.cs          # ScriptableObject de inventario (mochila)
|   |-- ItemData.cs               # ScriptableObject de definicao de item
|   +-- ItemPickup.cs             # Gatilho de coleta de itens do cenario
|-- Editor/
|   +-- GeradorAnimatorBase.cs    # Menu EPEC > Criar Animator Base (Humano4Dir / Quadrupede4Dir)
|-- SistemasBase/
|   |-- Cenas/
|   |   |-- FimDoDia.cs           # Resumo do dia por regras, save e proxima cena
|   |   |-- PontoDeSpawn.cs       # Onde o player aparece ao entrar na cena
|   |   |-- PortaDeCena.cs        # Troca de cena por interacao ou trigger
|   |   +-- SceneTransitionManager.cs # Raiz do prefab Nucleo: persiste entre cenas e troca com fade
|   |-- Escolhas/
|   |   +-- SaveManager.cs        # Persistencia de flags, reputacao, rota, nome e inventario
|   |-- Input/
|   |   +-- InputRebindManager.cs # Gerenciador de remapeamento dinamico de inputs
|   |-- AnimacaoPorMovimento.cs   # Alimenta o Animator (DirX, DirY, Velocidade) pelo Rigidbody2D
|   |-- MovementScript.cs         # Fisica de movimento 2D com Input System
|   |-- MovimentoNPC.cs           # MoverPara / SeguirAlvo para NPCs em cutscene
|   |-- NomeDoJogadorUI.cs        # Tela onde o jogador escolhe o nome ({X})
|   +-- SpriteChange.cs           # Animacao manual por sprite (ate os sprites finais chegarem)
|-- Condicao.cs                   # Checagem de flag/reputacao/item/rota usada por dados
|-- CutsceneManager.cs            # Orquestrador de cutscenes e sequencias lineares
|-- Efeito.cs                     # Mudanca de flag/reputacao/rota/instabilidade usada por dados
|-- Regra.cs                      # Condicoes -> efeitos; ListaDeRegras aplica a primeira que bate
|-- FlowerStateListener.cs        # Reatividade visual de sprites baseada em flags
|-- GlobalStateManager.cs         # ScriptableObject central de flags, reputacoes, rota e nome
|-- PostProcessingManager.cs      # Reservado (ainda vazio) para efeitos de pos-processamento
|-- ReputationSystem.cs           # Singleton de popup de afinidade
+-- WakeUpPrototype.cs            # Sequencia scriptada do prologo (acordar do protagonista)
```

---

## 2. Modulos e Componentes

### 2.1. Movimentacao e Input (`SistemasBase/`)

- **`MovementScript.cs`**:
  - Controla o movimento top-down do jogador no `Rigidbody2D` (`baseSpeed`, `shiftMultiplier`).
  - Integrado ao Unity Input System moderno via `InputActionReference` (`moveAction`, `sprintAction`).
  - Possui a propriedade publica `podeMover` (bool). Quando `false`, o script zera a velocidade fisica e o `InteractionSystem` deixa de interagir, travando o player durante dialogos ou cutscenes.
  - Implementa protecao de nulidade caso as `InputActionReference` nao estejam associadas no Inspector.

- **`SpriteChange.cs`**:
  - Atualiza os sprites sem usar o componente Animator do Unity.
  - Avalia a velocidade (`linearVelocity`) para alternar entre estados: parado, transicao, caminhando e correndo em 4 direcoes cardeais.

- **`InputRebindManager.cs`** (`SistemasBase/Input/`):
  - Permite remapear teclas e botoes em tempo de execucao usando `InputActionRebindingExtensions`.
  - Salva e restaura sobreposicoes de vinculacao no formato JSON (`SaveBindingOverridesAsJson` / `LoadBindingOverridesFromJson`) via PlayerPrefs.
  - Cancela e descarta a operacao pendente em `OnDestroy()`.

- **`NomeDoJogadorUI.cs`**:
  - Painel com `TMP_InputField`; `Confirmar()` grava `GlobalStateManager.nomeJogador` (sem `<` `>` e sem nome vazio).
  - `PedirNome()` e uma corrotina para cutscenes; `FaltaNome` indica jogo novo.

- **`MovimentoNPC.cs`**:
  - Move pelo `Rigidbody2D.linearVelocity` (o `AnimacaoPorMovimento` anima igual ao player).
  - `MoverPara(ponto, tempoMaximo = 10)` e corrotina e teleporta se nao chegar a tempo; `SeguirAlvo(alvo, distancia)` / `PararDeSeguir()`.

- **`AnimacaoPorMovimento.cs`**:
  - Le a velocidade do `Rigidbody2D` e escreve `DirX`/`DirY` (ultima direcao, parado continua virado) e `Velocidade` no Animator.
  - Os controllers base saem do `Editor/GeradorAnimatorBase.cs`; cada personagem usa um `AnimatorOverrideController`.


---

### 2.2. Gerenciamento de Estado e Persistencia

- **`GlobalStateManager.cs`** (ScriptableObject):
  - Mantem as listas serializaveis `variaveisGlobais` (`List<VariavelNarrativa>` com `nome`/`valor`) e `reputacoes` (`List<Reputacao>` com `nome`/`pontos`), mais `rota`, `instabilidade` e `nomeJogador`.
  - Listas no Inspector para visibilidade e JSON; dicionarios privados so como cache de busca.
  - Metodos: `SetVariavel`, `GetVariavel`, `AddReputacao`, `GetReputacao`, `DefinirRota`, `MudarRota`, `AddInstabilidade`, `Resetar`, `NomeExibido`.
  - `RotaB/RotaN/RotaR` sao derivadas de `rota` (uma rota por vez).

- **`Condicao.cs`** e **`Efeito.cs`**:
  - Classes serializaveis lidas dos JSONs. `Condicao.Valem(todas, qualquer, estado, mochila)` avalia listas E/OU; `Efeito.AplicarTodos(lista, estado)` aplica flags, reputacao, rota e instabilidade.
  - Pensadas para serem reaproveitadas por flores, gatilhos e mecanicas das proximas fases.

- **`Regra.cs`**:
  - `Regra` = condicoes + efeitos. `ListaDeRegras.Aplicar` acha a primeira que bate (lendo o estado de antes), desliga as flags de `limpar` e aplica os efeitos.

- **`ReputationSystem.cs`** (Singleton MonoBehaviour):
  - Mostra o popup de afinidade e soma no `GlobalStateManager` (`ReputationSystem.Instance`).
  - Limpa sua instancia estatica no ciclo de vida `OnDestroy()`.

- **`SaveManager.cs`** (`SistemasBase/Escolhas/`):
  - Salva e carrega o estado da sessao em `Application.persistentDataPath/savegame.json`.
  - Serializa flags, reputacoes, rota, instabilidade, nome e os IDs de itens do `InventoryData`.
  - Ao carregar, reconecta os `ItemData` a partir de `Resources/Itens/`. Sem save, chama `Resetar()` e `Limpar()`.

---

### 2.3. Interacao, Itens e Escolhas (`Interactions/`)

- **`InteractionSystem.cs`**:
  - Executado no jogador. Dispara um `Physics2D.Raycast` na direcao do ultimo movimento registrado.
  - Ao detectar um collider na camada `interactableLayer`, exibe o prompt visual (`interactionPrompt`).
  - Ao pressionar `interactAction`, chama `OnInteract` de todos os `IInteractAction` do objeto. Nao interage com o dialogo aberto nem com `podeMover = false`.

- **`Interactable.cs`**:
  - Marcador com bools de categoria; o raycast usa so a layer + `IInteractAction`.

- **`InventoryData.cs`** e **`ItemData.cs`**:
  - `InventoryData`: lista `itens` com `AddItem` (alias `Additem`), `RemoveItem`, `ContemItem` e `Limpar`.
  - `ItemData`: `itemID`, `itemName`, `itemDescription`, `itemIcon`.

- **`ItemPickup.cs`**:
  - Ao ser acionado, adiciona o `ItemData` a mochila (depois do dialogo, se o objeto tiver `DialogueTrigger`) e desativa o objeto.

- **`ChoiceSystemGlobal.cs`**:
  - Mostra as opcoes com temporizador opcional e devolve o indice escolhido (-1 no tempo esgotado).
  - No formato antigo grava o bool da opcao em uma flag; no v2 os efeitos ficam na propria opcao.
  - Expoe `Estado` e `Inventario` para o `DialogueSystemGlobal`.

---

### 2.4. Dialogos e Cutscenes (`Interactions/Dialogos/` e Raiz)

- **`DialogueSystemGlobal.cs`**:
  - Controla o painel de dialogo e o efeito de escrita caractere por caractere (retro typewriter).
  - Formato v2: falas com personagem/expressao (nome e retrato via `CharacterData`), condicoes por fala, efeitos e opcoes que encadeiam outro JSON (`proximoDialogo`) no mesmo painel.
  - `{X}` e trocado pelo nome do jogador.
  - Formato antigo continua: falas alternativas por flag/item e flags ao fim.
  - Garante a chamada de encerramento mesmo quando desativado subitamente (`OnDisable`), prevenindo travamento em cutscenes.

- **`CharacterData.cs`**:
  - ScriptableObject em `Resources/Personagens/`: `nomeExibido`, `corDoNome`, `expressoes` (com variante opcional por rota).

- **`DialogueTrigger.cs`**:
  - Conecta um arquivo JSON (`DialogueData`) ao mundo fisico atraves da interface de interacao e destrava o jogador no fim.

- **`CutsceneManager.cs`**:
  - Singleton com `TravarJogador`, `LiberarJogador` e `TocarDialogoEEsperar(TextAsset json)`, que espera o fim de toda a cadeia de dialogos.
  - Se o painel de dialogo for desligado no meio, a espera e liberada (nao ha timeout).

- **`WakeUpPrototype.cs`**:
  - Sequencia inicial do despertar: pede o nome (se `telaDeNome` estiver ligado e for jogo novo), toca o pensamento e o celular.
  - Utiliza `[FormerlySerializedAs("dialogopensamento")]` para preservar referencias configuradas em cenas legadas.

---

### 2.5. Cameras e Efeitos (`Cameras/` e Raiz)

- **`CameraFollow.cs`**:
  - Segue suavemente o jogador utilizando `Vector3.Lerp` dentro de `LateUpdate` (sem limites de mapa).

- **`SceneFollow.cs`**:
  - Camera por salas baseada no tamanho da tela (`orthographicSize` e proporcao de aspecto); as salas comecam em (0,0).

- **`FlowerStateListener.cs`**:
  - Le `variavelFlor` no `GlobalStateManager` e troca entre `spriteViva` e `spriteMurcha`.

- **`PostProcessingManager.cs`**:
  - Classe reservada, ainda vazia; os efeitos por rota entram na Fase 4 do `Docs/MattyDevPlan.md`.
---

### 2.6. Cenas (`SistemasBase/Cenas/`)

- **`SceneTransitionManager.cs`**:
  - Fica na raiz do prefab `Nucleo` e o mantem com `DontDestroyOnLoad`. A copia que chega com a cena nova se desliga no `Awake` (`DefaultExecutionOrder(-1000)`) antes de os filhos acordarem e se apaga.
  - `IrPara(cena, spawn)`: fade-out na `telaPreta`, carrega, poe player e camera no `PontoDeSpawn` e faz fade-in.
- **`PortaDeCena.cs`**: `IInteractAction` (Enter) ou `OnTriggerEnter2D` (encostar), chama `IrPara`.
- **`PontoDeSpawn.cs`**: so um `id` e um gizmo.
- **`FimDoDia.cs`**: aplica um `ListaDeRegras` em JSON (ex: `Assets/Dados/Regras/flor_dia1.json`), salva pelo `SaveManager` do Nucleo e chama `IrPara`.

---

## 3. Diretrizes de Desenvolvimento

1. **Serializacao de Listas:** Todas as estruturas que precisam ser salvas ou visualizadas no Inspector devem usar `List<T>` com classes decoradas com `[System.Serializable]`.
2. **Null Safety Obrigatoria:** Todo acesso a campos serializados e singletons deve conter validacao previa contra nulo.
3. **Padrao de Nomenclatura e Codificacao:** Classes e arquivos usam `PascalCase`. Comentarios e mensagens usam ASCII puro.
