using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

// Confere o caso do Cap 1 (Assets/Dados/Investigacoes/cap1_tobias_motos.json): pistas visiveis e combinacoes
public class InvestigacaoTests
{
    private GlobalStateManager estado;
    private CasoData caso;

    [SetUp]
    public void Criar()
    {
        estado = ScriptableObject.CreateInstance<GlobalStateManager>();
        estado.Resetar();
        TextAsset json = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Dados/Investigacoes/cap1_tobias_motos.json");
        caso = JsonUtility.FromJson<CasoData>(json.text);
    }

    private List<Pista> Visiveis()
    {
        return caso.pistas.FindAll(p => Condicao.Valem(p.condicoes, null, estado, null));
    }

    [Test]
    public void PistaSoApareceComAFlag()
    {
        Assert.IsEmpty(Visiveis());

        estado.SetVariavel("pistaMotoRapida", true);
        estado.SetVariavel("fezCarinhoTobias", true);
        CollectionAssert.AreEquivalent(new[] { "motoRapida", "tobiasDocil" }, Visiveis().ConvertAll(p => p.id));
    }

    [Test]
    public void ConclusaoIgnoraOrdemMasExigeOConjuntoExato()
    {
        Assert.IsNotNull(caso.Encontrar(new List<string> { "tobiasCorre", "motoRapida" }));
        Assert.IsNull(caso.Encontrar(new List<string> { "motoRapida" }));
        Assert.IsNull(caso.Encontrar(new List<string> { "motoRapida", "tobiasCorre", "tobiasDocil" }));
        Assert.IsNull(caso.Encontrar(new List<string> { "motoRapida", "motoqueiroXinga" }));
    }

    [Test]
    public void ConclusaoLigaDescobriuGatilhoTobias()
    {
        Conclusao achada = caso.Encontrar(new List<string> { "motoRapida", "tobiasCorre" });
        Efeito.AplicarTodos(achada.efeitos, estado);
        Assert.IsTrue(estado.GetVariavel("descobriuGatilhoTobias"));
    }
}
