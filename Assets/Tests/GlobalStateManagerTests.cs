using NUnit.Framework;
using UnityEngine;

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
}
