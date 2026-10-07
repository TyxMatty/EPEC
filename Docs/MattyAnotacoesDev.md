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
