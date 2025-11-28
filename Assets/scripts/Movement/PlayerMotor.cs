using UnityEngine;


[RequireComponent(typeof(CharacterController))]
[AddComponentMenu("Control Script/FPS Input")]
public class FPSinput : MonoBehaviour
{

    [Header("Collisions")]
    public float PUSHFORCE = 1.0f;

    // PUBLIC STATE
    public bool isGrounded { get; private set; }
    public bool notJumping { get; private set; }

    //world space velocity
    private Vector3 velocity;
    public Vector2 MoveInput { get; private set; }
 

    [Header("Ground / Air Speeds")]

    public float _maxVelocityGround = 6.0f;  // MAX_VELOCITY_GROUND
    public float _maxVelocityAir = 0.6f;     // MAX_VELOCITY_AIR  

    [Header("Acceleration / Friction")]

    public float _maxAcceleration = 60.0f;  // 10 * maxVelocityGround (tweak)
    public float _stopSpeed = 1.5f;         // STOP_SPEED
    public float _friction = 4.0f;          // the 4 multiplier in drop 

    [Header("Jump / Gravity")]

    public float _jumpHeight = 0.85f;           // the 0.85 in your GDS JUMP_IMPULSE
    public float _gravity = (3f)*15.34f;  // GRAVITY (positive; we'll subtract)

    [Header("Crouch")]

    public float _crouchMultiplier = 0.5f;       // for height; crouchHeight

    // components
    private CharacterController charController;
    private float standHeight;
    private float crouchHeight;

    // internal
    private bool wishJump;

    void Start()
    {
        charController = GetComponent<CharacterController>();

        standHeight = charController.height;
        crouchHeight = standHeight * _crouchMultiplier;

        // optional: tie maxAcceleration to ground speed like in GDS:
        _maxAcceleration = 10f * _maxVelocityGround;
    }

    void Update()
    {
        float dt = Time.deltaTime;

        /******************
         * GROUNDED STATE *
         ******************/
         
        isGrounded = charController.isGrounded;
        notJumping = (isGrounded && velocity.y <= 0f);

        /**************
         *   CROUCH   *
         **************/

        if (Input.GetKey(KeyCode.LeftControl))
        {
            charController.height = crouchHeight;
        }
        else
        {
            charController.height = standHeight;
        }

        /****************
         * GROUND STATE *
         ****************/

        //reset each frame

        Vector3 moveDirWorld = Vector3.zero;    //reset world move direction each frame
        Vector2 inputDir = Vector2.zero;        //reset input direction each frame


        /*************
         *   INPUT   *
         *************/

        if (Input.GetKey(KeyCode.W))        
        {
            moveDirWorld += transform.forward;
            inputDir.y += 1f;                       //FOWARD (+y)
        }
        //PLAYER BACKWARD                               
        if (Input.GetKey(KeyCode.S))
        {
            moveDirWorld -= transform.forward;      
            inputDir.y -= 1f;                       //BACKWARD (-y)
        }
        //PLAYER LEFT
        if (Input.GetKey(KeyCode.A))
        {
            moveDirWorld -= transform.right;
            inputDir.x -= 1f;                       //LEFT (-x)
        }
        //PLAYER RIGHT
        if (Input.GetKey(KeyCode.D))
        {
            moveDirWorld += transform.right;
            inputDir.x += 1f;                       //RIGHT (+x)
        }

        //Store 2d input for other systems
        //MoveInput = inputDir.sqrMagnitude > 1f ? inputDir.normalized : inputDir;
        if (inputDir.sqrMagnitude > 1f) 
        { 
            MoveInput = inputDir.normalized; 
        }
        else 
        { 
            MoveInput = inputDir;  
        }

        //normalize moveDirWorld
        Vector3 wishDir = moveDirWorld.normalized;

        /*************
         *   JUMP    *
         *************/

        //desired direction
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            wishJump = true;
        }
        else
        {
            if (!isGrounded) wishJump = false;   //omit jump input while in air

        }

        /////////////////////////////////
        // CAMERA BOB / VIEW TILT HOOK //
        /////////////////////////////////

        /*************
         *  GROUND   *
         *************/

        if (isGrounded) //GROUNDED
        {
            if (wishJump)   //PRESSED SPACE   
            {
                //JUMP FORUMLA
                velocity.y = Mathf.Sqrt(2f * _gravity * _jumpHeight);

                //horizontal update uses AIR behaviour
                velocity = UpdateVelocityAir(wishDir, dt);
                wishJump = false;
            }
            else
            {
                velocity = UpdateVelocityGround(wishDir, dt);
            }
        }
        else
        {
            //apply gravity (downwards)
            velocity.y -= _gravity * dt;

            //air horizontal acceleration
            velocity = UpdateVelocityAir(wishDir, dt);
        }

        //move the CharacterController using velocity
        charController.Move(velocity * dt);
    }



    //////////////////////
    //                  //
    //    ACCELERATE    //
    //                  //
    //////////////////////

    //accelerate in wish direction up to max velocity, limited by maxAcceleration * delta
    private Vector3 Accelerate(Vector3 wishDir, float maxVelocity, float delta)
    {
        //NO INPUT
        if (wishDir.sqrMagnitude < 1e-6f)
            return velocity;

        //SPEED ALONG WISH DIR
        float currentSpeed = Vector3.Dot(velocity, wishDir);

        //how much added speed along wish direction
        float addSpeed = Mathf.Clamp(
            maxVelocity - currentSpeed, //clamped
            0f,                         //MIN
            _maxAcceleration * delta    //MAX
        );

        //new velocity is old + extra along wishDir
        return velocity + addSpeed * wishDir;
    }



    //////////////////////
    //                  //
    //    GROUND VEL    //
    //                  //
    //////////////////////

    // Ground movement: friction + accelerate toward wishDir at ground max velocity
    private Vector3 UpdateVelocityGround(Vector3 wishDir, float delta)
    {
        // Friction
        float speed = velocity.magnitude;
        if (speed > 0f)
        {
            float control = Mathf.Max(_stopSpeed, speed);
            // drop = control * 4 * delta  (the "4" is friction strength)
            float drop = control * _friction * delta;

            float newSpeed = Mathf.Max(speed - drop, 0f);
            if (newSpeed != speed)
            {
                velocity *= newSpeed / speed;
            }
        }

        // Then accelerate along wish direction up to ground max
        return Accelerate(wishDir, _maxVelocityGround, delta);
    }



    //////////////////////
    //                  //
    //      AIR VEL     //
    //                  //
    //////////////////////

    // Air movement: same accelerate, but using air max velocity
    private Vector3 UpdateVelocityAir(Vector3 wishDir, float delta)
    {
        return Accelerate(wishDir, _maxVelocityAir, delta);
    }

    /*
    (1) OnControllerColliderHit => (unity callback)

            -triggers when (this) CharacterController hits something
    
    (2) ControllerColliderHit hit => (object)

            -tells you what you hit
                -hit.gameObject
                -hit.collider

            -surface normal
                -hit.normal

            -direction controller was moving when it hit
                -hit.moveDirection

            -any attatched rigidbody

        *** runs once per collision -> decides whether and how to push the thing bumped into ***
     
     */


    void OnControllerColliderHit(ControllerColliderHit hit)
    {
     
        //BODY HIT
        var bodyHit = hit.rigidbody;    //bodyHit has been hit by a charcontroller

        //edge cases
        if (bodyHit == null || bodyHit.isKinematic) return;

        //if at certain angle, no force
        if (hit.normal.y > 0.5f) return;

        //direction of what charcontroller hit
        Vector3 horizontalDir = 
            new Vector3(
                hit.moveDirection.x,    //the body's side to side
                0f,                     //up down
                hit.moveDirection.z     //left right
            );

        //if force is small enough
        if (horizontalDir.sqrMagnitude < 1e-6f) return;

        //normalize
        horizontalDir.Normalize();

        //speed at which player is moving
        float playerSpeed = charController.velocity.magnitude;

        //direction @ speed pushed push = (direction to be push *PUSHFORCE *speed pushed @
        Vector3 push = horizontalDir * PUSHFORCE * Mathf.Max(0.2f,playerSpeed);

        //the body that was hit's new velocity
        bodyHit.linearVelocity = 
            new Vector3(
                push.x,                     //push side to side
                bodyHit.linearVelocity.y,   //push up down
                push.z);                    //push left right

        //DEBUG
        Debug.Log($"HIT: {hit.gameObject.name} | isTrig={hit.collider.isTrigger} | hasRB={hit.rigidbody != null}");


    }



}
