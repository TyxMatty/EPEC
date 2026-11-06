using System.Collections.Generic;

// Origem: Assets/Scripts/Mecanicas/CasoData.cs
// Classes do JSON de investigacao ("juntar as pontas"), ex: Assets/Dados/Investigacoes/cap1_tobias_motos.json.
// Uma pista aparece no quadro quando suas condicoes valem (normalmente a flag ligada por um dialogo: {"flag":"pistaMotoRapida"}).
// O jogador marca pistas e junta: se o conjunto marcado for igual ao de uma conclusao, ela vale.
[System.Serializable]
public class CasoData
{
    public string titulo;
    public List<Pista> pistas = new List<Pista>();
    public List<Conclusao> conclusoes = new List<Conclusao>();
    // Texto mostrado quando a combinacao nao leva a nada
    public string erro = "Isso nao se encaixa...";

    // Conclusao cujo conjunto de pistas e exatamente o marcado (ordem nao importa); null = nenhuma
    public Conclusao Encontrar(List<string> marcadas)
    {
        return conclusoes.Find(c => c != null && c.pistas.Count == marcadas.Count && c.pistas.TrueForAll(marcadas.Contains));
    }
}

[System.Serializable]
public class Pista
{
    // Id usado nas conclusoes
    public string id;
    public string texto;
    // Pista visivel quando todas valem (vazio = sempre visivel)
    public List<Condicao> condicoes = new List<Condicao>();
}

[System.Serializable]
public class Conclusao
{
    // Ids das pistas que juntas levam a esta conclusao
    public List<string> pistas = new List<string>();
    public string texto;
    public List<Efeito> efeitos = new List<Efeito>();
}
