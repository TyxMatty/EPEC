using System.Collections.Generic;

// Origem: Assets/Scripts/Condicao.cs
// Uma checagem de estado usada por dialogos, opcoes, flores e gatilhos. Preencha UM alvo por condicao:
// {"flag":"acordouPrimeiroDia"} | {"reputacao":"Tobias","minimo":20} | {"item":"Chave"} | {"rota":"R"}
// "negar": true inverte o resultado (ex: {"flag":"falouComGi","negar":true} = falouComGi falsa)
[System.Serializable]
public class Condicao
{
    // Flag do GlobalStateManager que precisa estar ligada (RotaB/RotaN/RotaR tambem valem)
    public string flag;
    // Alvo de reputacao que precisa ter pelo menos 'minimo' pontos
    public string reputacao;
    public int minimo;
    // itemName ou itemID que precisa estar na mochila
    public string item;
    // Rota atual exigida: "B", "N" ou "R"
    public string rota;
    // Inverte o resultado da checagem
    public bool negar;

    public bool Vale(GlobalStateManager estado, InventoryData inventario)
    {
        bool resultado;
        if (!string.IsNullOrEmpty(flag)) resultado = estado != null && estado.GetVariavel(flag);
        else if (!string.IsNullOrEmpty(reputacao)) resultado = estado != null && estado.GetReputacao(reputacao) >= minimo;
        else if (!string.IsNullOrEmpty(item)) resultado = inventario != null && inventario.ContemItem(item);
        else if (!string.IsNullOrEmpty(rota)) resultado = estado != null && estado.rota == rota;
        else return true; // condicao sem alvo nao bloqueia nada
        return negar ? !resultado : resultado;
    }

    // true se TODAS de 'todas' valem E (a lista 'qualquer' esta vazia OU ALGUMA dela vale)
    public static bool Valem(List<Condicao> todas, List<Condicao> qualquer, GlobalStateManager estado, InventoryData inventario)
    {
        if (todas != null)
            foreach (var c in todas)
                if (c != null && !c.Vale(estado, inventario)) return false;

        if (qualquer == null || qualquer.Count == 0) return true;
        foreach (var c in qualquer)
            if (c != null && c.Vale(estado, inventario)) return true;
        return false;
    }
}
