using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class DialogueData
{
    public string dialogueID;
    public List<string> falas;
    
    public bool alteraVariavel;
    public string variavelAlteradaPorDialogo;
    
    public bool temFrasesAlternativas;
    public string variavelDaCondicao;
    public string itemDaCondicao;
    public string condicaoEsperada;
    public List<string> falasAlternativas;
    
    public bool dialogoAlternativoAlteraVariavel;
    public string variavelAlteradaPorDialogoAlternativo;

    public bool terminaEmEscolha;
    public string variavelParaSalvar;
    public List<string> textosDasOpcoes;
    public List<bool> valoresDasOpcoes;

    public bool temTempoLimite;
    public float tempoLimite;
    public bool escolhaNeutra;
}
