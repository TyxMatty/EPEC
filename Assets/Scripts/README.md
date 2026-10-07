# Documentacao Tecnica de Scripts - Projeto EPEC

Este documento detalha a arquitetura tecnica, a organizacao de diretorios e o funcionamento dos scripts no diretorio `Assets/Scripts/`. Todos os sistemas foram desenvolvidos com foco em modularidade, desacoplamento e manutencao limpa (Anti-Spaghetti).

---

## 1. Mapa de Diretorios

```text
Assets/Scripts/
|-- Cameras/
|   |-- CameraFollow.cs           # Camera suave com lerp e foco no alvo
|   +-- SceneFollow.cs            # Camera por salas (bounding box ortografico)
|-- Interactions/
|   |-- ChoiceSystemGlobal.cs     # Painel de escolhas ramificadas com timer
|   |-- Dialogos/
|   |   |-- DialogueData.cs       # Estruturas de dados e classes para JSON
|   |   |-- DialogueSystemGlobal.cs # Maquina de escrever retro e controle de UI
|   |   +-- DialogueTrigger.cs    # Gatilho de dialogo baseado em JSON
|   |-- IInteractAction.cs        # Interface para acoes polimorficas de interacao
|   |-- Interactable.cs           # Componente base detectado pelo Raycast
|   |-- InteractionSystem.cs      # Raycast do jogador e disparo de acoes
|   |-- InventoryData.cs          # ScriptableObject de inventario (mochila)
|   |-- ItemData.cs               # ScriptableObject de definicao de item
|   +-- ItemPickup.cs             # Gatilho de coleta de itens do cenario
|-- SistemasBase/
|   |-- Escolhas/
|   |   +-- SaveManager.cs        # Persistencia de flags, reputacao e inventario
|   |-- Input/
|   |   +-- InputRebindManager.cs # Gerenciador de remapeamento dinamico de inputs
|   |-- MovementScript.cs         # Fisica de movimento 2D com Input System
|   +-- SpriteChange.cs           # Animacao orientada por velocidade e direcao
|-- CutsceneManager.cs            # Orquestrador de cutscenes e sequencias lineares
|-- FlowerStateListener.cs        # Reatividade visual de sprites baseada em flags
|-- GlobalStateManager.cs         # ScriptableObject central de flags e reputacoes
|-- PostProcessingManager.cs      # Controle de efeitos visuais e pos-processamento
|-- ReputationSystem.cs           # Singleton gerenciador de afinidades e reputacao
+-- WakeUpPrototype.cs            # Sequencia scriptada do prologo (acordar do protagonista)
```

---

## 2. Modulos e Componentes

### 2.1. Movimentacao e Input (`SistemasBase/`)

- **`MovementScript.cs`**:
  - Controla o movimento top-down do jogador com aceleracao linear no `Rigidbody2D`.
  - Integrado ao Unity Input System moderno via `InputActionReference` (`moveAction`, `sprintAction`).
  - Possui a propriedade publica `podeMover` (bool). Quando definida como `false`, o script zera a velocidade fisica imediatamente, travando o player durante dialogos ou cutscenes.
  - Implementa protecao de nulidade caso as `InputActionReference` nao estejam associadas no Inspector.

- **`SpriteChange.cs`**:
  - Atualiza os frames de animacao sem usar o componente Animator do Unity, eliminando alocacao desnecessaria de memoria.
  - Avalia a velocidade (`linearVelocity`) para alternar entre estados: parado, caminhando e correndo em 4 direcoes cardeais.

- **`InputRebindManager.cs`** (`SistemasBase/Input/`):
  - Permite remapear teclas e botoes em tempo de execucao usando `InputActionRebindingExtensions`.
  - Salva e restaura sobreposicoes de vinculacao no formato JSON (`SaveBindingOverrides` / `LoadBindingOverrides`).
  - Possui descarte seguro de operacoes assincronas em `OnDisable()` e `OnDestroy()` para evitar vazamento de memoria.

---

### 2.2. Gerenciamento de Estado e Persistencia

- **`GlobalStateManager.cs`** (ScriptableObject):
  - Mantem a lista serializavel de flags booleanas (`List<GameFlag>`) e a lista de reputacoes (`List<Reputacao>`).
  - Evita o uso de Dicionarios para garantir total visibilidade no Inspector do Unity e facilitar a serializacao para JSON.
  - Inclui metodos seguros: `DefinirFlag`, `ObterFlag`, `AdicionarReputacao`, `ObterReputacao`, com busca segura e auto-inicializacao em `OnEnable()`.

- **`ReputationSystem.cs`** (Singleton MonoBehaviour):
  - Atua como fachada (Facade) acessivel globalmente (`ReputationSystem.Instance`) para manipular a reputacao de NPCs e faccoes.
  - Consulta e modifica os valores diretamente no `GlobalStateManager`.
  - Limpa sua instancia estatica no ciclo de vida `OnDestroy()`.

- **`SaveManager.cs`** (`SistemasBase/Escolhas/`):
  - Salva e carrega o estado da sessao em arquivo JSON no caminho `Application.persistentDataPath/savegame.json`.
  - Serializa as flags globais, os valores de reputacao e os IDs de itens presentes no `InventoryData`.
  - Ao carregar, reconecta automaticamente os assets de `ItemData` a partir da pasta `Resources/Itens/`.

---

### 2.3. Interacao, Itens e Escolhas (`Interactions/`)

- **`InteractionSystem.cs`**:
  - Executado no jogador. Dispara um `Physics2D.Raycast` na direcao do ultimo movimento registrado.
  - Ao detectar um collider na camada `interactableLayer`, exibe o prompt visual (`interactionPrompt`).
  - Ao pressionar a acao de interacao (`interactAction`), invoca o metodo de interacao do objeto alvo.

- **`Interactable.cs`**:
  - Componente base anexado aos objetos de cenario para identificacao pelo Raycast.
  - Oferece suporte a componentes polimorficos que implementem `IInteractAction`.

- **`InventoryData.cs`** e **`ItemData.cs`**:
  - `InventoryData`: ScriptableObject que gerencia a lista de itens coletados pelo jogador. Possui metodos utilitarios como `AdicionarItem`, `RemoverItem`, `ContemItem`, `Limpar` e um alias retrocompativel `AddItem`.
  - `ItemData`: ScriptableObject que descreve propriedades basicas do item (`itemID`, `nomeItem`, `descricao`, `icone`).

- **`ItemPickup.cs`**:
  - Script anexado a itens fisicos no cenario. Ao ser acionado, adiciona o `ItemData` associado a mochila (`InventoryData`) e desativa o objeto no cenario.

- **`ChoiceSystemGlobal.cs`**:
  - Gerencia a exibicao de opcoes de escolha na tela com suporte opcional a temporizador regressivo.
  - Modifica flags no `GlobalStateManager` ou adiciona itens no `InventoryData` dependendo da decisao tomada pelo jogador.

---

### 2.4. Dialogos e Cutscenes (`Interactions/Dialogos/` e Raiz)

- **`DialogueSystemGlobal.cs`**:
  - Controla o painel de dialogo e o efeito de escrita caractere por caractere (retro typewriter).
  - Executa interpolacao suave de transparencia via `CanvasGroup`.
  - Processa dialogos condicionais: se o jogador possuir determinado item no inventario, exibe falas alternativas e pode alterar flags especificas (`dialogoAlternativoAlteraVariavel`).
  - Garante chamada de encerramento (`onDialogueComplete`) mesmo quando desativado subitamente (`OnDisable`), prevenindo travamento do jogador em cutscenes.

- **`DialogueTrigger.cs`**:
  - Conecta um arquivo JSON (`DialogueData`) ao mundo fisico atraves da interface de interacao.

- **`CutsceneManager.cs`**:
  - Singleton que fornece utilitarios para sequencias scriptadas.
  - Metodo `TocarDialogoEEsperar(TextAsset json)` roda uma Coroutine que aguarda a conclusao do dialogo antes de prosseguir com a execucao.
  - Inclui guardas de tempo limite para evitar que a cutscene fique em estado de deadlock caso a janela de dialogo feche inesperadamente.

- **`WakeUpPrototype.cs`**:
  - Implementa a sequencia inicial do despertar do protagonista.
  - Utiliza `[FormerlySerializedAs("dialogopensamento")]` para preservar referencias configuradas em cenas legadas.

---

### 2.5. Cameras e Efeitos (`Cameras/` e Raiz)

- **`CameraFollow.cs`**:
  - Segue suavemente o jogador utilizando interpolacao linear (`Vector3.Lerp`) dentro de `LateUpdate`.
  - Possui limites configuraveis (min e max para eixos X e Y).

- **`SceneFollow.cs`**:
  - Implementa transicao de camera por salas baseado nas dimensoes da tela (`orthographicSize` e proporcao de aspecto).

- **`FlowerStateListener.cs`**:
  - Monitora uma flag especifica no `GlobalStateManager` e troca o sprite do elemento floral para refletir o estado emocional ou narrativo do mundo.

- **`PostProcessingManager.cs`**:
  - Aplica ajustes de pos-processamento (como blur, saturacao e vinheta) em resposta a rotas narrativas especificas (ex: Rota R).

---

## 3. Diretrizes de Desenvolvimento

1. **Serializacao de Listas:** Todas as estruturas que precisam ser salvas ou visualizadas no Inspector devem usar `List<T>` com classes decoradas com `[System.Serializable]`.
2. **Null Safety Obrigatoria:** Todo acesso a campos serializados e singletons deve conter validacao previa contra nulo.
3. **Padrao de Nomenclatura e Codificacao:** Classes e arquivos usam `PascalCase`. Comentarios e mensagens usam ASCII puro.