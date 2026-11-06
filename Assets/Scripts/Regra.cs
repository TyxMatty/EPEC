using System.Collections.Generic;
using UnityEngine;

// Origem: Assets/Scripts/Regra.cs
// "Se estas condicoes valem, aplique estes efeitos". Usada pelo FimDoDia para escolher a flor do dia
// (ex: Assets/Dados/Regras/flor_dia1.json) e reaproveitavel por qualquer resumo que precise de "a primeira que bater".
[System.Serializable]
public class Regra
{
    // Nome so para log e leitura do JSON
    public string nome;
    public List<Condicao> condicoes = new List<Condicao>();
    public List<Condicao> condicoesQualquer = new List<Condicao>();
    public List<Efeito> efeitos = new List<Efeito>();
}

// Raiz do JSON: { "limpar": ["flagA", ...], "regras": [ {...}, ... ] }
[System.Serializable]
public class ListaDeRegras
{
    // Flags desligadas antes de aplicar a regra escolhida (o resumo do dia vira um estado so)
    public List<string> limpar = new List<string>();
    // Em ordem de prioridade: vale a PRIMEIRA cujas condicoes batem
    public List<Regra> regras = new List<Regra>();

    // Avalia tudo antes de mexer no estado (as condicoes leem os valores de antes), aplica e devolve a regra (null = nenhuma)
    public Regra Aplicar(GlobalStateManager estado, InventoryData inventario)
    {
        Regra escolhida = regras.Find(r => r != null && Condicao.Valem(r.condicoes, r.condicoesQualquer, estado, inventario));
        if (escolhida == null) return null;

        foreach (string flag in limpar) estado.SetVariavel(flag, false);
        Efeito.AplicarTodos(escolhida.efeitos, estado);
        return escolhida;
    }
}
