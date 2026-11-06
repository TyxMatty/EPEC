using NUnit.Framework;
using UnityEngine;

// Testes EditMode do GlobalStateManager (rodar pelo Test Runner)
// Fica em Editor/ para compilar no Assembly-CSharp-Editor: o NUnit nao existe na build do player
public class GlobalStateManagerTests
{
    [Test]
    public void SetVariavel_AdicionaERecuperaNovaVariavelCorretamente()
    {
        // Arrange
        var stateManager = ScriptableObject.CreateInstance<GlobalStateManager>();
        stateManager.variaveisGlobais = new System.Collections.Generic.List<VariavelNarrativa>();

        // Act
        stateManager.SetVariavel("teste_tdd", true);
        bool result = stateManager.GetVariavel("teste_tdd");

        // Assert
        Assert.IsTrue(result);
    }

    [Test]
    public void GetVariavel_RetornaFalseSeVariavelNaoExistir()
    {
        // Arrange
        var stateManager = ScriptableObject.CreateInstance<GlobalStateManager>();
        stateManager.variaveisGlobais = new System.Collections.Generic.List<VariavelNarrativa>();

        // Act
        bool result = stateManager.GetVariavel("variavel_inexistente");

        // Assert
        Assert.IsFalse(result);
    }

    [Test]
    public void Resetar_ApagaVariaveisEReputacoes()
    {
        var stateManager = ScriptableObject.CreateInstance<GlobalStateManager>();
        stateManager.SetVariavel("portaAberta", true);
        stateManager.AddReputacao("Tobias", 10);

        stateManager.Resetar();

        Assert.IsFalse(stateManager.GetVariavel("portaAberta"));
        Assert.AreEqual(0, stateManager.GetReputacao("Tobias"));
        Assert.AreEqual(0, stateManager.variaveisGlobais.Count);
    }

    [Test]
    public void Mudou_DisparaEmFlagERota()
    {
        var stateManager = ScriptableObject.CreateInstance<GlobalStateManager>();
        stateManager.Resetar();
        int avisos = 0;
        stateManager.Mudou += () => avisos++;

        stateManager.SetVariavel("florNegativaDia1", true);
        stateManager.DefinirRota("R");

        Assert.AreEqual(2, avisos);
    }
}
