using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class DialogueData
{
    public string dialogueID;
    public List<string> falas;
    
    public bool alteraVariavel;
    public string variavelAlteradaPorDialogo;

    [Header("Reputacao - Dialogo Normal")]
    public bool alteraReputacao;
    public string alvoReputacao;
    public int pontosReputacao; 
    
    public bool temFrasesAlternativas;
    public string variavelDaCondicao;
    public string itemDaCondicao;
    public bool esperaCondicaoFalsa;
    public List<string> falasAlternativas;
    
    public bool dialogoAlternativoAlteraVariavel;
    public string variavelAlteradaPorDialogoAlternativo;

    [Header("Reputacao - Dialogo Alternativo")]
    public bool dialogoAlternativoAlteraReputacao;
    public string alvoReputacaoAlternativo;
    public int pontosReputacaoAlternativo;

    public bool terminaEmEscolha;
    public string variavelParaSalvar;
    public List<string> textosDasOpcoes;
    public List<bool> valoresDasOpcoes;

    public bool temTempoLimite;
    public float tempoLimite;
    public bool escolhaNeutra;
}
