using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VariavelNarrativa
{
    // Nome identificador unico da variavel (ex: falou_com_gi, porta_aberta)
    public string nome;
    // Estado booleano atual da variavel narrativa
    public bool valor;
}

// Origem: Assets/Scripts/GlobalStateManager.cs
// Estrutura serializavel para representar pontos de afeto/reputacao com NPCs e animais
[System.Serializable]
public class Reputacao
{
    // Nome do NPC ou animal (ex: Tobias, Laka, Gi)
    public string nome;
    // Quantidade inteira de pontos acumulados
    public int pontos;
}

// ScriptableObject central com todas as flags narrativas e reputacoes do jogo
// Por ser asset, o que for gravado em Play Mode fica no MemoriaDoJogo.asset; o SaveManager chama Resetar() quando nao ha save
[CreateAssetMenu(fileName = "GlobalStateManager", menuName = "Scriptable Objects/GlobalStateManager")]
public class GlobalStateManager : ScriptableObject
{
    [Header("Memoria Persistente do Jogo")]
    // Lista visivel no Inspector contendo todas as variaveis booleanas narrativas
    public List<VariavelNarrativa> variaveisGlobais = new List<VariavelNarrativa>(); 
    
    [Header("Afinidade com NPCs e Animais")]
    // Lista visivel no Inspector contendo os pontos de reputacao de cada NPC/animal
    public List<Reputacao> reputacoes = new List<Reputacao>();

    [Header("Rota, Estado Emocional e Jogador")]
    // Rota atual: "B", "N" ou "R" (as flags RotaB/RotaN/RotaR leem e gravam aqui, uma rota por vez)
    public string rota = "B";
    // Medidor de instabilidade emocional de X (0 = estavel); ataques de panico e Rota R sobem
    public int instabilidade = 0;
    // Nome escolhido pelo jogador; vazio = ainda nao escolheu (os dialogos mostram "X")
    public string nomeJogador = "";

    private const string OrdemRotas = "BNR";

    // Cache de busca rapida espelhando as listas acima (montado em Inicializar)
    // REVIEW: editar uma flag no Inspector durante o Play nao atualiza o dicionario e GetVariavel devolve o valor antigo.
    // Com ~30 flags um List.Find ja e rapido; apagar os dicts elimina essa dessincronia.
    private Dictionary<string, bool> dictVariaveis = new Dictionary<string, bool>();
    private Dictionary<string, int> dictReputacoes = new Dictionary<string, int>();

    // Disparado a cada mudanca de flag, reputacao, rota ou instabilidade e depois de Resetar/carregar o save
    // (flores e outros visuais se inscrevem no OnEnable e saem no OnDisable)
    public event System.Action Mudou;

    public void AvisarMudanca()
    {
        Mudou?.Invoke();
    }

    private void OnEnable()
    {
        Inicializar();
    }

    // Zera flags, reputacoes, rota, instabilidade e nome (Novo Jogo)
    public void Resetar()
    {
        variaveisGlobais = new List<VariavelNarrativa>();
        reputacoes = new List<Reputacao>();
        rota = "B";
        instabilidade = 0;
        nomeJogador = "";
        Inicializar();
        AvisarMudanca();
    }

    // Nome que substitui {X} nos dialogos
    public string NomeExibido => string.IsNullOrEmpty(nomeJogador) ? "X" : nomeJogador;

    // Define a rota ("B", "N" ou "R"); qualquer outro valor e ignorado com aviso
    public void DefinirRota(string novaRota)
    {
        if (!string.IsNullOrEmpty(novaRota) && novaRota.Length == 1 && OrdemRotas.Contains(novaRota))
        {
            rota = novaRota;
            AvisarMudanca();
        }
        else Debug.LogWarning($"[GlobalStateManager] Rota invalida '{novaRota}'. Use B, N ou R.");
    }

    // Anda na ordem B -> N -> R: passos > 0 piora, < 0 melhora, sem passar dos extremos
    public void MudarRota(int passos)
    {
        int atual = Mathf.Max(0, OrdemRotas.IndexOf(rota));
        rota = OrdemRotas[Mathf.Clamp(atual + passos, 0, OrdemRotas.Length - 1)].ToString();
        AvisarMudanca();
    }

    // Soma no medidor de instabilidade (nunca abaixo de 0)
    public void AddInstabilidade(int valor)
    {
        instabilidade = Mathf.Max(0, instabilidade + valor);
        AvisarMudanca();
    }

    // RotaB/RotaN/RotaR nao ficam na lista de flags: sao derivadas de 'rota'
    private static bool EhFlagDeRota(string nome)
    {
        return nome.Length == 5 && nome.StartsWith("Rota") && OrdemRotas.IndexOf(nome[4]) >= 0;
    }

    // Metodo publico para sincronizar os dicionarios de busca rapida com as listas do Inspector
    public void Inicializar()
    {
        dictVariaveis.Clear(); 
        if (variaveisGlobais != null)
        {
            foreach (var v in variaveisGlobais) 
            {
                if (v != null && !string.IsNullOrEmpty(v.nome) && !dictVariaveis.ContainsKey(v.nome)) 
                    dictVariaveis.Add(v.nome, v.valor); 
            }
        }

        dictReputacoes.Clear();
        if (reputacoes != null)
        {
            foreach (var r in reputacoes)
            {
                if (r != null && !string.IsNullOrEmpty(r.nome) && !dictReputacoes.ContainsKey(r.nome))
                    dictReputacoes.Add(r.nome, r.pontos);
            }
        }
    }

    // Define o valor de uma flag narrativa booleana
    public void SetVariavel(string nome, bool valor)
    {
        if (string.IsNullOrEmpty(nome)) return;
        if (EhFlagDeRota(nome))
        {
            // Ligar RotaX troca a rota; desligar nao faz nada (sempre existe uma rota)
            if (valor)
            {
                rota = nome.Substring(4);
                AvisarMudanca();
            }
            return;
        }
        if (dictVariaveis.Count == 0 && variaveisGlobais.Count > 0) Inicializar();

        if (dictVariaveis.ContainsKey(nome)) 
        {
            dictVariaveis[nome] = valor; 
            VariavelNarrativa varList = variaveisGlobais.Find(v => v != null && v.nome == nome); 
            if (varList != null) varList.valor = valor;
        }
        else
        {
            dictVariaveis.Add(nome, valor); 
            variaveisGlobais.Add(new VariavelNarrativa { nome = nome, valor = valor });
        }
        AvisarMudanca();
    }

    // Retorna o valor de uma flag narrativa booleana
    public bool GetVariavel(string nome)
    {
        if (string.IsNullOrEmpty(nome)) return false;
        if (EhFlagDeRota(nome)) return rota == nome.Substring(4);
        if (dictVariaveis.Count == 0 && variaveisGlobais.Count > 0) Inicializar();

        if (dictVariaveis.ContainsKey(nome))
            return dictVariaveis[nome];
        return false;
    }

    // Adiciona ou subtrai pontos inteiros de afinidade com um NPC ou animal
    public void AddReputacao(string nome, int pontos)
    {
        if (string.IsNullOrEmpty(nome)) return;
        if (dictReputacoes.Count == 0 && reputacoes.Count > 0) Inicializar();

        if (dictReputacoes.ContainsKey(nome))
        {
            dictReputacoes[nome] += pontos;
            Reputacao repList = reputacoes.Find(r => r != null && r.nome == nome);
            if (repList != null) repList.pontos += pontos;
        }
        else
        {
            dictReputacoes.Add(nome, pontos);
            reputacoes.Add(new Reputacao { nome = nome, pontos = pontos });
        }
        AvisarMudanca();
    }

    // Retorna os pontos de afinidade de um NPC ou animal
    public int GetReputacao(string nome)
    {
        if (string.IsNullOrEmpty(nome)) return 0;
        if (dictReputacoes.Count == 0 && reputacoes.Count > 0) Inicializar();

        if (dictReputacoes.ContainsKey(nome))
            return dictReputacoes[nome];
        return 0;
    }
}
