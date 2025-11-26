using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactiveTarget : MonoBehaviour
{
    //Start
    void Start()
    {
        
    }

    //Update
    void Update()
    {
        
    }


    //------------------------enemy react to hit
    //called by shooting script
    public void ReactToHit()
    {

        WanderingAI behaviour = GetComponent<WanderingAI>();
        if(behaviour != null)
        {
            behaviour.SetAlive(false);
        }

        StartCoroutine(Die());
    }


    //enemy death
    private IEnumerator Die()
    {
        this.transform.Translate(0, -0.5f, 0);

        this.transform.Rotate(-90, 0, 0);

        yield return new WaitForSeconds(1.5f);

        Destroy(this.gameObject);
    }
}
