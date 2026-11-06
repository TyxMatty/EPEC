# EPEC: Plano de Desenvolvimento e Divisao de Tarefas

Matty aqui! Este e o plano atualizado com base nos sistemas implementados e no roteiro do Prologo e Capitulo 1.

O objetivo deste plano e garantir progresso modular constante, sem acoplamentos perigosos (*spaghetti code*), com base em nossos **ScriptableObjects** (Listas) e eventos limpos.

---

## O que ja temos (Nossa Base Solida):
- [x] Movimentacao 2D e Animacao Manual (`MovementScript`, `SpriteChange`).
- [x] Migracao para Unity Input System moderno e Rebind em tempo de execucao (`InputRebindManager`).
- [x] Sistema de Cameras Dinamica e por Salas (`CameraFollow`, `SceneFollow`).
- [x] Interacao Desacoplada e Coleta de Itens (`InteractionSystem`, `Interactable`, `DialogueTrigger`, `ItemPickup`).
- [x] Inventario com Carregamento Dinamico via Resources (`InventoryData`, `ItemData`).
- [x] Dialogos em JSON com Typewriter Suave e Condicoes de Inventario (`DialogueSystemGlobal`, `DialogueData`).
- [x] Painel de Escolhas com Temporizador Regressivo (`ChoiceSystemGlobal`).
- [x] Sistema de Cutscenes e Dialogos Standalone com Corrotinas (`CutsceneManager`, `WakeUpPrototype`).
- [x] Sistema de Reputacao com Persistencia (`ReputationSystem`, `GlobalStateManager`).
- [x] Sistema de Save/Load em JSON com integracao total (`SaveManager`).
- [x] Reatividade Visual de Flores com 2 sprites (`FlowerStateListener`). O `PostProcessingManager` ainda e uma classe vazia.
- [x] Condicoes, Efeitos, Rota (B/N/R), Instabilidade e Nome do jogador (`Condicao`, `Efeito`, `GlobalStateManager`, `NomeDoJogadorUI`).
- [x] Dialogo v2: falante, retrato por expressao/rota, condicao por fala e opcoes ramificadas (`DialogueSystemGlobal`, `CharacterData`).

---

## Roteiro ate o Capitulo 1 (plano de 06/11/2026)

Levantado a partir do roteiro revisado (`Entre Patas e Caminhos (revisado).docx`). Cada fase destrava a seguinte.

### Fase 1 - Dados do roteiro (CONCLUIDA)
- [x] **Condicoes e Efeitos:** flag, reputacao minima, item e rota; listas E/OU; efeitos de flag, reputacao, rota e instabilidade.
- [x] **Dialogo v2:** personagem + expressao por fala, retrato (`CharacterData` em `Resources/Personagens`), `{X}`, condicao por fala.
- [x] **Escolhas ramificadas:** opcoes com condicao, efeitos e `proximoDialogo` (JSONs em `Resources/Dialogos`).
- [x] **Rota + instabilidade:** uma rota so (`rota`), `MudarRota` B -> N -> R, medidor `instabilidade`, tudo no save.
- [x] **Nome do jogador:** `NomeDoJogadorUI`, chamado pelo `WakeUpPrototype` em jogo novo.
- [x] Exemplo real: abrigo do Cap 1 (Decisoes 1.1 e 1.2) em `cap1_abrigo_apresentacao.json`.
- [ ] Na cena: criar `nomeText`/`retratoImage` no Panel de dialogo, o painel da `NomeDoJogadorUI` e os `CharacterData` de X, Maria, Gi, Tobias e enfermeira.

### Fase 2 - Montar as cenas
- [ ] **Troca de cenas:** `SceneTransitionManager` com fade e ponto de spawn + prefab "Nucleo" (player, camera, UI, managers) com DontDestroyOnLoad.
- [ ] **NPCs em cutscene:** `MoverPara(posicao)` e `SeguirAlvo(distancia)` (Tobias mais perto se `fezCarinhoTobias`).
- [ ] **Fim do dia:** `EncerrarDia()` com fade, calculo da flor, save e proxima cena (desmaio e Rota R profunda).
- [ ] **Sprites com Animator:** controller base `Humano4Dir` e `Quadrupede4Dir` (Blend Trees Idle/Walk/Run por DirX/DirY + estados avulsos como Abracar, Girar, Chutar, Latir), um `AnimatorOverrideController` por personagem, override extra para a Rota R e um `AnimacaoPorMovimento` substituindo o `SpriteChange`. Importacao: mesmo Pixels Per Unit, Filter Point, Compression None, pivot nos pes, Sprite Atlas por personagem e Transparency Sort Axis (0,1,0) no Renderer2D.

### Fase 3 - Mecanicas do Cap 1
- [ ] **QTE generico:** sequencia de teclas com janela de tempo; agressor (sucesso/falha -> `agressorEscapa`) e ataque de panico (mais de 3 falhas -> desmaio).
- [ ] **Investigacao (juntar as pontas):** `PistaData`, `CasoData`, gatilho de pista e tela do quadro.
- [ ] **Flores v2:** lista de regras (condicoes -> sprite), reacao a mudanca de estado e calculo da flor do dia pelas 6 regras do roteiro.

### Fase 4 - Clima e tutorial
- [ ] **Pos-processamento:** Volumes do URP por rota, flashback, panico e blur da cidade.
- [ ] **Audio:** `AudioManager` com crossfade de musica por rota e SFX (alarme, celular, moto, latido).
- [ ] **Seguranca e Bem-estar por animal:** dois numeros por animal + painel do tutorial do fim do Cap 1.

---

## Proximos Passos (Tarefas a Executar)

### Tarefa 1: O Coracao Narrativo (GameState Baseado em Listas)
- [x] **Criar GameStateData / GlobalStateManager:** ScriptableObject com `List<GameFlag>` e `List<Reputacao>`.
- [x] **Atualizar ChoiceSystemGlobal.cs:** Refatorar escolhas para ler e gravar nas listas.

### Tarefa 2: Reflexos Visuais do Mundo (O Sistema de Flores e Rota R)
- [x] **Criar FlowerStateListener.cs:** Script que monitora flags e troca sprites de flores no cenario.
- [ ] **Criar PostProcessingManager.cs:** Ajustes de pos-processamento (blur, vinheta, cores) para rotas criticas. (Arquivo criado, mas vazio; ver Fase 4.)

### Tarefa 3: Montagem do Prologo na Cena (Greyboxing e Multi-Cenas)
Dividir os mapas para facilitar versionamento, organizacao e desempenho:
- [ ] **Estrutura de Cenas:** Separar `Prologo_Quarto`, `Prologo_Jardim` e `Prologo_Rua`.
- [ ] **Greyboxing Inicial:** Blocos cinzas, colisores e posicionamento de triggers.
- [ ] **Gerenciador de Transicao:** Desenvolver `SceneTransitionManager` com fade e carregamento assincrono aditivo.
- [ ] Integrar cutscene inicial com `WakeUpPrototype` na nova cena do quarto.

### Tarefa 4: Sistema de Save / Persistencia com JSON
- [x] **Criar SaveManager.cs:** Serializacao completa de flags, reputacao e inventario.
- [x] **Otimizacao e Tratamento de Erros:** Guardas com try/catch e null checks rigorosos.

### Tarefa 5: Sistemas de Gameplay Especificos do Cap 1 (QTE e Investigacao)
- [ ] **Criar Mecanica de Ataque de Panico (QTE):**
  - Script `PanicAttackSystem.cs` com janela de tempo para reacao de botoes.
  - Maximo de 3 falhas antes do desmaio do protagonista.
- [ ] **Criar Mecanica de Investigacao (Juntar as Pontas):**
  - Interface que permite selecionar pistas e combina-las logicamente para conclusao.
- [ ] **Criar Mecanica de QTEs
  - QTES.
### Tarefa 6: Escalabilidade do DialogueSystem e Cutscenes
- [x] **Desacoplar o DialogueSystemGlobal:** Eliminacao do "God Object", uso de triggers independentes.
- [x] **Criar CutsceneManager.cs Global:** Suporte a corrotinas de espera (`TocarDialogoEEsperar`).
- [x] **Suavidade Retro do Typewriter:** Revertido para concatenacao caractere a caractere (`dialogueText.text += letter`).

### Tarefa 7: Sistema de Reputacao Global
- [x] **ReputationSystem.cs:** Fachada estatica com busca segura e soma/subtracao de afinidade.
- [x] **Integracao com SaveManager:** Salvo e carregado em arquivo JSON.

### Tarefa 8: Modernizacao de Input e Rebinding
- [x] **Migracao para InputActionReference:** `MovementScript` e `InteractionSystem`.
- [x] **Criacao de InputRebindManager:** Modificacao e persistencia de bindings em tempo de execucao.

### Tarefa 9 (Escopo Futuro): Sistemas do Caminho do Acolhimento
*Nota: A serem desenvolvidos apenas apos a conclusao do Capitulo 1.*
- [ ] **Sistema de Seguranca:** Risco de acidentes, locais perigosos, etc.
- [ ] **Sistema de Bem-Estar:** Alimentacao, agua, abrigo e descanso dos animais.

---

## Ordem de Execucao para a Proxima Sessao

Com a Fase 1 pronta, a prioridade sera:
1. **Ligar a Fase 1 na cena:** nome/retrato no Panel, tela de nome e `CharacterData` dos personagens; rodar os testes no Test Runner.
2. **Fase 2:** `SceneTransitionManager` + prefab Nucleo, greyboxing do Prologo (Tarefa 3) e o pipeline de sprites com Animator.
3. **Fase 3:** QTE generico (agressor e ataque de panico) e investigacao.
