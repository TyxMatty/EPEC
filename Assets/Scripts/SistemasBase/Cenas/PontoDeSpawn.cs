using UnityEngine;

// Origem: Assets/Scripts/SistemasBase/Cenas/PontoDeSpawn.cs
// Marca onde o player aparece ao entrar na cena; a PortaDeCena/FimDoDia pede pelo 'id' (ex: "PortaDaCasa")
public class PontoDeSpawn : MonoBehaviour
{
    public string id;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 0.3f);
    }
}
