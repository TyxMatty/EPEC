using UnityEngine;

[CreateAssetMenu(fileName = "NewScriptableObjectScript", menuName = "Scriptable Objects/NewScriptableObjectScript")]
public class NewScriptableObjectScript : ScriptableObject
{
    [Header("Speed configurations")]
public float baseSpeed = 5f;
public float shiftMultiplier = 1.5f
}
void Update()
{
    float moveX = Input.GetAxisRaw("Horizontal")

}
