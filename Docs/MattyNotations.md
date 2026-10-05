# EPEC: Entre Patas e Caminhos - Especificação de Design e Sistema

Aqui é o Mat. Li com muito carinho o roteiro do EPEC(que eu ajudei a escrever kkkk):

Baseado no Prólogo e no Capítulo 1, preparei esta especificação técnica para guiar nosso desenvolvimento, respeitando nossas regra de ouro: **desenvolvimento claro, e comentado** para evitar *spaghetti code* e manter a serialização limpa no Unity.

---

## 1. Visão Geral do Sistema Narrativo e Estado

O jogo possui um alto grau de ramificação baseado em decisões (Rotas B, N e R) e no estado emocional do protagonista. O mundo reflete essas escolhas visualmente (jardim de flores, cores, música).

### A Regra das Listas (Anti-Spaghetti)
Em vez de usarmos um `Dictionary<string, bool>` (que é péssimo para visualizar no Inspector do Unity e difícil de debugar), usaremos **Listas de ScriptableObjects** ou **Classes Serializáveis** gerenciadas por um `GlobalStateManager`.

**Modelo Proposto:**
```csharp
[System.Serializable]
public class GameFlag
{
    public string flagName; // Ex: "acordouPrimeiroDia", "aceitaAjudarTobias"
    public bool isTrue;
}

[CreateAssetMenu(fileName = "GameStateData", menuName = "Scriptable Objects/GameState")]
public class GameStateData : ScriptableObject
{
    [Header("Flags Globais")]
    public List<GameFlag> flags = new List<GameFlag>();

    // Métodos utilitários simples
    public void SetFlag(string name, bool value) { ... }
    public bool GetFlag(string name) { ... }
}
```

## 2. Tradução do Roteiro para Sistemas (Prólogo & Cap 1)

### 2.1. Variáveis Globais (Bools do Roteiro)
O `GameStateData` (a nossa lista) precisará armazenar as seguintes variáveis listadas no documento:
- **Prólogo:** `acordouPrimeiroDia`, `respondeuChamada`, `respondeuMensagemMaria`, `seAlimentou`, `falouComGi`.
- **Rotas e Flores:** `florSuperPositivaDia1`, `florPositivaDia1`, `florNeutraPositivaDia1`, `florNeutraNegativaDia1`, `florNegativaDia1`, `florSuperNegativaDia1`, `RotaB`, `RotaN`, `RotaR`, `abortedRotaR`.
- **Tobias:** `aceitaAjudarTobias`, `fezCarinhoTobias`, `auxiliouTobias`, `agressorEscapa`, `protagonistaNumeroVeterinaria`, `tobiasInterage`, `tobiasInterageMaisOuMenos`, `tobiasOlhoMachucado`, `tobiasCego`.

### 2.2. Mecânicas Principais a Desenvolver
1. **Sistema de Flores (Reflexo do Mundo):** 
   - Um script no cenário (`FlowerManager`) que lê o `GameStateData` no `Start()` e ajusta os sprites/cores das flores da casa do X.
2. **Sistema de QTE (Ataque de Pânico):**
   - Mini-game rítmico ou de reação rápida (pressionar botões). Máximo de 3 falhas antes do desmaio.
3. **Mecânica de Investigação:**
   - O mini-game "juntar as pontas" (dedução). Uma UI onde o jogador combina evidências (ex: "barulho de moto" + "latido do Tobias") para gerar uma conclusão.
4. **Sistema de Reputação / Afinidade:**
   - Além das flags, precisaremos de variáveis numéricas inteiras. Podemos ter uma lista de `ReputationData` para acompanhar a pontuação (ex: `+10 Reputação com Tobias`).

---

## 3. Minhas Ideias e Sugestões (Visão de Desenvolvimento)

- **Feedback Visual (Filtros e Áudio):** Como a Rota R é agressiva e tem blur, podemos criar um `PostProcessingManager` que altera os perfis de Pós-Processamento (Blur, Color Grading) e o Pitch/Volume da música dependendo das flags `RotaR` e da Reputação.
- **Isolamento de Cenas:** Em vez de fazer o jogo todo numa cena gigante, vamos separar os ambientes (Quarto de X, Jardim, Abrigo, Rua, RU, Praça) e carregar os cenários de forma aditiva.
- **Data-Driven Dialogues:** Expandir o nosso `ChoiceSystemGlobal` e `DialogueSystemGlobal` para lerem de arquivos ScriptableObjects ou JSON, assim não precisamos preencher falas em cada GameObject do cenário manualmente, facilitando correções ortográficas e tradução no futuro. -> to fazeno


