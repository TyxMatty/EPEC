using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VariavelNarrativa
{
    public string nome;
    public bool valor;
}

[CreateAssetMenu(fileName = "GlobalStateManager", menuName = "Scriptable Objects/GlobalStateManager")]
public class GlobalStateManager : ScriptableObject
{
    [Header("Memoria Persistente do Jogo")]
    public List<VariavelNarrativa> variaveisGlobais = new List<VariavelNarrativa>(); 
    
    // dictionary invisivel pro inspector, mas ultra rapido pro jogo rodar, testando...
    private Dictionary<string, bool> dictVariaveis = new Dictionary<string, bool>();

    public void Inicializar()
    {
        dictVariaveis.Clear(); // limpa as variaveís 
        foreach (var v in variaveisGlobais) // constantemnte faz isso
        {
            if (!dictVariaveis.ContainsKey(v.nome)) // se nao tiver a variavel la,
                dictVariaveis.Add(v.nome, v.valor); // adiciona
        }
    }

    public void SetVariavel(string nome, bool valor)
    {
        if (dictVariaveis.ContainsKey(nome)) // se tiver o nome,
        {
            dictVariaveis[nome] = valor; // cria, silenciosamente no dicionario, essa variavel
            VariavelNarrativa varList = variaveisGlobais.Find(v => v.nome == nome); // copia essa variavel na var lista Variaveis Globais
            if (varList != null) varList.valor = valor;
        }
        else
        {
            dictVariaveis.Add(nome, valor); 
            variaveisGlobais.Add(new VariavelNarrativa { nome = nome, valor = valor });
        }
    }

    public bool GetVariavel(string nome)
    {
        if (dictVariaveis.ContainsKey(nome))
            return dictVariaveis[nome];
        return false;
    }
}