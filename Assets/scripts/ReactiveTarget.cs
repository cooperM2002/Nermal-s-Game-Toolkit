using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactiveTarget : MonoBehaviour
{



    /////////////////////
    ///   WHEN SHOT   ///
    /////////////////////

    /*
    
    called within RayShooter, handles ragdoll on/off when target is hit
    
    */

    public void ReactToHit(Transform hitBone, Vector3 hitPoint, Vector3 hitNormal)
    {
        Debug.Log("ReactToHit on " + gameObject.name);


        //stop wandering behaviour
        WanderingAI behaviour = GetComponent<WanderingAI>();

        //SET DEATH
        if (behaviour != null)
        {
            behaviour.SetAlive(false);
            behaviour.enabled = false;
        }

        //trigger ragdoll
        RagdollOnDeath ragdoll = GetComponent<RagdollOnDeath>();
        if (ragdoll != null)
        {
            //direction eminating from center outward
            Vector3 hitDirection = -hitNormal;

            //apply death

            //bone point direction force
            ragdoll.DieFromHit(
                hitBone,
                hitPoint,
                hitDirection,
                3f            //with this force
            );
        }

        else
        {
            //if not a ragdoll entity
            StartCoroutine(OldDie());
            return;
        }


        //Optionally destroy the body after a delay
        StartCoroutine(DestroyAfterDelay(5f));
    }

    ///////////////
    // OLD DEATH //
    ///////////////
    private IEnumerator OldDie()
    {
        this.transform.Translate(0, 0, 0);
        this.transform.Rotate(-90, 0, 0);
        yield return new WaitForSeconds(1.5f);
        Destroy(this.gameObject);
    }

    private IEnumerator DestroyAfterDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }


    


}
