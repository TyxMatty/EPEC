using UnityEngine;
using System.Collections.Generic;

// Origem: Assets/Scripts/Interactions/Dialogos/DialogueData.cs
// Espelho do JSON de dialogo lido via JsonUtility (cada campo aqui = uma chave do .json)
// REVIEW: JsonUtility ignora sem aviso chaves que nao existem aqui: o exemplo do Docs/README.md (variavelParaAlterar,
// textoPergunta, variavelDaEscolha, temTimer) e o valorEsperado do testeCondicao.json nao fazem nada.
[System.Serializable]
public class DialogueData
{
    // ID do dialogo (nenhum script le hoje; teste.json e testeCondicao.json repetem maria_prologo_01)
    public string dialogueID;
    // Falas exibidas em ordem pelo typewriter
    public List<string> falas;
    
    // Se true, grava variavelAlteradaPorDialogo = true ao fim do dialogo normal
    public bool alteraVariavel;
    public string variavelAlteradaPorDialogo;

    [Header("Reputacao - Dialogo Normal")]
    // Se true, soma pontosReputacao em alvoReputacao ao fim do dialogo normal
    public bool alteraReputacao;
    public string alvoReputacao;
    public int pontosReputacao; 
    
    // Se true, testa as condicoes abaixo e usa falasAlternativas quando elas batem
    public bool temFrasesAlternativas;
    // Flag do GlobalStateManager exigida (vazio = sem condicao de flag)
    public string variavelDaCondicao;
    // itemName ou itemID exigido na mochila (vazio = sem condicao de item)
    public string itemDaCondicao;
    // Inverte as condicoes: as alternativas tocam quando a flag/item NAO existe
    public bool esperaCondicaoFalsa;
    // Falas usadas quando as condicoes batem
    public List<string> falasAlternativas;
    
    // Se true, grava variavelAlteradaPorDialogoAlternativo = true ao fim das falas alternativas
    public bool dialogoAlternativoAlteraVariavel;
    public string variavelAlteradaPorDialogoAlternativo;

    [Header("Reputacao - Dialogo Alternativo")]
    // Se true, soma pontosReputacaoAlternativo em alvoReputacaoAlternativo ao fim das falas alternativas
    public bool dialogoAlternativoAlteraReputacao;
    public string alvoReputacaoAlternativo;
    public int pontosReputacaoAlternativo;

    // Se true, abre o ChoiceSystemGlobal no fim e grava o bool escolhido em variavelParaSalvar
    public bool terminaEmEscolha;
    public string variavelParaSalvar;
    // Texto de cada opcao e o bool gravado por cada uma (mesma ordem)
    public List<string> textosDasOpcoes;
    public List<bool> valoresDasOpcoes;

    // Timer da escolha em segundos; ao zerar grava escolhaNeutra
    public bool temTempoLimite;
    public float tempoLimite;
    public bool escolhaNeutra;
}
