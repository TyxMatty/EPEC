# Manual do Desenvolvedor - Projeto EPEC

Bem-vindo ao manual do projeto EPEC! Este documento e o guia definitivo da arquitetura do jogo. Ele explica como cada sistema funciona, onde cada script deve ser anexado no Unity e como conectar as referencias no Inspector para garantir estabilidade e extensibilidade sem spaghetti code.

---

## 1. Estrutura de Pastas de Scripts (`Assets/Scripts/`)

Para manter o projeto limpo e desacoplado, os scripts estao organizados nos seguintes modulos:

- `Assets/Scripts/`: Gerenciadores globais e regras narrativas (`GlobalStateManager.cs`, `Condicao.cs`, `Efeito.cs`, `Regra.cs`, `ReputationSystem.cs`, `CutsceneManager.cs`, `PostProcessingManager.cs`, `FlowerStateListener.cs`, `WakeUpPrototype.cs`).
- `Assets/Scripts/SistemasBase/`: Movimentacao, animacao e tela de nome (`MovementScript.cs`, `MovimentoNPC.cs`, `AnimacaoPorMovimento.cs`, `SpriteChange.cs`, `NomeDoJogadorUI.cs`).
- `Assets/Scripts/SistemasBase/Cenas/`: Troca de cena e fim do dia (`SceneTransitionManager.cs`, `PontoDeSpawn.cs`, `PortaDeCena.cs`, `FimDoDia.cs`).
- `Assets/Scripts/Editor/`: Ferramentas do Editor (`GeradorAnimatorBase.cs`, menu `EPEC > Criar Animator Base`).
- `Assets/Scripts/SistemasBase/Input/`: Rebind e gerenciamento de entradas (`InputRebindManager.cs`).
- `Assets/Scripts/SistemasBase/Escolhas/`: Persistencia de dados em disco (`SaveManager.cs`).
- `Assets/Scripts/Interactions/`: Interacao fisica, inventario e escolhas (`InteractionSystem.cs`, `Interactable.cs`, `ItemPickup.cs`, `ChoiceSystemGlobal.cs`, `InventoryData.cs`, `ItemData.cs`, `IInteractAction.cs`).
- `Assets/Scripts/Interactions/Dialogos/`: Dialogos data-driven e maquina de escrever (`DialogueSystemGlobal.cs`, `DialogueTrigger.cs`, `DialogueData.cs`, `CharacterData.cs`).
- `Assets/Scripts/Cameras/`: Modos de camera dinamica e por salas (`CameraFollow.cs`, `SceneFollow.cs`).

Dados carregados por nome ficam em `Assets/Resources/`:
- `Resources/Itens/`: todos os `ItemData` (o `SaveManager` restaura a mochila por aqui).
- `Resources/Dialogos/`: todos os JSONs de dialogo (o `proximoDialogo` acha os arquivos por aqui).
- `Resources/Personagens/`: um `CharacterData` por personagem (nome e retratos da caixa de dialogo).

Regras ligadas por campo no Inspector ficam em `Assets/Dados/Regras/` (ex: `flor_dia1.json`, usado pelo `FimDoDia`).

---

## 2. O Jogador (Player)

O GameObject do jogador na cena e o centro das acoes fisicas e de input.

### Scripts para Anexar no Player:

1. **`MovementScript`** (em `SistemasBase`)
   - **O que faz:** Controla a fisica de movimentacao 2D utilizando `Rigidbody2D` e o Unity Input System moderno (`InputActionReference`).
   - **O que exige:** Componentes `Rigidbody2D` (com Gravity Scale = 0 e Collision Detection = Continuous) e `BoxCollider2D`.
   - **Campos no Inspector:**
     - `baseSpeed`: Velocidade base de caminhada.
     - `shiftMultiplier`: Multiplicador de velocidade ao correr.
     - `moveAction`: Referencia para o `InputActionReference` de movimento (Vector2 / WASD / setas).
     - `sprintAction`: Referencia para o `InputActionReference` de corrida (Shift).
   - **Controle Externo:** A variavel publica `podeMover` (bool) e alterada por dialogos e cutscenes para travar o jogador instantaneamente. Com ela em `false` o `InteractionSystem` tambem nao interage.

2. **`SpriteChange`** (em `SistemasBase`)
   - **O que faz:** Troca o sprite diretamente no `SpriteRenderer` com base na velocidade do `Rigidbody2D`, sem Animator. Vale ate os sprites finais chegarem; depois sai para o `AnimacaoPorMovimento` (secao 9).
   - **O que exige:** Componente `SpriteRenderer`.
   - **Campos no Inspector:**
     - Um sprite por direcao para andar, correr e a transicao do inicio do movimento; 4 frames de idle ciclados em ordem.
     - `transitionSpeed`: Duracao da transicao e intervalo entre frames do idle.

3. **`InteractionSystem`** (em `Interactions`)
   - **O que faz:** Dispara um `Raycast2D` invisivel na direcao em que o jogador esta virado para detectar objetos interagiveis.
   - **Campos no Inspector:**
     - `interactableLayer`: LayerMask configurada para detectar objetos interativos (ex: Layer `Interagivel`).
     - `interactionRange`: Distancia maxima de alcance da interacao.
     - `interactionPrompt`: GameObject ou icone de UI exibido quando o jogador esta na frente de um objeto interativo.
     - `interactAction`: Referencia para o `InputActionReference` de interacao (Enter no `PlayerControls`).

---

## 3. Os Gerenciadores Globais (Game Managers)

Os managers coordenam o estado do jogo e nao possuem corpo fisico no mundo. Recomenda-se criar um GameObject vazio chamado `GameManager` na cena inicial e anexar os gerenciadores necessarios:

1. **`GlobalStateManager`** (ScriptableObject)
   - **O que faz:** A memoria central do jogo. Guarda as flags booleanas (`List<VariavelNarrativa>`), as reputacoes (`List<Reputacao>`), a rota atual (`rota` = "B", "N" ou "R"), o medidor `instabilidade` e o `nomeJogador`.
   - **Caracteristicas:**
     - Listas visiveis no Inspector e serializaveis em JSON (dicionarios so como cache interno de busca).
     - `RotaB`, `RotaN` e `RotaR` funcionam como flags, mas sao uma rota so: ligar uma desliga as outras.
   - **Metodos Principais:** `SetVariavel/GetVariavel`, `AddReputacao/GetReputacao`, `DefinirRota`, `MudarRota(passos)` (B -> N -> R), `AddInstabilidade`, `Resetar()` (Novo Jogo).

2. **`ReputationSystem`** (MonoBehaviour Singleton)
   - **O que faz:** Mostra o popup de afinidade e soma pontos no `GlobalStateManager`. Sem ele na cena, os efeitos de reputacao continuam funcionando, so sem popup.
   - **Campos no Inspector:**
     - `stateManager`: Referencia ao asset `GlobalStateManager`.
   - **Metodos Principais:**
     - `ReputationSystem.Instance.AdicionarReputacao(nome, valor)`

3. **`SaveManager`** (em `SistemasBase/Escolhas`)
   - **O que faz:** Serializa o estado completo do jogo (`GameSaveData`) em um arquivo JSON em `Application.persistentDataPath/savegame.json`. Sem save, chama `Resetar()` e `Limpar()` (Novo Jogo).
   - **O que e salvo:** flags, reputacoes, rota, instabilidade, nome do jogador e os IDs dos itens da mochila.
   - **Campos no Inspector:**
     - `stateManager`: Referencia ao asset `GlobalStateManager`.
     - `inventory`: Referencia ao asset `InventoryData`.

4. **`ChoiceSystemGlobal`** (em `Interactions`)
   - **O que faz:** Exibe a UI de escolhas com temporizador opcional e devolve a opcao escolhida para o dialogo.
   - **Campos no Inspector:**
     - `choiceUI`: GameObject do painel da Canvas de escolhas.
     - `choiceTexts`: Array de `TextMeshProUGUI` das opcoes (opcoes alem desse numero sao cortadas com aviso).
     - `stateManager`: Referencia ao asset `GlobalStateManager`.
     - `inventarioGlobal`: Referencia ao asset `InventoryData`.

5. **`DialogueSystemGlobal`** (em `Interactions/Dialogos`)
   - **O que faz:** Controla a exibicao de falas na tela com efeito retro de maquina de escrever (`dialogueText.text += letter`), mostra quem fala e encadeia dialogos.
   - **Campos no Inspector:**
     - `dialogueText`: Componente `TextMeshProUGUI` onde as frases sao renderizadas.
     - `nomeText` (opcional): `TextMeshProUGUI` com o nome de quem fala.
     - `retratoImage` (opcional): `Image` com o retrato do falante.
     - `canvasGroup`: Componente `CanvasGroup` do painel de dialogo para o fade de saida.
     - `choiceSystem`: Referencia ao `ChoiceSystemGlobal`. E por ele que o dialogo le o `GlobalStateManager` e o `InventoryData` (condicoes e efeitos).
     - `submitAction`: Acao que avanca as falas (Enter).

6. **`CutsceneManager`**
   - **O que faz:** Singleton que trava/libera o jogador e fornece a corrotina de espera `TocarDialogoEEsperar`.
   - **Seguranca:** Se o painel de dialogo for desligado no meio, a corrotina e liberada (nao ha timeout).

7. **`NomeDoJogadorUI`** (em `SistemasBase`)
   - **O que faz:** Painel com `TMP_InputField` onde o jogador escolhe o nome que substitui `{X}` nos dialogos.
   - **Campos no Inspector:** `stateManager`, `painel`, `campoNome` e `tamanhoMaximo`. Ligue o botao "Confirmar" em `Confirmar()`; Enter no campo tambem confirma.
   - **Uso:** `yield return telaDeNome.PedirNome();` numa cutscene. O `WakeUpPrototype` ja faz isso quando `FaltaNome`.

---

## 4. Dados em Scriptable Objects (`Assets`)

Estes assets sao salvos diretamente nos arquivos do projeto Unity:

- **`GlobalStateManager`**: Asset criado via `Create -> Scriptable Objects -> GlobalStateManager`.
- **`InventoryData`**: Asset criado via `Create -> Scriptable Objects -> Inventory`. Contem a lista `itens` (`List<ItemData>`) e os metodos `AddItem` (alias `Additem`), `RemoveItem`, `ContemItem` e `Limpar`.
- **`ItemData`**: Asset que define um item colecionavel (`itemID`, `itemName`, `itemDescription`, `itemIcon`).
  - **REGRA OBRIGATORIA:** Todos os assets de `ItemData` **DEVEM** ser salvos dentro da pasta `Assets/Resources/Itens/`. O `SaveManager` usa `Resources.LoadAll<ItemData>("Itens")` para restaurar os itens.
- **`CharacterData`**: Asset criado via `Create -> Scriptable Objects -> Personagem`, salvo em `Assets/Resources/Personagens/<Nome>.asset`.
  - `nomeExibido` (vazio = nome do arquivo; `{X}` = nome do jogador), `corDoNome` e a lista `expressoes` (`nome`, `rota`, `sprite`).
  - Uma expressao com `rota` = "R" substitui a padrao na Rota R (o roteiro pede expressoes mais agressivas nessa rota).

---

## 5. Objetos Interagiveis no Cenario

Para criar um objeto que o jogador pode inspecionar, falar ou coletar:

1. Crie o GameObject com `SpriteRenderer` e `BoxCollider2D`.
2. Defina a **Layer** do GameObject para a mesma configurada no `InteractionSystem` do jogador (ex: `Interagivel`).
3. Anexe os componentes conforme o proposito do objeto:

### A - Para Dialogos e Eventos: `DialogueTrigger`
- Anexe o script `DialogueTrigger` ao objeto.
- Arraste o arquivo `TextAsset` (o `.json` em `Assets/Resources/Dialogos/`) para o campo `dialogueJson`.

### B - Para Coleta de Itens: `ItemPickup`
- Anexe o script `ItemPickup` ao objeto.
- Arraste o asset `ItemData` correspondente (salvo em `Resources/Itens/`) para o campo `itemToAdd`.
- Ao interagir, o item e adicionado ao `InventoryData` e o GameObject e desativado (se houver `DialogueTrigger`, depois do dialogo).

### C - Para Cenarios Reativos: `FlowerStateListener`
- Anexe o script `FlowerStateListener` em flores ou elementos do cenario que reagem ao estado narrativo.
- Configure `variavelFlor` (nome da flag no `GlobalStateManager`), `spriteViva` e `spriteMurcha`.
- O script atualiza o sprite no `Start()` e pode ser chamado a qualquer momento via `AtualizarEstadoFlor()`.

---

## 6. Sistema de Cutscenes e Dialogos Standalone

Para criar cutscenes scriptadas (como a do despertar em `WakeUpPrototype.cs`):

1. Crie uma Coroutine no seu script da cutscene (`IEnumerator ExecutarCutscene()`).
2. Trave o jogador: `CutsceneManager.Instance.TravarJogador();`
3. Dispare o dialogo e pause a execucao ate o fim da leitura (inclusive de toda a cadeia de `proximoDialogo`):
   ```csharp
   yield return CutsceneManager.Instance.TocarDialogoEEsperar(meuTextAssetJson);
   ```
4. Execute animacoes, mudancas de camera ou fades.
5. Devolva o movimento ao jogador ao concluir: `CutsceneManager.Instance.LiberarJogador();`

Dialogos de cutscene nao destravam o jogador sozinhos; so o `DialogueTrigger` (interacao do jogador) destrava no fim.

---

## 7. Formato dos Dialogos em JSON

Os arquivos ficam em `Assets/Resources/Dialogos/` com extensao `.json`. O JsonUtility **ignora sem aviso** chaves com nome errado, entao copie os nomes daqui. Os textos podem ter acento; `{X}` vira o nome do jogador.

### 7.1. Formato v2 (use este)

```json
{
    "dialogueID": "cap1_abrigo_apresentacao",
    "linhas": [
        { "personagem": "Maria", "expressao": "feliz", "texto": "Voce veio mesmo! Nao acredito!!" },
        { "personagem": "Gi", "texto": "{X}! A Maria me contou muitas coisas boas sobre voce!",
          "condicoes": [ { "flag": "falouComGi" } ] },
        { "personagem": "Gi", "texto": "E esse o tal {X}?",
          "condicoes": [ { "flag": "falouComGi", "negar": true } ] }
    ],
    "efeitos": [],
    "opcoes": [
        { "texto": "Quem e Tobias?", "proximoDialogo": "cap1_maria_conta_tobias" },
        { "texto": "Ando muito ocupado...",
          "efeitos": [ { "flag": "aceitaAjudarTobias", "negar": true }, { "rota": "R" } ],
          "proximoDialogo": "cap1_maria_entende" }
    ]
}
```

- **`linhas`**: cada fala tem `personagem` (nome do `CharacterData`; vazio = narracao; `X` = protagonista), `expressao` (vazio = `neutra`), `texto` e, opcionalmente, `condicoes` / `condicoesQualquer`. Falas cujas condicoes nao valem sao puladas.
- **`efeitos`**: aplicados quando as falas acabam.
- **`opcoes`**: cada opcao tem `texto`, `condicoes` / `condicoesQualquer` (a opcao so aparece se valerem), `efeitos` e `proximoDialogo` (nome de outro JSON em `Resources/Dialogos`, sem `.json`; vazio = encerra).
- `temTempoLimite` e `tempoLimite` tambem valem para as opcoes v2; no tempo esgotado nenhuma opcao e aplicada.
- Exemplo completo do roteiro: `cap1_abrigo_apresentacao.json` e os 3 dialogos que ele encadeia.

### 7.2. Condicoes e Efeitos

Uma **condicao** checa um alvo; `"negar": true` inverte. `condicoes` exige TODAS; `condicoesQualquer` exige ALGUMA.

| Condicao | Significa |
|---|---|
| `{ "flag": "acordouPrimeiroDia" }` | flag ligada (`RotaB/RotaN/RotaR` tambem valem) |
| `{ "reputacao": "Tobias", "minimo": 20 }` | reputacao >= 20 (`repTobias >= 20` do roteiro) |
| `{ "item": "Chave" }` | item na mochila (`itemName` ou `itemID`) |
| `{ "rota": "R" }` | rota atual |

Um **efeito** muda o estado; os campos preenchidos sao aplicados juntos.

| Efeito | Faz |
|---|---|
| `{ "flag": "fezCarinhoTobias" }` | liga a flag (`"negar": true` desliga) |
| `{ "reputacao": "Tobias", "pontos": 10 }` | soma reputacao (negativo subtrai) |
| `{ "rota": "B" }` | define a rota |
| `{ "passosRota": 1 }` | anda B -> N -> R (1 piora, -1 melhora) |
| `{ "instabilidade": 1 }` | soma no medidor de instabilidade |

### 7.3. Formato antigo (ainda funciona)

```json
{
    "dialogueID": "porta",
    "falas": [ "A porta esta trancada por fora." ],
    "alteraVariavel": false,
    "variavelAlteradaPorDialogo": "",
    "temFrasesAlternativas": true,
    "variavelDaCondicao": "",
    "itemDaCondicao": "Chave",
    "esperaCondicaoFalsa": false,
    "falasAlternativas": [ "X abriu a porta" ],
    "dialogoAlternativoAlteraVariavel": true,
    "variavelAlteradaPorDialogoAlternativo": "portaAberta",
    "terminaEmEscolha": false,
    "variavelParaSalvar": "",
    "textosDasOpcoes": [],
    "valoresDasOpcoes": [],
    "temTempoLimite": false,
    "tempoLimite": 0.0,
    "escolhaNeutra": false
}
```

Quando `linhas` esta preenchido, `falas`/`falasAlternativas` sao ignorados.

---

## 8. Cenas, Nucleo e Fim do Dia

### 8.1. Prefab `Nucleo`
Tudo que vive o jogo inteiro fica dentro de um prefab `Nucleo`, com o `SceneTransitionManager` no objeto RAIZ: Player, Main Camera, Canvas de UI (dialogo, escolhas, tela de nome, tela preta), EventSystem, `CutsceneManager`, `ReputationSystem` e `SaveManager`. Ele passa de cena em cena (DontDestroyOnLoad).

Toda cena leva uma copia do `Nucleo`, entao qualquer cena roda sozinha no Play. Quando se chega por porta, a copia da cena nova se desliga e se apaga antes de acordar.

Montagem (uma vez, na cena atual):
1. Crie um GameObject vazio `Nucleo` na raiz e arraste para dentro Player, Main Camera, Canvas, EventSystem e os managers.
2. No Canvas, crie `TelaPreta`: Image preta esticada na tela inteira + `CanvasGroup` (Alpha 0). Deixe-a por cima do dialogo (ultimo filho do Canvas).
3. Adicione `SceneTransitionManager` no `Nucleo` e ligue `jogador` (Player) e `telaPreta`.
4. Arraste o `Nucleo` para `Assets/Prefabs/` e coloque uma instancia em cada cena.
5. Adicione as cenas em `File > Build Profiles > Scene List`.

**Regra:** objetos da cena nao apontam no Inspector para objetos do `Nucleo` (a copia da cena some quando se chega por porta). Use `CutsceneManager.Instance`, `SceneTransitionManager.Instance` ou `FindAnyObjectByType`. Assets (`GlobalStateManager`, `InventoryData`, JSONs) podem ser ligados normalmente.

### 8.2. Portas e pontos de spawn
- **`PontoDeSpawn`** (`id`): onde o player aparece ao entrar na cena (gizmo azul).
- **`PortaDeCena`** (`cena`, `spawn`): com collider normal na layer de interacao, troca ao apertar Enter; com collider **Is Trigger**, troca ao encostar. Deixe o spawn de volta fora do trigger da porta.
- Por codigo ou UnityEvent: `SceneTransitionManager.Instance.IrPara("Prologo_Jardim", "PortaDaCasa");`
- A troca trava o player durante o fade-out, solta antes de carregar e posiciona player e camera no spawn.

### 8.3. NPCs em cutscene: `MovimentoNPC`
- No NPC: `Rigidbody2D` (Kinematic serve) + `MovimentoNPC` (`velocidade`).
- `yield return maria.MoverPara(marcador.position);` termina ao chegar; se ficar presa por 10 s, teleporta (a cutscene nunca trava).
- `tobias.SeguirAlvo(player, estado.GetVariavel("fezCarinhoTobias") ? 1f : 2.5f);` e `PararDeSeguir()`.

### 8.4. Fim do dia: `FimDoDia`
- **Campos:** `estado`, `mochila` (opcional), `regras` (JSON), `proximaCena`, `spawn`.
- **`EncerrarDia()`** (UnityEvent, cutscene ou QTE): aplica a primeira regra que bate, salva (acha o `SaveManager` do Nucleo sozinho) e troca de cena com fade.
- Fim normal, desmaio do ataque de panico e Rota R profunda: um `FimDoDia` para cada, com `proximaCena` diferente.
- Regras do dia 1: `Assets/Dados/Regras/flor_dia1.json`, as 6 flores do roteiro na mesma ordem. Formato (mesmas Condicoes e Efeitos da secao 7.2):

```json
{
  "limpar": ["florSuperPositivaDia1", "florNegativaDia1"],
  "regras": [
    { "nome": "1 - Colorida, brilhante",
      "condicoes": [ {"flag": "auxiliouTobias"}, {"flag": "abortedRotaR", "negar": true} ],
      "efeitos": [ {"flag": "florSuperPositivaDia1"} ] }
  ]
}
```
- As condicoes leem o estado de antes; as flags de `limpar` sao desligadas so depois que uma regra bate. Nenhuma regra batendo = nada muda e aparece um aviso no Console.

---

## 9. Sprites com Animator

1. Rode uma vez **`EPEC > Criar Animator Base`**. Ele gera `Assets/Animacoes/Base/Humano4Dir.controller` e `Quadrupede4Dir.controller`:
   - estados Parado, Andando e Correndo, cada um uma Blend Tree 2D por `DirX`/`DirY` com 4 clips vazios (Cima, Baixo, Esquerda, Direita);
   - parametro `Velocidade`: anda acima de 0.1 e corre acima de 5.5 (entre o `baseSpeed` 5 e o Sprint 7.5).
2. Importe a sprite sheet com:
   - Sprite Mode Multiple, o mesmo Pixels Per Unit para todos e Filter Mode Point;
   - Compression None e pivot nos pes (Bottom) no Sprite Editor.
3. Crie os clips do personagem na janela Animation (ex: `Maria_Andando_Baixo`) com Loop Time ligado.
4. `Create > Animation > Animator Override Controller`, com Controller = `Humano4Dir` (ou `Quadrupede4Dir`), e troque cada clip base pelo do personagem.
5. No personagem:
   - `Animator` (Controller = o override), `AnimacaoPorMovimento` e `Rigidbody2D`;
   - tire o `SpriteChange`.
6. Acoes avulsas do roteiro (Abracar, Girar, Chutar, Latir):
   - estado novo no controller base com um Trigger do mesmo nome;
   - na cutscene, `GetComponent<Animator>().SetTrigger("Girar");`.
7. Ordem de desenho:
   - no Renderer 2D (`Assets/Settings`), Transparency Sort Mode = Custom Axis e Axis = (0, 1, 0);
   - nos `SpriteRenderer`, Sprite Sort Point = Pivot. Quem esta mais abaixo na tela fica na frente.

---

## 10. Boas Praticas e Convencoes de Codigo

1. **Null Checks em Campos Serializados:** Sempre valide campos `[SerializeField]` com verificacao nula antes de acessar membros para prevenir `NullReferenceException` quando referencias forem esquecidas no Inspector.
2. **Preservacao de Nomes no Inspector:** Ao renomear variaveis serializadas em scripts C#, utilize o atributo `[FormerlySerializedAs("nomeAntigo")]` para evitar que as ligacoes nas cenas (`.unity`) e prefabs sejam perdidas.
3. **Comentarios e Codificacao:** Mantenha todos os comentarios de codigo e mensagens em caracteres ASCII puros para evitar problemas de compatibilidade e corrupcao de codificacao UTF-8 no Windows. Textos de dialogo nos JSONs podem ter acento.
4. **Nomes de flags:** use a grafia oficial do `Docs/MattyNotations.md` (secao 2.1); o codigo diferencia maiusculas.
