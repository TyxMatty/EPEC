# Documentação de Scripts - Projeto EPEC

Este documento detalha a arquitetura e o funcionamento dos scripts principais do projeto até o momento. Os sistemas foram projetados para serem modulares, separando a lógica de movimentação, interação e interface de usuário.

## Visão Geral da Arquitetura

O fluxo principal do jogo atualmente baseia-se na exploração top-down 2D. O jogador movimenta-se pelo cenário e utiliza um sistema de *Raycast* invisível para detectar objetos interativos. Ao interagir, o controle do jogador é bloqueado e as informações do objeto (como falas) são enviadas para um gerenciador global de UI, que processa a exibição do diálogo e devolve o controle ao jogador quando finalizado.

---

## Sistemas Principais

### 1. Movimentação e Animação

* **`MovementScript.cs`**: Responsável pela física de movimentação do jogador utilizando `Rigidbody2D` e o novo Input System do Unity. Calcula a direção baseada nas teclas WASD/Setas e aplica multiplicadores de velocidade quando a tecla Shift é pressionada. Possui a variável pública `podeMover`, que permite a outros sistemas (como diálogos) paralisar o jogador definindo a velocidade como zero.


* **`SpriteChange.cs` (`ScriptChange`)**: Gerencia as animações 2D sem depender do Animator nativo do Unity, trocando os sprites diretamente no `SpriteRenderer`. Ele lê a magnitude e direção da velocidade (`linearVelocity`) no `Rigidbody2D` para determinar se o jogador está parado, andando ou correndo, e atualiza os *frames* com base em um temporizador interno (`transitionSpeed`).



### 2. Sistemas de Câmera

O projeto possui duas abordagens de câmera disponíveis para uso:

* **`CameraFollow.cs`**: Implementa uma câmera dinâmica que segue o alvo (jogador) de forma suave utilizando `Vector3.Lerp`. Executado em `LateUpdate` para evitar travamentos visuais durante a movimentação da física.


* **`SceneFollow.cs`**: Implementa uma câmera de transição por "salas" (estilo clássico de jogos 2D). O script calcula as bordas da sala atual multiplicando o `orthographicSize` e o `aspect` da câmera e move a visão de forma fixa e suave para o centro da nova sala quando o jogador cruza a fronteira.



### 3. Interação e Diálogos (Core Narrativo)

Este é o fluxo integrado de comunicação entre o mundo e a interface:

* **`Interactable.cs`**: Um componente de dados anexado aos objetos físicos do cenário ou NPCs. Contém um array de `strings` (`falas`) e variáveis booleanas categóricas (como `isNPC`, `isPickableObject`) para organizar regras futuras.


* **`InteractionSystem.cs`**: Fica no jogador e lança constantemente um `Raycast2D` na direção do último movimento registrado. Se o raio detectar um objeto na camada `interactableLayer`, exibe um alerta visual na tela (`interactionPrompt`). Ao pressionar Enter ou 'E', o script lê o componente `Interactable` do alvo, altera o `podeMover` do jogador para falso, e injeta as falas no sistema de diálogo.


* **`DialogueSystemGlobal.cs`**: Gerenciador de UI que recebe um array de textos e executa o efeito de digitação caractere por caractere (através da corrotina `DigitarLinha`). Permite ao jogador autocompletar a frase atual ou avançar para a próxima usando a tecla Enter. Ao finalizar todas as falas, executa um *fade out* visual via `CanvasGroup` e reativa o `podeMover` do `MovementScript`.


### 4. Sistemas em Desenvolvimento

* **`ChoiceSystemGlobal.cs`**: Estrutura base (protótipo) criada para gerenciar as futuras ramificações narrativas e escolhas múltiplas exibidas na tela.


---

##  Como Configurar um Novo Objeto Interativo

Para a equipe de Level Design e Integração, siga estes passos para criar um objeto que o jogador possa ler/falar:

1. Crie o objeto na cena (ex: Placa, NPC) e adicione um **Collider 2D** (obrigatório para o Raycast detectá-lo).
2. Altere a **Layer** do objeto para a layer interativa configurada no projeto (ex: `Interagivel`).
3. Anexe o script `Interactable.cs` ao objeto.
4. No componente `Interactable` pelo Inspector, adicione as frases desejadas no array `Falas`.

### 5. Sistema de Inventário

* **InventoryData.cs**: Um ScriptableObject que funciona como banco de dados persistente dos itens do jogador. Contém uma lista itens inicializada via OnEnable para garantir a segurança da serializazação do Unity.
* **ItemData.cs**: Outro ScriptableObject que define o molde para itens coletáveis (Nome, Descrição e ícone).
* **Integração**: O InteractionSystem.cs foi atualizado para verificar se o objeto � um isPickableObject. Se for, o item é adicionado ao InventoryData do jogador de forma segura, com verificações de nulidade para prevenir NullReferenceException antes de possivelmente desaparecer (Destroy).
