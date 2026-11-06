using System.Collections.Generic;
using UnityEngine;

// Origem: Assets/Scripts/Efeito.cs
// Uma mudanca de estado aplicada no fim de um dialogo ou ao escolher uma opcao. Campos vazios sao ignorados:
// {"flag":"aceitaAjudarTobias"} liga (com "negar":true desliga) | {"reputacao":"Tobias","pontos":10}
// {"rota":"R"} define a rota | {"passosRota":1} piora (B->N->R), -1 melhora | {"instabilidade":1} soma no medidor
[System.Serializable]
public class Efeito
{
    // Flag que sera ligada (ou desligada com 'negar')
    public string flag;
    public bool negar;
    // Alvo e pontos de reputacao (negativo subtrai)
    public string reputacao;
    public int pontos;
    // Rota definida diretamente: "B", "N" ou "R"
    public string rota;
    // Passos na ordem B -> N -> R (positivo piora, negativo melhora)
    public int passosRota;
    // Soma no medidor de instabilidade emocional
    public int instabilidade;

    public void Aplicar(GlobalStateManager estado)
    {
        if (estado == null)
        {
            Debug.LogWarning("[Efeito] GlobalStateManager nulo; efeito ignorado.");
            return;
        }

        if (!string.IsNullOrEmpty(flag)) estado.SetVariavel(flag, !negar);
        if (!string.IsNullOrEmpty(reputacao))
        {
            // Pelo ReputationSystem quando ele esta na cena, para mostrar o popup de afinidade
            if (ReputationSystem.Instance != null) ReputationSystem.Instance.AdicionarReputacao(reputacao, pontos);
            else estado.AddReputacao(reputacao, pontos);
        }
        if (!string.IsNullOrEmpty(rota)) estado.DefinirRota(rota);
        if (passosRota != 0) estado.MudarRota(passosRota);
        if (instabilidade != 0) estado.AddInstabilidade(instabilidade);
    }

    public static void AplicarTodos(List<Efeito> efeitos, GlobalStateManager estado)
    {
        if (efeitos == null) return;
        foreach (var e in efeitos)
            if (e != null) e.Aplicar(estado);
    }
}
