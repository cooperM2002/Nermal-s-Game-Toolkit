using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class RagdollOnDeath : MonoBehaviour
{
      /////bodypart mapping
    public enum BodyPart
    {
        Head,
        Chest,
        Hip,
        Arm,
        Hand,
        Leg,
        Knee,
        Other
    }


    //animator object
    public Animator animator;
    public CharacterController controller;

    private Rigidbody[] ragdollBodies;
    private Animator[] allAnimators;

    private bool isDead = false;



    private void Awake()
    {
        ragdollBodies = GetComponentsInChildren<Rigidbody>();   //get all ragdoll bodies
        SetRagdoll(false);  //start entity alive on start
        allAnimators = GetComponentsInChildren<Animator>();


        if (animator == null)
            animator = GetComponent<Animator>();
        SetRagdoll(false);

    }


    private void SetRagdoll(bool enabled)
    {
        foreach (var rb in ragdollBodies)
        {
            rb.isKinematic = !enabled;
            rb.useGravity = enabled;
        }
    }//end setragdoll


    ////////////////////////////////
    ///  APPLY DEATH TO RAGDOLL  ///
    ////////////////////////////////

    public void DieFromHit(Transform hitBone, Vector3 hitPoint, Vector3 hitDirection, float baseForce = 5f)
    {

        //death
        if (isDead) return; //already dead
        isDead = true;      //die, dead
        Debug.Log("RagdollOnDeath.Die() called on " + gameObject.name);
        
        foreach (var anim in allAnimators)
        {
            if (anim != null)
                anim.enabled = false;
        }

        //turn off 
        if (animator != null) animator.enabled = false;
        if (controller != null) controller.enabled = false; //controller

        SetRagdoll(true);

        //get WHICH body part hit
        BodyPart part = GetBodyPart(hitBone);

        //decide behaviour depending on part
        float finalForce = baseForce;
        OnBodyPartHit(part, hitBone, hitPoint, ref finalForce);

        //ragdoll HIT ON BONE
        ApplyHitForce(
            hitBone,        //which bone
            hitPoint,       //where
            hitDirection,   //in dir
            finalForce       //with this force
        );


    }//end die

    ////////////////////
    /// GET HIT BONE ///
    ////////////////////
    /*
    
        returns bone hit by ray

    */
    private Rigidbody GetHitRigidBody(Transform hitBone)
    {

        //if no rigidbody
        if (hitBone == null) { return null; }

        //gameobject hit by ray
        Rigidbody rbHit = hitBone.GetComponent<Rigidbody>();    //get rigidbody of hit object
        if (rbHit != null) { return rbHit; }                        //return that rigidbody component

        //if no rigidbody on that bone object
        if (ragdollBodies != null && ragdollBodies.Length > 0)
        {
            return ragdollBodies[0];//return hip by default
        }

        return null;
    }


    ////////////////////
    /// APPLY FORCE  ///
    ////////////////////
    /*
    
        applies force to rigidbody

    */
    private void ApplyHitForce(Transform hitBone, Vector3 hitPoint, Vector3 hitDirection, float force)
    {
        //the bone that was hit
        Rigidbody target = GetHitRigidBody(hitBone);

        //if empty
        if (target == null)
        {
            return;
        }

        //direction
        hitDirection.Normalize();

        //apply force
        target.AddForceAtPosition(
            hitDirection * force,
            hitPoint,
            ForceMode.Impulse
        );

    }

    ///////////////////////////////////////////
    ///                                     ///
    ///     SPECIFIC BODYPART BEHAVIOUR     ///
    ///                                     ///
    ///////////////////////////////////////////
  

    /*
    
        Head,
        Chest,
        Hip,
        Arm,
        Hand,
        Leg,
        Knee,
        Other

        hip,
            chest,
                head,
                upperarm.l
                    lowerarm.l
                        hand.l
                upperarm.r
                    lowerarm.r
                        hand.r
                upperleg.l
                    lowerleg.l
                        foot.l
                upperleg.r
                    lowerleg.r
                        foot.r
    */
    private BodyPart GetBodyPart(Transform bone)
    {
        //base case
        if (bone == null) return BodyPart.Other;


        string nameBodyPart = bone.name.ToLower();

        //HEAD, "head"
        if (nameBodyPart.Contains("head"))
        {
            return BodyPart.Head;
        }

        //TORSO, "chest", "hip"
        if (nameBodyPart.Contains("chest") || nameBodyPart.Contains("hips"))
        {
            return BodyPart.Chest;
        }

        //ARM, "upperarm.l .r", "lowerarm.l .r", "hand.l .r"
        if (
            nameBodyPart.Contains("upperarm.l") ||
            nameBodyPart.Contains("upperarm.r") ||
            nameBodyPart.Contains("lowerarm.l") ||
            nameBodyPart.Contains("lowerarm.r") ||
            nameBodyPart.Contains("hand.l") ||
            nameBodyPart.Contains("hand.r")
        )
        {
            return BodyPart.Arm;
        }

        //ARM, "upperarm.l .r", "lowerarm.l .r", "hand.l .r"
        if (
            nameBodyPart.Contains("upperleg.l") ||
            nameBodyPart.Contains("upperleg.r") ||
            nameBodyPart.Contains("lowerleg.l") ||
            nameBodyPart.Contains("lowerleg.r") ||
            nameBodyPart.Contains("foot.l") ||
            nameBodyPart.Contains("foot.r")
        )
        {
            return BodyPart.Leg;
        }

        return BodyPart.Other;
    }
    

    /// <summary>
    /// IMPORTANT
    /// </summary>
    /// <param name="part"></param>
    /// <param name="hitBone"></param>
    /// <param name="hitPoint"></param>
    /// <param name="force"></param>
    private void OnBodyPartHit(BodyPart part, Transform hitBone, Vector3 hitPoint, ref float force)
    {
        switch (part)
        {
            case BodyPart.Head:
                // Example: stronger knockback, extra effects
                force *= 30f;
                Debug.Log("HEADSHOT on " + hitBone.name);
                // TODO: spawn headshot VFX, award bonus points, etc.
                break;

            case BodyPart.Chest:
                force *= 1.0f;
                Debug.Log("Torso hit");
                // TODO: normal behaviour
                break;

            case BodyPart.Arm:
                force *= 0.8f;
                Debug.Log("Arm hit");
                // TODO: maybe disarm, drop weapon, etc.
                break;

            case BodyPart.Leg:
                force *= 10f;
                Debug.Log("Leg hit");
                // TODO: maybe slower movement if you do non-instant kills later
                break;

            case BodyPart.Other:
                Debug.Log("Other hit: " + hitBone.name);
                break;
        }
    }

}
