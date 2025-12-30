using Unity.VisualScripting;
using UnityEngine;

//IMPLEMENTS "grabbing" which is the action of continuously HOLDING something
//TODO: if object is ItemPickup/ItemDefinition item => hold E -> grab
//TODO: if object is static/prop prefab => ie normal rigidbody -> normal logic

public class Grabber : MonoBehaviour
{
    public Interacting interacting; 

    public Transform holdPoint = null;  // hold point on player
    private FixedJoint connectionJoint; // joint that connects two rigidbodies
    private Rigidbody grabbedBody;      // rigidbody that was grabbed

    public KeyCode grabKey = KeyCode.E;         //interact key
    public KeyCode throwKey = KeyCode.Mouse0;   //throwkey

    public float grabRange = 3f;
    public float pullRange = 20f;

    public float throwForce = 100f;
    public float pullForce = 50f;

    public float grabBreakForce = 100f;     //normal force
    public float grabBreakTorque = 100f;    //rotational force

   
    private void Awake()
    {
        if (holdPoint == null)
        {
            Debug.LogError("Grab hold point must not be null!");
        }

        if (holdPoint.IsChildOf(transform) == false)
        {
            Debug.LogError("Grab hold point must be a child of this object");
        }

        //players collider
        Collider playerCollider = GetComponentInParent<Collider>();
        Debug.Log(playerCollider.gameObject.name+"");

        //
        playerCollider.gameObject.layer = LayerMask.NameToLayer("Player");
    }


    void Update()
    {
        //not HOLDING -> try PULL (E) 
        if (Input.GetKeyDown(grabKey) && connectionJoint == null) //E & ~(holding)
            AttemptPull();

        //HOLDING -> DROP (E)              
        else if (Input.GetKeyDown(grabKey) && connectionJoint != null) //E & holding
            Drop();

        //HOLDING -> THROW (M0)           
        else if (Input.GetKeyDown(throwKey) && connectionJoint != null) //m0 & holding
            Throw();
    }


    /// <summary>
    /// throws grabbed rigidbody w/ throwforce
    /// </summary>
    
    void Throw()
    {
        //not holding
        if(grabbedBody == null) return;
        
        //apply force
        Rigidbody thrownBody = grabbedBody;             //object to be thrown
        Vector3 force = transform.forward * throwForce; //thrown force
        thrownBody.AddForce(force);                     //adds force @ direction * magnitude

        Drop();

    }


    /// <summary>
    /// attempts to pull not held object 
    /// performs raycast through middle of camera,
    /// if the raycast hits a rigidbody that is not kinematic, pick its up
    /// </summary>
    void AttemptPull()
    {
        //get hit
        RaycastHit hitInfo;
        bool hitSomething = interacting.TryGetHit(grabRange, out hitInfo);

        //CHECKS
        if (hitSomething == false) return;  //hit empty
        grabbedBody = hitInfo.rigidbody;    //hit object rigidbody

        if (grabbedBody == null || grabbedBody.isKinematic) //return if not rigidbody or is kinematic
            return;

        ////////////////////////////////
        ///  grab logic starts here  ///
        ////////////////////////////////

        //if within pull range
        if (hitInfo.distance < pullRange)
        {
            grabbedBody.transform.position = holdPoint.position;

            //create and configure grabjoint component 
            connectionJoint = gameObject.AddComponent<FixedJoint>();
            connectionJoint.connectedBody = grabbedBody;
            connectionJoint.breakForce = grabBreakForce;
            connectionJoint.breakTorque = grabBreakTorque;

            //ignore parents colliders
            foreach (Collider c in GetComponentsInParent<Collider>())
            {
                Physics.IgnoreCollision(c, hitInfo.collider, true);
            }
        }
        else
        {
            Vector3 pull = -transform.forward * this.pullForce;
        }

    }

    //drops held object
    void Drop()
    {
        if (connectionJoint != null) Destroy(connectionJoint);
        if (grabbedBody == null) return;
        
        //reenable collisions on this object and its colliders
        foreach(Collider c in GetComponentsInParent<Collider>())
            Physics.IgnoreCollision(c, grabbedBody.GetComponent<Collider>(), false);
        
        grabbedBody = null;

    }


    private void OnDrawGizmos()
    {
        if (holdPoint == null)return;
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(holdPoint.position, 0.2f);
    }

    // Called when a joint that's attached to the game object this
    // component is on has broken.
    private void OnJointBreak(float breakForce)
    {
        // When our joint breaks, call Drop to ensure that we clean up
        // after ourselves.
        Drop();
    }


}
