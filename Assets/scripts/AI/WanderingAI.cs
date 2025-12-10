using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class WanderingAI : MonoBehaviour
{


    /////////////////////////////
    ///  AI attribute fields  ///
    /////////////////////////////
    
    //public
    public float speed = 2.5f;
    public float obstacleRange = 5.0f;
    public float viewRadius = 0.75f;

    //private
    private bool isAlive;
    private Vector3 origin;
    private Vector3 forward;
    private Ray sightRay;


    /////////////////////
    ///  Player data  ///
    /////////////////////

    private FPSinput player;
    


    //start
    void Start()
    {
        isAlive = true;

        /*--------------PLAYER MOTOR--------------*/
        player = FindFirstObjectByType<FPSinput>();
        if (player == null)
        {
            Debug.Log("CameraBehaviour: no player motor detected");
        }
        /*-----------------------------------------*/
    }





    //Update
    void Update()
    {

        origin = transform.position;
        forward = transform.forward;

        Vector3 playerPosition = player.myPosition;
        Vector3 toPlayer = playerPosition - origin;

        toPlayer.y = 0f;    //ignore up/down
        forward.y = 0f;     //ignore forward y

        float angleToPlayer=0f;
        bool notOnPos = toPlayer.sqrMagnitude < 0.0001f;
        if (notOnPos)
        {
            angleToPlayer = Vector3.SignedAngle(forward, toPlayer, Vector3.up);
        }



        //check for alive
        if (isAlive)
        {
            ////////////////////
            /// MOVING LOGIC ///
            ////////////////////



            //move forward regardless of turning
            transform.Translate(0, 0, speed * Time.deltaTime);
        }




        //ray in enimating forward from character origin
        // sightRay= new Ray(origin,forward);
        sightRay= new Ray(transform.position,transform.forward);
        RaycastHit hitInfo;



        ////////////////////////
        ///  OBSTACLE LOGIC  ///
        ////////////////////////
        
        //perform raycast with circular volume around ray
        if (Physics.SphereCast(sightRay, viewRadius, out hitInfo))
        {
            if (hitInfo.distance < obstacleRange)
            {

                //////////////////////////////////////
                /// IMPLEMENT OTHER OBSTACLES HERE ///
                //////////////////////////////////////

                //TODO: ...



                //on the players collider
                if (hitInfo.collider.CompareTag("Player"))
                {

                }

                //default obstacle avoidance behaviour
                else
                {

                    //turn toward a new direction if within obstacle range
                    float angle = Random.Range(-110, 110);
                    transform.Rotate(0, angle, 0);
                
                }
            }
        }
        

    }


    //setter for alive state
    public void SetAlive(bool alive)
    {
        isAlive = alive;
    }

    void OnDrawGizmos()   // or OnDrawGizmosSelected if you only want it when selected
    {

        // Vector3 start =
        //     new Vector3(
        //         transform.position.x,
        //         transform.position.y + 2,
        //         transform.position.z
        //         );
                
        // Vector3 dir   = transform.forward.normalized;
        // float radius  = viewRadius;
        // float range   = obstacleRange;

        // Vector3 end = start + dir * range;

        // Gizmos.color = Color.yellow;
        // Gizmos.DrawLine(start, end);
        // //Gizmos.DrawWireSphere(start, radius);
        // Gizmos.DrawWireSphere(end, radius);
        // Only draw for the main world camera
        if (!Camera.current.CompareTag("MainCamera")) return;
        Ray ray= new Ray(transform.position,transform.forward);

        float radius  = viewRadius;
        float range = obstacleRange;

        Vector3 start = new Vector3(
            ray.origin.x,
            ray.origin.y+2f,
            ray.origin.z
            );
            
        Vector3 dir   = ray.direction.normalized;
        Vector3 end = start + dir * range;

        Gizmos.color = Color.gray;
        Gizmos.DrawLine(start, end);
        //Gizmos.DrawWireSphere(start, radius);
        Gizmos.DrawSphere(end, radius);



        // drawViewShape(sightRay);
    }


    // void drawViewShape(Ray sight)
    // {
 
    // }
    
    


}
