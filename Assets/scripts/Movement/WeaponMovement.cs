using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class WeaponSway : MonoBehaviour
{

    ///////////////////////
    ///     FIELDS      ///
    ///////////////////////

    //BOB
    [Header("Bob")]                 

    public float _bobSpeed=4f;          //speed
    public float _bobAmount=0.5f;       //amplitude
    public float _bobIdleAmount=10.0f;  //?????????


    //POS SWAY
    [Header("Position Sway")]

    public float _swayPosAmount = 0.5f;     //amount
    public float _swayPosSmoothAmount = 5.0f;//amount smooth

    
    //ROT SWAY
    [Header("Rotation Sway")]

    public float _swayRotAmount = 2f;       //amount
    public float _swayRotSmoothAmount = 6f; //amount smooth

    //SWAY BOUNDARIES
    [Header("Sway Min/Max")]

    public float[] swayPosBounderies = 
    {
        -0.25f,   // [0] -> left  
        0.5f,  // [1] -> right
        -0.5f,   // [2] -> top
        0.5f   // [3] -> bottom
    };

    //public float _swayPosMaxLeft = -0.5f;
    //public float _sway_pos_max_right = 0.5f;
    //public float _sway_pos_max_bottom = -1f;
    //public float _sway_pos_max_top = -1f;

    
    //TILT
    [Header("Weapon Tilt")]

    //fields, amounts
    public float _tilt_degrees_amount = 10f;    // max roll when holding A/D
    public float _tilt_smooth_time = 0.08f;     // lower = snappier, higher = floatier





    //////////////////////
    ///  base values   ///
    //////////////////////
    
    //SWAY transform vectors
    private Vector3 posStart;       //position: (x,y,z)
    private Quaternion rotStart;    //rotation axes: (x,y,z) by scalar (w) -> (x,y,z,w)

    //BOB base amounts
    private float bobAmountBase, bobSpeedBase;      //bob base
    private float bobSprintAmount, bobSprintSpeed;  //bob sprint

    //SWAY base amounts
    private float swayPosBase, swayRotBase;     //pos,rot base
    private float  swayPosSprint, swayRotSprint;//pos,rot sprint

    //TILT speed
    private float tiltSpeed;    //rate of tilt





    void Start()
    {

        ///////////////////////////
        ///                     ///
        ///     PLACEHOLDER     ///
        ///                     ///
        ///////////////////////////


         /*---------------------*
         *                      *
         *      PLACEHOLDER     *
         *                      *
         *---------------------*/

        //INITIAL TRANSFORMS
        posStart = transform.localPosition;
        rotStart = transform.localRotation;


        //BOB start amounts
        bobAmountBase = _bobAmount; //base bob amount
        bobSpeedBase = _bobSpeed;   //base bob speed
        bobSprintAmount = _bobAmount * 2;   //bob amount modified on sprint
        bobSprintSpeed = _bobSpeed * 2;     //bob speed modified on sprint


        //ROT,POS SWAY starter amounts
        swayPosBase = _swayPosAmount;   //starting position sway
        swayRotBase = _swayRotAmount;   //starting rotation sway
        swayPosSprint = _swayPosAmount * 2; //position sway modified on sprint 
        swayRotSprint = _swayRotAmount * 2; //rotation sway modified on sprint


    }

    //update
    void Update()
    {

        ///////////////////////
        ///                 ///
        ///     SPRINT      ///
        ///                 ///
        ///////////////////////


        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
        {

            //change TO modified values

            bobSprintAmount = _bobAmount;   //bob -> sprint bob
            _swayPosAmount = swayPosSprint; //pos sway -> sprint pos sway
            _swayRotAmount =swayRotSprint;  //rot sway -> sprint rot sway
            _bobSpeed = bobSprintSpeed;     //bob speed -> sprint bob speed

        } 
        else
        {

            //change BACK TO base valyes

            bobAmountBase  = _bobAmount;      //bob -> base bob
            _swayPosAmount = swayPosBase;   //pos sway -> base pos sway
            _swayRotAmount = swayRotBase;   //rot sway -> base rot sway
            _bobSpeed = bobSpeedBase;       //bob speed -> base bob speed
        }


        /////////////////////
        ///               ///
        ///     TILT      ///
        ///               ///
        /////////////////////


        //input changes direction
        float tilt_direction = 0f;                              //initial direction
        if (Input.GetKey(KeyCode.A)) tilt_direction = -1f;      //A is left direction
        else if (Input.GetKey(KeyCode.D)) tilt_direction = 1f;  //D is right direction


        //the z transform and its target
        Vector3 tilt_transform = transform.localEulerAngles;            //represent the transform

        float tilt_z_target = -tilt_direction * _tilt_degrees_amount;   //tilt by (degrees) amount in left or right direction


        float tilt_z = Mathf.SmoothDampAngle
            (
                tilt_transform.z,   //transform this
                tilt_z_target,      //to this value
                ref tiltSpeed,  //at this speed
                _tilt_smooth_time   //over this period of time
            );


        tilt_transform.z = tilt_z;
        transform.localEulerAngles = tilt_transform;



        /*for mouse
         *  -amount of mouse movement per frame horizontally / vertically, 
         *  -delta, small change each frame
         *  -mouse_horizontal => rotate camera,player left / right => yaw
         *  -mouse_vertical =>  rotate camera up / down => pitch
        */

        float mouse_horizontal = Input.GetAxis("Mouse X");  //rotation amount per frame left/right -> yaw
        float mouse_vertical = Input.GetAxis("Mouse Y");  //rotation amount per frame up/down -> pitch


        /* for keyboard keys
         *  -movement direction along X(side to side) / Z(front to back) 
         *  -axis state: (-1) = full left/back, (0)  = neutral, (1)  = full right/forward
         */

        float movement_horizontal = Input.GetAxis("Horizontal");
        float movement_vertical = Input.GetAxis("Vertical");


        ///////////////////////
        ///                 ///
        ///     POS SWAY    ///
        ///                 ///
        ///////////////////////

        /* movement sway offset */

        Vector3 movement_offset = new Vector3
            (-movement_horizontal, 0, -movement_vertical)*(_swayPosAmount/8);//sway mult



        /* position sway */

        //get sway according to mouse movement
        float sway_pos_mouse_horizontal = -mouse_horizontal * _swayPosAmount;   //horizontal position sway w/ mouse h
        float sway_pos_mouse_vertical = -mouse_vertical * _swayPosAmount;       //vertical position sway w/ mouse v

        //BOUNDARIES
        //[0] -> left  
        //[1] -> right
        //[2] -> top
        //[3] -> bottom
        
        Vector3 sway_pos_mouse_offset = 
            new Vector3(
                sway_pos_mouse_horizontal, 
                sway_pos_mouse_vertical, 
                0
            );

        //sway_pos_mouse_offset.x = 
        //    Mathf.Clamp(
        //        sway_pos_mouse_offset.x,
        //        swayPosBounderies[0],
        //        swayPosBounderies[1]
        //    );

        //sway_pos_mouse_offset.y = 
        //    Mathf.Clamp(
        //        sway_pos_mouse_offset.y, 
        //        _sway_pos_max_bottom, 
        //        _sway_pos_max_top
        //    );

        ///////////////////
        ///             ///
        ///     BOB     ///
        ///             ///
        ///////////////////

        float bob = Mathf.Sin(Time.time * _bobSpeed)*(_bobAmount);//bob
        sway_pos_mouse_offset.y += bob/_bobIdleAmount;

        //calculate target position with base position + calculated offset + movement offset
        Vector3 sway_pos_target = posStart + sway_pos_mouse_offset + movement_offset;


        sway_pos_target.x =
            Mathf.Clamp(
                sway_pos_target.x,
                swayPosBounderies[0],
                swayPosBounderies[1]
            );

        Vector3 sway_pos_smooth = Vector3.Lerp(transform.localPosition,sway_pos_target,Time.deltaTime* _swayPosSmoothAmount);


        transform.localPosition = sway_pos_smooth;


        ///////////////////////
        ///                 ///
        ///     ROT SWAY    ///
        ///                 ///
        ///////////////////////
        
        /* rotation sway*/
        float sway_rot_mouse_horizontal = -mouse_horizontal * _swayRotAmount;
        float sway_rot_mouse_vertical = -mouse_vertical * _swayRotAmount;

        Quaternion sway_rot_offset = Quaternion.Euler(sway_pos_mouse_horizontal, sway_pos_mouse_vertical, 0);
        Quaternion sway_rot_target = rotStart * sway_rot_offset;
        Quaternion sway_rot_smooth = Quaternion.Slerp(transform.localRotation,sway_rot_target,Time.deltaTime * _swayRotSmoothAmount);

        transform.localRotation = sway_rot_smooth;

    }
}
