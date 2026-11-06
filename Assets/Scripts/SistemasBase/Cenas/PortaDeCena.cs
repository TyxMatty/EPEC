using UnityEngine;

// Origem: Assets/Scripts/SistemasBase/Cenas/PortaDeCena.cs
// Leva o player para outra cena. Com collider normal (layer de interacao) troca ao apertar Interact;
// com collider "Is Trigger" troca ao encostar (saidas de rua, fim de corredor).
// O PontoDeSpawn de volta precisa ficar fora do trigger da porta, senao o player e mandado de volta.
public class PortaDeCena : MonoBehaviour, IInteractAction
{
    // Nome da cena de destino (como no Build Profiles) e id do PontoDeSpawn nela
    public string cena;
    public string spawn;

    public void OnInteract(InteractionSystem interactor)
    {
        Abrir();
    }

    void OnTriggerEnter2D(Collider2D outro)
    {
        if (outro.GetComponent<MovementScript>() != null) Abrir();
    }

    private void Abrir()
    {
        if (SceneTransitionManager.Instance == null)
        {
            Debug.LogError("[PortaDeCena] Nenhum SceneTransitionManager (prefab Nucleo) na cena.");
            return;
        }
        SceneTransitionManager.Instance.IrPara(cena, spawn);
    }
}
