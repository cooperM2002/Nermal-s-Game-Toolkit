using UnityEngine;

public class Interacting : MonoBehaviour
{

    KeyCode interactionKey = KeyCode.E;
    public float interactionRange = 2.0f;

    //start
    void Start()
    {
        
    }

    //update
    void Update()
    {
        if (Input.GetKeyDown(interactionKey)){
            AttemptInteraction();
        }
    }

    void AttemptInteraction()
    {
        //create a ray from curr pos in the forward direction
        var ray = new Ray(transform.position, transform.forward);

        RaycastHit hit;

        //layer mask respresents everything except player
        var notPlayer = ~(1 << LayerMask.NameToLayer("Player"));

        //
        var layerMask = Physics.DefaultRaycastLayers & notPlayer;

        //raycast out only onto every layer except layer, everything except player
        if(Physics.Raycast(ray, out hit, interactionRange, layerMask))
        {
            //try to get interactable object
            var interactable = hit.collider.GetComponent<Interactable>();

            //does it exist?
            if(interactable != null)
            {
                //it has been interacted with
                interactable.Interact(gameObject);
            }
        }

    }
}
