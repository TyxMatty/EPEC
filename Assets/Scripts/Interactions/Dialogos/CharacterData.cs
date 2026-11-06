using System.Collections.Generic;
using UnityEngine;

// Origem: Assets/Scripts/Interactions/Dialogos/CharacterData.cs
// Dados de um personagem para a caixa de dialogo: nome, cor e retratos por expressao.
// Salvar em Assets/Resources/Personagens/<Nome>.asset; o JSON chama pelo nome do arquivo ("personagem":"Maria").
[CreateAssetMenu(fileName = "NovoPersonagem", menuName = "Scriptable Objects/Personagem")]
public class CharacterData : ScriptableObject
{
    // Nome mostrado na caixa (vazio = nome do arquivo; "{X}" = nome escolhido pelo jogador)
    public string nomeExibido;
    // Cor do nome na caixa de dialogo
    public Color corDoNome = Color.white;
    // Retratos por expressao; uma entrada com 'rota' preenchida substitui a padrao naquela rota
    public List<Expressao> expressoes = new List<Expressao>();

    // Procura expressao + rota atual, depois a expressao sem rota, depois "neutra"
    public Sprite Retrato(string expressao, string rota)
    {
        if (string.IsNullOrEmpty(expressao)) expressao = "neutra";
        Expressao achada = expressoes.Find(e => e != null && e.nome == expressao && e.rota == rota)
                        ?? expressoes.Find(e => e != null && e.nome == expressao && string.IsNullOrEmpty(e.rota))
                        ?? expressoes.Find(e => e != null && e.nome == "neutra" && (e.rota == rota || string.IsNullOrEmpty(e.rota)));
        return achada != null ? achada.sprite : null;
    }
}

[System.Serializable]
public class Expressao
{
    // Nome usado no JSON ("neutra", "feliz", "brava", "sorriso"...)
    public string nome = "neutra";
    // Vazio = vale em todas as rotas; "R" = versao vista por X na Rota R (roteiro: expressoes mais agressivas)
    public string rota;
    public Sprite sprite;
}
