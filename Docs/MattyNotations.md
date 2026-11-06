# EPEC: Entre Patas e Caminhos - Especificacao de Design e Sistema

Documento de especificacao tecnica e direcao de design para o desenvolvimento do Prologo e Capitulo 1, focado em manutencao limpa, modularidade e estabilidade.

---

## 1. Visao Geral do Sistema Narrativo e Estado

O jogo possui ramificacoes baseadas em decisoes (Rotas B, N e R) e no estado emocional do protagonista. O ambiente reflete essas escolhas visualmente e sonoramente (flores, paleta de cores, musica e filtros de tela).

### A Regra das Listas (Anti-Spaghetti)
Para evitar as limitacoes de serializacao dos dicionarios no Unity Inspector e garantir que os dados sejam visiveis e editaveis pela equipe de design, o jogo utiliza estritamente listas de classes serializaveis no `GlobalStateManager`:

```csharp
[System.Serializable]
public class GameFlag
{
    public string flagName; // Ex: "acordouPrimeiroDia", "aceitaAjudarTobias"
    public bool isTrue;
}

[System.Serializable]
public class Reputacao
{
    public string nome;     // Ex: "Tobias", "Maria", "Gi"
    public int valor;       // Pontuacao de afinidade
}
```

---

## 2. Status dos Sistemas do Roteiro (Prologo & Cap 1)

### 2.1. Variaveis Globais Mapeadas
Grafia oficial (igual ao roteiro revisado e aos JSONs; o codigo diferencia maiusculas):
- **Prologo:** `acordouPrimeiroDia`, `respondeuChamada`, `respondeuMensagemMaria`, `seAlimentou`, `falouComGi`.
- **Rotas e Flores:** `florSuperPositivaDia1`, `florPositivaDia1`, `florNeutraPositivaDia1`, `florNeutraNegativaDia1`, `florNegativaDia1`, `florSuperNegativaDia1`, `florQuaseMorta` (status de uma decisao negativa maxima), `RotaB`, `RotaN`, `RotaR`, `abortedRotaR`.
  - `RotaB/RotaN/RotaR` sao uma rota so (`GlobalStateManager.rota`): ligar uma desliga as outras.
  - `florNegativaDia1` e ligada no meio do Cap 1 (Decisao 1.2 / recusar o Tobias) e lida no fim do dia: se ja estava ligada, a flor do dia vira `florSuperNegativaDia1`.
- **Tobias:** `aceitaAjudarTobias`, `mariaVindo`, `foiAjudarTobias`, `fezCarinhoTobias`, `auxiliouTobias`, `agressorEscapa`, `protagonistaNumeroVeterinaria`, `tobiasInterage`, `tobiasInterageMaisOuMenos`, `tobiasOlhoMachucado`, `tobiasCego`.
- **Reputacao:** alvos `Tobias` e `Maria` (o roteiro escreve `repTobias` / `repMaria`).
- **Investigacao (nomes propostos, o roteiro nao nomeia):** pistas `pistaMotoRapida`, `pistaTobiasCorre`, `pistaMotoqueiroXinga` (a do carinho usa `fezCarinhoTobias`) e a conclusao `descobriuGatilhoTobias`.

### 2.2. Mecanicas e Modulos
1. **Sistema de Flores (Concluido no codigo, falta arte):**
   - `FlowerStateListener.cs` escolhe o sprite por uma lista de estados (condicoes -> sprite) e reage a toda mudanca do `GlobalStateManager`.
   - `FimDoDia.cs` calcula a flor do dia pelas 6 regras do roteiro (`Assets/Dados/Regras/flor_dia1.json`).
2. **Sistema de Reputacao / Afinidade (Concluido):**
   - Implementado via `ReputationSystem.cs` e integrado ao `GlobalStateManager` e `SaveManager.cs`. Permite acompanhar pontuacoes de relacionamento (+/- pontos com animais e NPCs).
3. **Data-Driven Dialogues e Escolhas (Concluido):**
   - Dialogos gerenciados por arquivos JSON em `Assets/Resources/Dialogos/` (classes em `DialogueData.cs`) e processados por `DialogueSystemGlobal.cs` com typewriter suave.
   - Formato v2: falante e retrato por fala, condicao por fala (flag, reputacao minima, item, rota; E/OU), opcoes com efeitos e `proximoDialogo`, e `{X}` com o nome do jogador.
4. **Sistema de Cutscenes Standalone (Concluido):**
   - `CutsceneManager.cs` permite orquestrar sequencias narrativas com bloqueio de player e corrotinas sincronizadas.
5. **Modernizacao de Input e Rebinding (Concluido):**
   - `MovementScript.cs` e `InteractionSystem.cs` migrados para `InputActionReference`.
   - `InputRebindManager.cs` adicionado para remapeamento dinamico em tempo de execucao.
6. **Sistema de Pos-Processamento (Pendente):**
   - `PostProcessingManager.cs` existe mas esta vazio; os filtros da Rota R e distorcoes emocionais entram na Fase 4.
7. **Sistema de QTE (Concluido no codigo, falta a UI na cena):**
   - `QTESystem.cs`: direcoes com janela de tempo e limite de falhas; serve para o agressor e para o ataque de panico (mais de 3 falhas = desmaio pelo `FimDoDia`).
8. **Mecanica de Investigacao (Concluido no codigo, falta a UI na cena):**
   - `QuadroInvestigacao.cs` + casos em JSON (`Assets/Dados/Investigacoes/`); caso do Cap 1: `cap1_tobias_motos.json`.

---

## 3. Direcao para as Proximas Sessoes

1. **Greyboxing e Multi-Cenas do Prologo:**
   - Criar e dividir as cenas: `Prologo_Quarto`, `Prologo_Jardim`, `Prologo_Rua`.
   - Utilizar colisores cinzas e hitboxes para validar a navegacao antes da arte final.
2. **Implementacao do QTE de Ataque de Panico:**
   - Montar o prototipo da UI de feedback cardiaco/respiratorio e leitura de input com tolerancia de falhas.
3. **Transicao de Cenas Suave:**
   - Desenvolver o gerenciador de carregamento assincrono de cenas aditivas (`SceneTransitionManager`).
