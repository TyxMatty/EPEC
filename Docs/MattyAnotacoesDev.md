# EPEC: Code Review e Análise de Qualidade

Pedi, após verificação manual, para 4 modelos de agentes fazerm revisões em nossos códigos, eis algumas notas que tiveram:

---

Olá Matty! Como pediu, fiz uma revisão minuciosa de todo o código atual (`MovementScript`, `SpriteChange`, `InteractionSystem`, `ChoiceSystemGlobal`, `DialogueSystemGlobal`, `Interactable`, `Scene/CameraFollow`). 

No geral, a lógica base do jogo está muito bem estruturada! Você separou bem os sistemas visuais (UI) da física (Rigidbody), o que é ótimo. Abaixo estão minhas observações focadas na sua regra principal: **Evitar Spaghetti Code e focar em Listas em vez de Dicionários**.

---

## 1. Arquitetura e o perigo do "God Object" (Interactable.cs)

**Observação:** 
O seu script `Interactable.cs` está crescendo muito. Ele atualmente guarda:
- Textos de Diálogo.
- Lógica de alterar variáveis (`alteraVariavel`).
- Condições complexas (`temFrasesAlternativas`, `itemDaCondicao`).
- Lógica de Escolhas e Timers (`terminaEmEscolha`, `textosDasOpcoes`).
- Lógica de Inventário (`isPickableObject`, `itemToAdd`).
  // estou me responsabilizando disso - Matty
**O Risco (Spaghetti Code):** 
Se continuarmos colocando tudo dentro do `Interactable.cs`, ele vai virar uma "God Class" (Classe Deus) que sabe demais. Toda vez que você for arrumar um bug de inventário, pode quebrar o sistema de diálogo sem querer.

**Sugestão de Refatoração (Para o futuro):**
Em vez de um script gigante, podemos usar **Componentes Múltiplos**. O GameObject teria:
- `Interactable.cs` (Apenas a tag que o Raycast detecta).
- `DialogueTrigger.cs` (Só guarda os textos e manda pro DialogueSystem).
- `ChoiceTrigger.cs` (Só guarda as opções e manda pro ChoiceSystem).
- `ItemPickup.cs` (Só guarda o `ItemData`).
Assim, você monta NPCs e Itens como se fosse blocos de Lego!

---

## 2. O Fim dos Dicionários (ChoiceSystemGlobal.cs)

**Observação:**
O `ChoiceSystemGlobal` tenta salvar escolhas usando `variavelAtualParaSalvar` (string) chamando `SalvarEscolha(...)` em um `GlobalStateManager`. Se o `GlobalStateManager` usar um Dictionary por baixo dos panos, teremos problemas de serialização e visualização no Unity.

**A Solução (Listas):**
Como concordamos no plano, o `GameStateData` (ScriptableObject) terá uma `List<GameFlag>`. 
O `ChoiceSystemGlobal` deve ser atualizado para buscar na Lista, algo como:
```csharp
public void SalvarEscolha(string flagName, bool valor) {
    GameFlag flag = stateManager.flags.Find(f => f.flagName == flagName);
    if (flag != null) flag.isTrue = valor;
}
```
// comentário do matty: isso eu já fiz, podem checar o código tbm

---

## 3. Sistema de Input (MovementScript.cs e outros)

**Observação:**
Vocês estão usando o pacote novo de Input System, mas estão fazendo *Polling* direto do hardware:
`var keyboard = Keyboard.current; if (keyboard.wKey.isPressed) ...`

**Ponto Positivo:** Funciona perfeitamente bem para PC.
**Atenção:** Se vocês quiserem lançar o jogo para Mobile (joystick na tela) ou dar suporte a Controles de Xbox/PlayStation depois, teriam que reescrever todos os ifs. 
**Sugestão:** No futuro, podemos mudar isso para usar `InputActionReference`, onde o Unity mapeia automaticamente o botão de "Andar" para o teclado e controle ao mesmo tempo. Mas **não é prioridade agora**.  -- RNF

---

## 4. Práticas Excelentes Encontradas

- **Câmeras (`CameraFollow` e `SceneFollow`):** Usar o `LateUpdate` para atualizar a câmera após a física do `MovementScript` (que roda no FixedUpdate interno ou Update) é o jeito certinho de evitar stuttering (engasgos visuais). Mandou bem!
- **Null Checks Iniciais:** Você usou `RequireComponent` no `MovementScript` e no `SpriteChange`. Isso é uma excelente prática para evitar que alguém da equipe esqueça de colocar o Rigidbody2D no personagem e o jogo quebre.
- **Isolamento de Estado:** O uso de `podeMover` para travar o player durante os diálogos é limpo e direto.

---

**Resumo da Ópera:** 
Seu código está muito longe de ser um spaghetti code. Com a adoção das Listas de Estado (`GameStateData`) e a eventual quebra do `Interactable` em pequenos blocos (Lego), o projeto vai escalar facilmente para os próximos 4 capítulos sem dor de cabeça!

