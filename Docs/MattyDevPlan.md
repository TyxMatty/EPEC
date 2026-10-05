# EPEC: Plano de Desenvolvimento e Divisão de Tarefas

Matty aqui, eis o plano que montei baseado no que já temos pronto (`MovementScript`, `InteractionSystem`, `DialogueSystemGlobal`, `InventoryData`, etc) e no que precisamos para dar vida ao Roteiro do Prólogo e Capítulo 1.

O objetivo deste plano é garantir progresso constante, sem criar acoplamentos perigosos (*spaghetti code*). Vamos focar em módulos independentes que se comunicam através dos nossos **ScriptableObjects** (Listas).

---

## O que já temos (Nossa Base Sólida):
✅ Movimentação e Animação (`MovementScript`, `SpriteChange`)
✅ Sistema de Câmera (`CameraFollow`, `SceneFollow`)
✅ Interação Básica e Inventário (`InteractionSystem`, `Interactable`, `InventoryData`)
✅ Diálogo Linear e Protótipo de Escolhas (`DialogueSystemGlobal`, `ChoiceSystemGlobal`)

---

## Próximos Passos (Tarefas a Executar)

### Tarefa 1: O Coração Narrativo (GameState Baseado em Listas)
O `ChoiceSystemGlobal` atual é um protótipo. Precisamos da infraestrutura definitiva para salvar as escolhas.
- [ ] **Criar `GameStateData.cs`:** Um ScriptableObject que conterá uma `List<GameFlag>` (para bools como `acordouPrimeiroDia`) e uma `List<GameReputation>` (para pontuação como `+10 Tobias`).
- [ ] **Atualizar `ChoiceSystemGlobal.cs`:** Refatorar o sistema de escolhas para que, ao clicar em uma opção, ele busque a flag correspondente na lista do `GameStateData` e mude seu valor.

### Tarefa 2: Reflexos Visuais do Mundo (O Sistema de Flores e Rota R)
As escolhas afetam o ambiente. Precisamos de sistemas que "escutem" as variáveis.
- [ ] **Criar `FlowerStateListener.cs`:** Um script que é colocado nas flores da casa do X. No `Start()`, ele checa a lista do `GameStateData` e muda seu próprio sprite/cor (Refletindo `florNeutraPositivaDia1`, etc).
- [ ] **Criar `RouteVisualManager.cs`:** Um gerenciador que altera o Post-Processing (blur, cores dessaturadas) e a trilha sonora quando a flag `RotaR` é verdadeira.

### Tarefa 3: Sistemas de Gameplay Específicos do Cap 1
O Capítulo 1 introduz novas mecânicas além de andar e falar.
- [ ] **Criar Mecânica de Ataque de Pânico (QTE):** 
  - Script `PanicAttackSystem.cs` que exibe UI de pressionar botões em tempo limite.
  - Se falhar 3 vezes, dispara um evento de "Desmaio" e avança o dia.
- [ ] **Criar Mecânica de Investigação (Juntar as Pontas):**
  - Script `InvestigationMiniGame.cs`.
  - UI que permite selecionar duas pistas do Inventário/Mente e combiná-las.

### Tarefa 4: Montagem do Prólogo na Cena
Com os sistemas prontos, começaremos a Level Design.
- [ ] Configurar os GameObjects do Quarto do X (Cama, Celular, Espelho) com o `Interactable.cs`.
- [ ] Preencher as falas e configurar o `ChoiceSystemGlobal` para a "Decisão 1: Acordar ou Dormir".
- [ ] Configurar o Jardim com o script `FlowerStateListener`.

---

## Ordem de Execução Sugerida

Sugiro começarmos pela **Tarefa 1**. Assim que tivermos as Listas de estado funcionando, o sistema de Escolhas vai ficar muito poderoso e seguro de se expandir. O que acham? Favor comentem aqui em baixo - Matty
