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
// REVIEW: por ser asset, tudo que SetVariavel/AddReputacao grava em Play Mode fica salvo no MemoriaDoJogo.asset
// (ja commitado com portaAberta=1 e aceitaAjudarTobias=1) e a build comeca com esses valores. Falta um Resetar() no Novo Jogo.
[CreateAssetMenu(fileName = "GlobalStateManager", menuName = "Scriptable Objects/GlobalStateManager")]
public class GlobalStateManager : ScriptableObject
{
    [Header("Memoria Persistente do Jogo")]
    // Lista visivel no Inspector contendo todas as variaveis booleanas narrativas
    public List<VariavelNarrativa> variaveisGlobais = new List<VariavelNarrativa>(); 
    
    [Header("Afinidade com NPCs e Animais")]
    // Lista visivel no Inspector contendo os pontos de reputacao de cada NPC/animal
    public List<Reputacao> reputacoes = new List<Reputacao>(); 
    
    // Cache de busca rapida espelhando as listas acima (montado em Inicializar)
    // REVIEW: editar uma flag no Inspector durante o Play nao atualiza o dicionario e GetVariavel devolve o valor antigo.
    // Com ~30 flags um List.Find ja e rapido; apagar os dicts elimina essa dessincronia.
    private Dictionary<string, bool> dictVariaveis = new Dictionary<string, bool>();
    private Dictionary<string, int> dictReputacoes = new Dictionary<string, int>();

    private void OnEnable()
    {
        Inicializar();
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
    }

    // Retorna o valor de uma flag narrativa booleana
    public bool GetVariavel(string nome)
    {
        if (string.IsNullOrEmpty(nome)) return false;
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
