using System.Collections.Generic;
using UnityEngine;

// a classe foi movida para aqui, tornando-se acessível a qualquer script do jogo
[System.Serializable]
public class VariavelNarrativa
{
    public string nome;
    public bool valor;
}

[CreateAssetMenu(fileName = "GlobalStateManager", menuName = "Scriptable Objects/GlobalStateManager")] // permite criar um asset do tipo GlobalStateManager no menu de criação de assets
public class GlobalStateManager : ScriptableObject
{
    [Header("Memória Persistente do Jogo")]
    public List<VariavelNarrativa> variaveisGlobais = new List<VariavelNarrativa>(); // MESMO DO CHOICESYSTEM GLOBAL, SÓ QUE PARA SEMPRE
}