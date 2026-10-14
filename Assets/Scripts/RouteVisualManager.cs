using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class RouteVisualManager : MonoBehaviour
{
    [SerializeField] private Volume volumeRotaR;
    [SerializeField] private AudioSource trilhaNormal;
    [SerializeField] private AudioSource trilhaRotaR;
    [SerializeField, Range(0f, 1f)] private float volumeMax = 0.8f;
    [SerializeField, Min(0.01f)] private float duracao = 2f;

    private float intensidade;// 0 = rota normal, 1 = Rota R completa
    private bool isAtiva;// Verifica se está ativa
    private Coroutine transicao;

    private void Awake()
    {
        trilhaRotaR.loop = true;
        
        // Garante que a trilha R comece tocando em silêncio
        trilhaRotaR.volume = 0f;
        trilhaRotaR.Play(); 
        
        Aplicar(0f);
    }

    public void SetRota(bool ativa)
    {
        if (ativa == isAtiva) return;// Ignora chamadas repetidas
        isAtiva = ativa;

        if (transicao != null) StopCoroutine(transicao);
        transicao = StartCoroutine(Transicao(ativa ? 1f : 0f));
    }

    private IEnumerator Transicao(float alvo)
    {
        // Impede que duas músicas toquem juntas 
        while (!Mathf.Approximately(intensidade, alvo))
        {
            intensidade = Mathf.MoveTowards(intensidade, alvo, Time.deltaTime / duracao);
            Aplicar(intensidade);
            yield return null;
        }

        intensidade = alvo;// Garante o valor exato no final
        Aplicar(intensidade);
        transicao = null;
    }

    private void Aplicar(float t)
    {
        volumeRotaR.weight = t;
        trilhaRotaR.volume = t * volumeMax;
        trilhaNormal.volume = (1f - t) * volumeMax;
    }
}
