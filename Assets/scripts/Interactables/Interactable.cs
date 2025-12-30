using UnityEngine;


[RequireComponent(typeof(Collider))]
public class Interactable : MonoBehaviour
{

    /// <summary>
    /// Overridable generic interact method, called on ray hit of Interactables collider.
    /// </summary>
    /// <param name="fromObject"></param>
    public virtual void Interact(GameObject fromObject)
    {
        Debug.LogFormat($"interacted with by {fromObject.name}", fromObject);
    }


}
