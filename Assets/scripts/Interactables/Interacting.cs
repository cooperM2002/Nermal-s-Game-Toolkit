using UnityEngine;

public class Interacting : MonoBehaviour
{


    KeyCode interactionKey = KeyCode.F;
    public float interactionRange = 2.0f;



    //update
    void Update()
    {
        if (Input.GetKeyDown(interactionKey)) AttemptInteraction();
    }



    void AttemptInteraction()
    {
        //get hit
        RaycastHit hitInfo;
        bool hitSomething = TryGetHit(interactionRange, out hitInfo);

        //perform raycast
        if(hitSomething)
        {
            //try to get interactable object
            Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
            if(interactable != null) interactable.Interact(gameObject); //it has been interacted with
        }

    }


    /// <summary>
    /// Lives on the Main Camera, performs check raycasts with a player mask. For interactions.
    /// </summary>
    /// <param name="range"></param>
    /// <param name="hit"></param>
    /// <returns></returns>
    public bool TryGetHit(float range, out RaycastHit hit)
    {
        Ray ray = new Ray(transform.position, transform.forward);   //origin, direction

        int notPlayer = ~(1 << LayerMask.NameToLayer("Player"));    //look @ ~(Player)
        int mask = Physics.DefaultRaycastLayers & notPlayer;        //everything except player is looked at

        bool hitSomething = Physics.Raycast(

            ray,        //origin, direction
            out hit,    //CBR on hitInfo, modifies hitInfo
            range,      //max distance
            mask        //only on specified layers
            );

        return hitSomething;
    }
}
