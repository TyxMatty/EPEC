using UnityEngine;
public class Interactable : MonoBehaviour
{
    [Header("Diálogo do Objeto")]
    [TextArea(3, 5)]
    public string[] falas; // As falas específicas deste NPC ou objeto
    public bool hasSpecialInteraction; // Se o objeto tem uma interação especial, como abrir uma porta ou iniciar um minigame -> ser mais escalável;
    public bool isNonPickableObject; // se é um objeto não pegável
    public bool isNPC; // se é um NPC
    public bool isPickableObject; // se é um objeto pegável
// todas essas bools podem ou não ser usadas, dependendo do que o objeto for, mas é bom ter elas para organizar
// melhor o código e deixar mais escalável, caso queira adicionar mais tipos de interação no futuro.
// isso são boas práticas de programação, para deixar o código mais limpo e organizado, e também para facilitar a manutenção do código no futuro.
}
