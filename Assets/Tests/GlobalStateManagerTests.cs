using NUnit.Framework;
using UnityEngine;

// Testes EditMode do GlobalStateManager (rodar pelo Test Runner)
// REVIEW: sem asmdef nem pasta Editor este arquivo entra no Assembly-CSharp; o NUnit nao vai para a build do player,
// entao a build provavelmente quebra. Mover para Assets/Tests/Editor/ resolve (confirmar com um Build).
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
