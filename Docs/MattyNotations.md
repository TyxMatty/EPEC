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

### 2.2. Mecanicas e Modulos
1. **Sistema de Flores (Concluido):**
   - Implementado via `FlowerStateListener.cs`. Escuta flags no `GlobalStateManager` e troca o sprite para refletir a saude do jardim.
2. **Sistema de Reputacao / Afinidade (Concluido):**
   - Implementado via `ReputationSystem.cs` e integrado ao `GlobalStateManager` e `SaveManager.cs`. Permite acompanhar pontuacoes de relacionamento (+/- pontos com animais e NPCs).
3. **Data-Driven Dialogues e Escolhas (Concluido):**
   - Dialogos gerenciados por arquivos JSON em `DialogueData.cs` e processados por `DialogueSystemGlobal.cs` com typewriter suave.
   - Suporte a condicoes por inventario, falas alternativas e alteracao automatica de flags.
4. **Sistema de Cutscenes Standalone (Concluido):**
   - `CutsceneManager.cs` permite orquestrar sequencias narrativas com bloqueio de player e corrotinas sincronizadas.
5. **Modernizacao de Input e Rebinding (Concluido):**
   - `MovementScript.cs` e `InteractionSystem.cs` migrados para `InputActionReference`.
   - `InputRebindManager.cs` adicionado para remapeamento dinamico em tempo de execucao.
6. **Sistema de Pos-Processamento (Concluido):**
   - `PostProcessingManager.cs` criado para controlar os filtros da Rota R e distorcoes emocionais.
7. **Sistema de QTE (Ataque de Panico) (Pendente para amanha):**
   - Mini-game ritmico/reacao com janela de tempo. Maximo de 3 falhas antes do desmaio.
8. **Mecanica de Investigacao (Pendente):**
   - Interface de "juntar as pontas" para cruzar evidencias e gerar conclusoes.

---

## 3. Direcao para as Proximas Sessoes

1. **Greyboxing e Multi-Cenas do Prologo:**
   - Criar e dividir as cenas: `Prologo_Quarto`, `Prologo_Jardim`, `Prologo_Rua`.
   - Utilizar colisores cinzas e hitboxes para validar a navegacao antes da arte final.
2. **Implementacao do QTE de Ataque de Panico:**
   - Montar o prototipo da UI de feedback cardiaco/respiratorio e leitura de input com tolerancia de falhas.
3. **Transicao de Cenas Suave:**
   - Desenvolver o gerenciador de carregamento assincrono de cenas aditivas (`SceneTransitionManager`).
