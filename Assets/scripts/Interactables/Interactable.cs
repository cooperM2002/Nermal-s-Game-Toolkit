using UnityEngine;


[RequireComponent(typeof(Collider))]
public class Interactable : MonoBehaviour
{

    public void Interact(GameObject fromObject)
    {
        Debug.LogFormat("interacted with by {0}", fromObject);
    }

    //start
    void Start()
    {
        
    }

    //update
    void Update()
    {
        
    }
}
