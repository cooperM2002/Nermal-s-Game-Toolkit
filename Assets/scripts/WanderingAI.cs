using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class WanderingAI : MonoBehaviour
{
    //speed and distance at which to react to targets
    public float speed = 2.5f;
    public float obstacleRange = 5.0f;

    private bool isAlive;


    //start
    void Start()
    {
        isAlive = true;
    }





    //Update
    void Update()
    {

        //check for alive
        if (isAlive)
        {
            //move forward regardless of turning
            transform.Translate(0, 0, speed * Time.deltaTime);
        }

        

        //ray in same pos and point in same dir as character
        Ray ray= new Ray(transform.position,transform.forward);
        RaycastHit hit;

        //perform raycasr with curcular volume around ray
        if(Physics.SphereCast(ray, 0.75f, out hit))
        {
            if (hit.distance < obstacleRange) 
            { 
                //turn toward a new direction if within obstacle range
                float angle = Random.Range(-110, 110);
                transform.Rotate(0, angle, 0);
            }

        }
    }


    //setter for alive state
    public void SetAlive(bool alive)
    {
        isAlive=alive;
    }


}
