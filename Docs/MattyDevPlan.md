# EPEC: Plano de Desenvolvimento e Divisao de Tarefas

Matty aqui, eis o plano que montei baseado no que ja temos pronto (MovementScript, InteractionSystem, DialogueSystemGlobal, InventoryData, etc) e no que precisamos para dar vida ao Roteiro do Prologo e Capitulo 1.

O objetivo deste plano e garantir progresso constante, sem criar acoplamentos perigosos (*spaghetti code*). Vamos focar em modulos independentes que se comunicam atraves dos nossos **ScriptableObjects** (Listas).

---

## O que ja temos (Nossa Base Solida):
- Movimentacao e Animacao (MovementScript, SpriteChange)
- Sistema de Camera (CameraFollow, SceneFollow)
- Interacao Basica e Inventario (InteractionSystem, Interactable, InventoryData)
- Dialogo Linear, Prototipo de Escolhas e Condicoes via JSON (DialogueSystemGlobal, ChoiceSystemGlobal, DialogueData)
- Save System (SaveManager, GlobalStateManager otimizado)
- Sistema de Cutscenes Base (CutsceneManager, Bloqueio de Movimento)

---

## Proximos Passos (Tarefas a Executar)

### Tarefa 1: O Coracao Narrativo (GameState Baseado em Listas)
- [x] **Criar GameStateData.cs:** Um ScriptableObject que contera uma ListGameFlag.
- [x] **Atualizar ChoiceSystemGlobal.cs:** Refatorar o sistema de escolhas para buscar flags na lista.

### Tarefa 2: Reflexos Visuais do Mundo (O Sistema de Flores e Rota R)
- [x] **Criar FlowerStateListener.cs:** Um script que checa a lista do GameStateData e muda seu proprio sprite/cor.
- [x] **Criar RouteVisualManager.cs:** Alterar o Post-Processing (blur, cores) e a trilha sonora na Rota R.

### Tarefa 3: Montagem do Prologo na Cena (Greyboxing e Multi-Cenas)
Dividindo mapas para facilitar o versionamento e colaboracao da equipe.
- [ ] **Estrutura de Cenas:** Criar Prologo_Quarto, Prologo_Jardim e Prologo_Reuniao separadas.
- [ ] **Greyboxing Inicial:** Usar blocos cinzas/brancos e Hitboxes.
- [ ] **Transicao de Cenas:** Desenvolver um SceneTransitionManager.
- [ ] Instanciar os JSONs e Cutscenes (Cutscene_Acordar, DoorToGarden).

### Tarefa 4: Sistema de Save / Persistencia com JSON
- [x] **Criar SaveManager.cs** 
- [x] **Otimizacao no GameStateData**

### Tarefa 5: Sistemas de Gameplay Especificos do Cap 1 (QTE e Investigacao)
- [ ] **Criar Mecanica de Ataque de Panico (QTE):** 
  - Script PanicAttackSystem.cs que exibe UI de pressionar botoes.
  - Maximo de 3 falhas toleradas. Na quarta falha, X desmaia e encerra o dia.
- [ ] **Criar Mecanica de Investigacao (Juntar as Pontas):**
  - UI que permite selecionar duas pistas e combina-las.

### Tarefa 6: Escalabilidade do DialogueSystem e Cutscenes
- [x] **Desacoplar o DialogueSystemGlobal:** Refatoracao completa para chamadas genericas.
- [x] **Criar CutsceneManager.cs Global:** Finalizado (com controle de trava de player).
- [x] **Correcao de Bugs Criticos:** Corrigida leitura de condicoes de escolhas e item null-checks.

### Tarefa 7: Sistema de Reputacao Global
- [ ] **Sistema de Reputacao:** Adicionar suporte numerico ao GlobalStateManager para computar afinidade com animais e NPCs (ex: +10 Tobias).

### Tarefa 8 (Escopo Futuro): Sistemas do Caminho do Acolhimento
*Nota: A serem desenvolvidos apenas APOS a conclusao do Capitulo 1.*
- [ ] **Sistema de Seguranca:** Avaliar risco de atropelamento, exposicao, locais perigosos, etc.
- [ ] **Sistema de Bem-Estar:** Avaliar alimentacao, agua limpa, descanso.

---

## Ordem de Execucao Atualizada

A base narrativa (JSONs, Cutscenes, Saves) ja foi finalizada. Nosso proximo passo para amanha sera:
1. Iniciar o **Greyboxing do Prologo (Tarefa 3)** com cenas separadas.
2. Construir o **Sistema de Flores (Tarefa 2)**.
3. Desenvolver o **QTE de Panico (Tarefa 5)**.
