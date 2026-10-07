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
- [x] Reatividade Visual de Flores e Pos-Processamento (`FlowerStateListener`, `PostProcessingManager`).

---

## Proximos Passos (Tarefas a Executar)

### Tarefa 1: O Coracao Narrativo (GameState Baseado em Listas)
- [x] **Criar GameStateData / GlobalStateManager:** ScriptableObject com `List<GameFlag>` e `List<Reputacao>`.
- [x] **Atualizar ChoiceSystemGlobal.cs:** Refatorar escolhas para ler e gravar nas listas.

### Tarefa 2: Reflexos Visuais do Mundo (O Sistema de Flores e Rota R)
- [x] **Criar FlowerStateListener.cs:** Script que monitora flags e troca sprites de flores no cenario.
- [x] **Criar PostProcessingManager.cs:** Ajustes de pos-processamento (blur, vinheta, cores) para rotas criticas.

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

## Ordem de Execucao para a Proxima Sessao (Amanha)

Com os sistemas base e a arquitetura 100% estaveis e documentados, a prioridade sera:
1. **Greyboxing do Prologo (Tarefa 3):** Criar as cenas separadas e testar o fluxo de navegacao fisica.
2. **Sistema de Transicao de Cenas (`SceneTransitionManager`):** Fazer a troca suave entre o quarto e o jardim.
3. **Mecanica de Ataque de Panico / QTE (Tarefa 5):** Implementar o mini-game de respiracao/panico.
