using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

// Testes EditMode de Condicao, Efeito e da rota do GlobalStateManager
public class RegrasNarrativasTests
{
    private GlobalStateManager estado;

    [SetUp]
    public void Criar()
    {
        estado = ScriptableObject.CreateInstance<GlobalStateManager>();
        estado.Resetar();
    }

    [Test]
    public void Condicao_FlagENegar()
    {
        estado.SetVariavel("falouComGi", true);
        Assert.IsTrue(new Condicao { flag = "falouComGi" }.Vale(estado, null));
        Assert.IsFalse(new Condicao { flag = "falouComGi", negar = true }.Vale(estado, null));
        Assert.IsTrue(new Condicao { flag = "naoExiste", negar = true }.Vale(estado, null));
    }

    [Test]
    public void Condicao_ReputacaoMinima()
    {
        estado.AddReputacao("Tobias", 20);
        Assert.IsTrue(new Condicao { reputacao = "Tobias", minimo = 20 }.Vale(estado, null));
        Assert.IsFalse(new Condicao { reputacao = "Tobias", minimo = 21 }.Vale(estado, null));
    }

    [Test]
    public void Condicao_Item()
    {
        var mochila = ScriptableObject.CreateInstance<InventoryData>();
        var chave = ScriptableObject.CreateInstance<ItemData>();
        chave.itemID = "01";
        chave.itemName = "Chave";
        mochila.AddItem(chave);

        Assert.IsTrue(new Condicao { item = "Chave" }.Vale(estado, mochila));
        Assert.IsTrue(new Condicao { item = "01" }.Vale(estado, mochila));
        Assert.IsFalse(new Condicao { item = "Coleira" }.Vale(estado, mochila));
    }

    [Test]
    public void Valem_TodasEQualquer()
    {
        // Espelho do prologo: acordouPrimeiroDia OU respondeuMensagemMaria
        var qualquer = new List<Condicao> { new Condicao { flag = "acordouPrimeiroDia" }, new Condicao { flag = "respondeuMensagemMaria" } };
        Assert.IsFalse(Condicao.Valem(null, qualquer, estado, null));
        estado.SetVariavel("respondeuMensagemMaria", true);
        Assert.IsTrue(Condicao.Valem(null, qualquer, estado, null));

        var todas = new List<Condicao> { new Condicao { rota = "R" } };
        Assert.IsFalse(Condicao.Valem(todas, qualquer, estado, null));
        Assert.IsTrue(Condicao.Valem(null, null, estado, null));
    }

    [Test]
    public void Rota_PassosNaoPassamDosExtremos()
    {
        estado.MudarRota(1);
        Assert.AreEqual("N", estado.rota);
        estado.MudarRota(5);
        Assert.AreEqual("R", estado.rota);
        estado.MudarRota(-9);
        Assert.AreEqual("B", estado.rota);
    }

    [Test]
    public void Rota_FlagsRotaXSaoUmaRotaSo()
    {
        estado.SetVariavel("RotaR", true);
        Assert.AreEqual("R", estado.rota);
        Assert.IsTrue(estado.GetVariavel("RotaR"));
        Assert.IsFalse(estado.GetVariavel("RotaB"));
        Assert.AreEqual(0, estado.variaveisGlobais.Count);
    }

    [Test]
    public void Efeito_AplicaFlagReputacaoRotaEInstabilidade()
    {
        var efeitos = new List<Efeito>
        {
            new Efeito { flag = "aceitaAjudarTobias" },
            new Efeito { flag = "florNegativaDia1", negar = true },
            new Efeito { reputacao = "Tobias", pontos = 10 },
            new Efeito { passosRota = 2 },
            new Efeito { instabilidade = -3 },
        };
        Efeito.AplicarTodos(efeitos, estado);

        Assert.IsTrue(estado.GetVariavel("aceitaAjudarTobias"));
        Assert.IsFalse(estado.GetVariavel("florNegativaDia1"));
        Assert.AreEqual(10, estado.GetReputacao("Tobias"));
        Assert.AreEqual("R", estado.rota);
        Assert.AreEqual(0, estado.instabilidade);
    }
}
