using NUnit.Framework;
using UnityEngine;

// Confere os JSONs de Resources/Dialogos (todos abrem e todo proximoDialogo existe) e a cadeia de exemplo do Cap 1
public class DialogosJsonTests
{
    private GlobalStateManager estado;

    [SetUp]
    public void Criar()
    {
        estado = ScriptableObject.CreateInstance<GlobalStateManager>();
        estado.Resetar();
    }

    [Test]
    public void TodosOsDialogosAbremEEncadeiam()
    {
        TextAsset[] arquivos = Resources.LoadAll<TextAsset>("Dialogos");
        Assert.IsNotEmpty(arquivos, "Nenhum JSON em Resources/Dialogos");

        foreach (var arquivo in arquivos)
        {
            DialogueData data = JsonUtility.FromJson<DialogueData>(arquivo.text);
            Assert.IsNotNull(data, arquivo.name);
            if (data.opcoes == null) continue;

            foreach (var opcao in data.opcoes)
            {
                Assert.IsFalse(string.IsNullOrEmpty(opcao.texto), $"{arquivo.name}: opcao sem texto");
                if (!string.IsNullOrEmpty(opcao.proximoDialogo))
                    Assert.IsNotNull(Resources.Load<TextAsset>("Dialogos/" + opcao.proximoDialogo),
                        $"{arquivo.name}: proximoDialogo '{opcao.proximoDialogo}' nao existe");
            }
        }
    }

    [Test]
    public void Cap1_GiFalaDiferenteSeFalouComGi()
    {
        DialogueData data = DialogueSystemGlobal.CarregarDialogo("cap1_abrigo_apresentacao");

        var falas = data.linhas.FindAll(l => Condicao.Valem(l.condicoes, l.condicoesQualquer, estado, null));
        Assert.AreEqual(4, falas.Count);
        StringAssert.StartsWith("Vejo que", falas[2].texto);

        estado.SetVariavel("falouComGi", true);
        falas = data.linhas.FindAll(l => Condicao.Valem(l.condicoes, l.condicoesQualquer, estado, null));
        Assert.AreEqual(4, falas.Count);
        StringAssert.StartsWith("{X}!", falas[2].texto);
    }

    [Test]
    public void Cap1_RecusarTobiasLevaARotaR()
    {
        // Percorre a cadeia aplicando os efeitos na mesma ordem que o DialogueSystemGlobal
        DialogueData inicio = DialogueSystemGlobal.CarregarDialogo("cap1_abrigo_apresentacao");
        Opcao recusar = inicio.opcoes.Find(o => o.texto == "Ando muito ocupado...");
        Efeito.AplicarTodos(recusar.efeitos, estado);

        DialogueData proximo = DialogueSystemGlobal.CarregarDialogo(recusar.proximoDialogo);
        Efeito.AplicarTodos(proximo.efeitos, estado);

        Assert.AreEqual("R", estado.rota);
        Assert.IsFalse(estado.GetVariavel("aceitaAjudarTobias"));
        Assert.IsTrue(estado.GetVariavel("florNegativaDia1"));
    }
}
