# EPEC: Plano de Desenvolvimento e Divisao de Tarefas

Matty aqui, eis o plano que montei baseado no que ja temos pronto (MovementScript, InteractionSystem, DialogueSystemGlobal, InventoryData, etc) e no que precisamos para dar vida ao Roteiro do Prologo e Capitulo 1.

O objetivo deste plano e garantir progresso constante, sem criar acoplamentos perigosos (*spaghetti code*). Vamos focar em modulos independentes que se comunicam atraves dos nossos **ScriptableObjects** (Listas).

---

## O que ja temos (Nossa Base Solida):
- Movimentacao e Animacao (MovementScript, SpriteChange)
- Sistema de Camera (CameraFollow, SceneFollow)
- Interacao Basica e Inventario (InteractionSystem, Interactable, InventoryData)
- Dialogo Linear, Prot�tipo de Escolhas e Condicoes via JSON (DialogueSystemGlobal, ChoiceSystemGlobal, DialogueData)
- Save System (SaveManager, GlobalStateManager otimizado)

---

## Proximos Passos (Tarefas a Executar)

### Tarefa 1: O Coração Narrativo (GameState Baseado em Listas)
O `ChoiceSystemGlobal` atual é um protótipo. Precisamos da infraestrutura definitiva para salvar as escolhas.
- [x] **Criar `GameStateData.cs`:** Um ScriptableObject que conterá uma `List<GameFlag>` (para bools como `acordouPrimeiroDia`) e uma `List<GameReputation>` (para pontuação como `+10 Tobias`). -> ja fiz - matty
- [x] **Atualizar `ChoiceSystemGlobal.cs`:** Refatorar o sistema de escolhas para que, ao clicar em uma opção, ele busque a flag correspondente na lista do `GameStateData` e mude seu valor. -> ja fiz tbm

### Tarefa 2: Reflexos Visuais do Mundo (O Sistema de Flores e Rota R)
As escolhas afetam o ambiente. Precisamos de sistemas que "escutem" as variaveis.
- [ ] **Criar FlowerStateListener.cs:** Um script que e colocado nas flores da casa do X. No Start(), ele checa a lista do GameStateData e muda seu proprio sprite/cor (Refletindo lorNeutraPositivaDia1, etc).
- [ ] **Criar RouteVisualManager.cs:** Um gerenciador que altera o Post-Processing (blur, cores dessaturadas) e a trilha sonora quando a flag RotaR e verdadeira.

### Tarefa 3: Montagem do Prologo na Cena
Com os sistemas prontos, comecaremos a Level Design.
- [ ] Configurar os GameObjects do Quarto do X (Cama, Celular, Espelho) com o Interactable.cs.
- [ ] Preencher as falas e configurar o ChoiceSystemGlobal para a "Decisao 1: Acordar ou Dormir".
- [ ] Configurar o Jardim com o script FlowerStateListener.

### Tarefa 4: Sistema de Save / Persistencia com JSON
- [x] **Criar SaveManager.cs** 
- [x] **Otimizacao no GameStateData**

### Tarefa 5: Sistemas de Gameplay Especificos do Cap 1
O Capitulo 1 introduz novas mecanicas alem de andar e falar.
- [ ] **Criar Mecanica de Ataque de Panico (QTE):** 
  - Script PanicAttackSystem.cs que exibe UI de pressionar botoes em tempo limite.
  - Se falhar 3 vezes, dispara um evento de "Desmaio" e avanca o dia.
- [ ] **Criar Mecanica de Investigacao (Juntar as Pontas):**
  - Script InvestigationMiniGame.cs.
  - UI que permite selecionar duas pistas do Inventario/Mente e combina-las.

### Tarefa 6: Escalabilidade do DialogueSystem e Cutscenes
- [ ] **Desacoplar o DialogueSystemGlobal:** Permitir que IniciarDialogo seja invocado via eventos (UnityEvents) ou por qualquer outro script no jogo, e nao apenas por um DialogueTrigger atrelado a um GameObject interagivel.
- [ ] **Criar CutsceneManager.cs Global:** Um script para gerenciar cutscenes. Deve pausar o jogador, disparar animacoes representativas de cada acao, invocar dialogos especificos (ex: dialogo Y) que levam a escolhas X, Y, Z.

---

## Ordem de Execucao Sugerida

Sugiro comecarmos pela **Tarefa 6** agora que o core esta feito, estruturando o Cutscene Manager e desacoplando os dialogos. Em seguida, partimos para a **Tarefa 3** (Montagem do Prologo). -- matty
