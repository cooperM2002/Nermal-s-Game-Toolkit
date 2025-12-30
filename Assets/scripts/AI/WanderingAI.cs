
using UnityEngine;
public class WanderingAI : MonoBehaviour
{


    /////////////////////////////
    ///  AI attribute fields  ///
    /////////////////////////////
    
    //exportable
    public bool targetIsVisible { get; private set; }


    //public
    public float speed = 2.5f;
    public float maxDistance = 5.0f;
    public float viewRadius = 0.75f;
    [Range(0f, 360f)] public float angle = 45f;
    public bool visualize = true;
    public Transform visibilityCube;


    //private
    private bool isAlive;
    private Vector3 origin;
    private Vector3 forward;
    private Ray sightRay;
    private Renderer cubeRenderer;
    



    /////////////////////
    ///  Player data  ///
    /////////////////////

    private FPSinput player;



    //start
    void Start()
    {
        isAlive = true;

        /*-----PLAYER MOTOR-----*/
        player = FindFirstObjectByType<FPSinput>();
        if (player == null)
        {
            Debug.Log("CameraBehaviour: no player motor detected");
        }

        /*-----VISIBILITY CUBE-----*/
        // if (visibilityCube != null)
        // {
            cubeRenderer = visibilityCube.GetComponent<Renderer>();
        // }
        // else
        // {
        //     Debug.LogWarning("visibilityCube not assigned on " + name);
        // }
    }



    //setter for alive state
    public void SetAlive(bool alive)
    {
        isAlive = alive;
    }




    //Update
    void Update()
    {

        //PLAYER  (=>TARGET)
        Vector3 playerPosition = player.myPosition; //player origin

        //ENTITY
        origin = transform.position;    //this objects position
        forward = transform.forward;    //this objects forward position
        forward.y = 0f;                 //ignore forward y

        //vector from entity -> player
        Vector3 toPlayer = playerPosition - origin; //
        toPlayer.y = 0f;                            //ignore up/down
        float angleToPlayer = 0f;                   //


        //entity not on playerPosition
        bool notOnPos = toPlayer.sqrMagnitude < 0.0001f;
        if (notOnPos)
        {
            angleToPlayer = Vector3.SignedAngle(forward, toPlayer, Vector3.up);
        }

        //////////////////////
        ///  VISUALIZE FOV ///
        //////////////////////

        if (visualize)
        {
            if (!visualize || cubeRenderer == null) return;

            // true -> red, false -> green
            var colour = targetIsVisible ? Color.red : Color.green;
            cubeRenderer.material.color = colour;
        }


        //IF ALIVE
        if (isAlive)
        {

            ////////////////////
            /// MOVING LOGIC ///
            ////////////////////

            //move forward regardless of turning
            transform.Translate(0, 0, speed * Time.deltaTime);
        }

        //ray in enimating forward
        sightRay = new Ray(transform.position, transform.forward);

        ////////////////////////
        ///  OBSTACLE LOGIC  ///
        ////////////////////////

        detectObstacle(sightRay);

    }//end of update


    public bool CheckVisibilityAtPoint(Vector3 worldPoint)
    {
        //vector from this pos to a point in world
        Vector3 toTarget = worldPoint - transform.position;
        var degreesToTarget = Vector3.Angle(transform.forward, toTarget);
        bool inView = degreesToTarget < (angle / 2);

        //EXIT EARLY IN NOT IN VIEW
        if (!inView)
        {
            return false;
        }

        //distance to target
        float distanceToTarget = toTarget.magnitude;

        //ray max length length
        float rayDistance = Mathf.Min(maxDistance, distanceToTarget);
        //view ray and its info
        Ray ray = new Ray(transform.position, toTarget);
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            if (hit.collider.transform == player.transform)
            {
                return true;
            }
            return false;
        }
        else
        {
            return true;
        }

    }


    public bool CheckVisibility()
    {
        //vector from this pos to a point in world
        Vector3 toTarget = player.myPosition - transform.position;
        float degreesToTarget = Vector3.Angle(transform.forward, toTarget);
        bool inView = degreesToTarget < (angle / 2);

        //EXIT EARLY IN NOT IN VIEW
        if (!inView)
        {
            return false;
        }

        //distance to target
        float distanceToTarget = toTarget.magnitude;

        //ray max length length
        float rayDistance = Mathf.Min(maxDistance, distanceToTarget);
        //view ray and its info
        Ray ray = new Ray(transform.position, toTarget);
        RaycastHit hit;

        bool canSee = false;

        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            if (hit.collider.transform == player.transform)
            {
                canSee = true;
            }

            // Visualize the ray.
            Debug.DrawRay(transform.position, hit.point);
        }
        else
        {
            // Visualize the ray.
            Debug.DrawRay(transform.position,toTarget.normalized * rayDistance);
        }
        return canSee;


    }


    void detectObstacle(Ray sight)
    {

        ////////////////////////
        ///  OBSTACLE LOGIC  ///
        ////////////////////////
        RaycastHit hitInfo;

        //perform raycast with circular volume around ray
        if (Physics.SphereCast(sight, viewRadius, out hitInfo))
        {
            if (hitInfo.distance < maxDistance)
            {
                //////////////////////////////////////
                /// IMPLEMENT OTHER OBSTACLES HERE ///
                //////////////////////////////////////

                //TODO: ...

                //on the players collider
                if (hitInfo.collider.CompareTag("Player"))
                {
                    //attack etc
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
    }//END OF OBSTACLE


    void OnDrawGizmos()   // or OnDrawGizmosSelected if you only want it when selected
    {
        //render only on main camera
        if (!Camera.current.CompareTag("MainCamera")) return;
        Ray ray = new Ray(transform.position, transform.forward);
        float radius = viewRadius;
        float range = maxDistance;
        //origin for line
        Vector3 start = new Vector3(
            ray.origin.x,
            ray.origin.y + 2f,
            ray.origin.z
            );
        //direction of line
        Vector3 dir = ray.direction.normalized;
        //endpoint of line
        Vector3 end = start + dir * range;
        Gizmos.color = Color.gray;
        Gizmos.DrawLine(start, end);
        Gizmos.DrawSphere(end, radius);
    }//END OF GIZMO





}
