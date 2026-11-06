using UnityEngine;
using System.Collections.Generic;

// Origem: Assets/Scripts/Interactions/Dialogos/DialogueData.cs
// Espelho do JSON de dialogo lido via JsonUtility (cada campo aqui = uma chave do .json)
// O JsonUtility ignora sem aviso chaves que nao existem aqui: um nome errado no JSON simplesmente nao faz nada.
// Formato v2 (linhas/opcoes/efeitos) convive com o antigo; ver Docs/README.md secao 7.
[System.Serializable]
public class DialogueData
{
    // ID do dialogo (nenhum script le hoje; teste.json e testeCondicao.json repetem maria_prologo_01)
    public string dialogueID;

    [Header("Formato v2")]
    // Falas com falante, expressao e condicao por fala; se preenchido, falas/falasAlternativas sao ignorados
    public List<Fala> linhas;
    // Opcoes ao fim das falas: cada uma com condicoes para aparecer, efeitos e o proximo dialogo
    public List<Opcao> opcoes;
    // Efeitos aplicados quando as falas acabam (antes das opcoes)
    public List<Efeito> efeitos;

    [Header("Formato antigo")]
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

// Uma fala do formato v2: {"personagem":"Maria","expressao":"feliz","texto":"Voce veio, {X}!"}
[System.Serializable]
public class Fala
{
    // Nome do personagem (asset em Resources/Personagens); vazio = narracao; "X" = protagonista
    public string personagem;
    // Expressao do retrato (vazio = "neutra")
    public string expressao;
    // Texto da fala; {X} vira o nome do jogador
    public string texto;
    // A fala so aparece se TODAS de 'condicoes' valem e (se houver) ALGUMA de 'condicoesQualquer'
    public List<Condicao> condicoes;
    public List<Condicao> condicoesQualquer;
}

// Uma opcao do formato v2: {"texto":"Pode... ser?","efeitos":[...],"proximoDialogo":"cap1_maria_conta_tobias"}
[System.Serializable]
public class Opcao
{
    // Texto do botao; {X} vira o nome do jogador
    public string texto;
    // A opcao so aparece se as condicoes valem (ex: escondida na Rota R)
    public List<Condicao> condicoes;
    public List<Condicao> condicoesQualquer;
    // Efeitos aplicados ao escolher
    public List<Efeito> efeitos;
    // Nome do JSON em Resources/Dialogos tocado em seguida (vazio = encerra o dialogo)
    public string proximoDialogo;
}
