using NUnit.Framework;
using UnityEditor;
using UnityEngine;

// Confere as regras da flor do dia 1 (Assets/Dados/Regras/flor_dia1.json) pelo mesmo caminho do FimDoDia
public class FimDoDiaTests
{
    private GlobalStateManager estado;
    private ListaDeRegras regras;

    [SetUp]
    public void Criar()
    {
        estado = ScriptableObject.CreateInstance<GlobalStateManager>();
        estado.Resetar();
        TextAsset json = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Dados/Regras/flor_dia1.json");
        regras = JsonUtility.FromJson<ListaDeRegras>(json.text);
    }

    private void Ligar(params string[] flags)
    {
        foreach (string f in flags) estado.SetVariavel(f, true);
    }

    [Test]
    public void RotaBPerfeita_FlorSuperPositiva_ELimpaAFlorDaManha()
    {
        Ligar("florNeutraPositivaDia1", "tobiasInterage", "protagonistaNumeroVeterinaria",
              "auxiliouTobias", "fezCarinhoTobias", "aceitaAjudarTobias");

        Assert.AreEqual("1 - Colorida, brilhante", regras.Aplicar(estado, null).nome);
        Assert.IsTrue(estado.GetVariavel("florSuperPositivaDia1"));
        Assert.IsFalse(estado.GetVariavel("florNeutraPositivaDia1"));
    }

    [Test]
    public void RotaRSemAbortar_JaNegativa_FlorSuperNegativa()
    {
        Ligar("florNegativaDia1");

        Assert.AreEqual("6 - A beira da morte", regras.Aplicar(estado, null).nome);
        Assert.IsTrue(estado.GetVariavel("florSuperNegativaDia1"));
        Assert.IsFalse(estado.GetVariavel("florNegativaDia1"));
    }

    [Test]
    public void AbortouSemCombinacaoDoRoteiro_NenhumaRegra_NaoMexe()
    {
        Ligar("abortedRotaR", "florNeutraNegativaDia1");

        Assert.IsNull(regras.Aplicar(estado, null));
        Assert.IsTrue(estado.GetVariavel("florNeutraNegativaDia1"));
    }
}
