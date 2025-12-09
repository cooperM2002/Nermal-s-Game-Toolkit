using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.WSA;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class WeaponSway : MonoBehaviour
{


    /////////////////////////
    ///   CLASS IMPORTS   ///
    /////////////////////////
    private FPSinput playerMotor;
    private MouseLook mouseLook;


    ///////////////////////
    ///     FIELDS      ///
    ///////////////////////

    //BOB
    [Header("Bob")]

    public float _bobSpeed = 4f;    //speed
    public float _bobAmount = 0.5f;   //amplitude
    public float _bobIdleAmount = 10.0f;  //?????????


    //POS SWAY
    [Header("Position Sway")]

    public float _swayPosAmount = 0.5f;         //amount
    public float _swayPosSmoothAmount = 5.0f;   //amount smooth


    //ROT SWAY
    [Header("Rotation Sway")]

    public float _swayRotAmount = 2f;       //amount
    public float _swayRotSmoothAmount = 6f; //amount smooth

    //SWAY BOUNDARIES
    [Header("Sway Min/Max")]

    public float[] swayPosBounderies =
    {
        -0.25f, // [0] -> left  
        0.5f,   // [1] -> right
        -0.5f,  // [2] -> top
        0.5f    // [3] -> bottom
    };


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
    private float bobCurrentAmount, bobCurrentSpeed;
    private float bobAmountBase, bobSpeedBase;      //bob base
    private float bobSprintAmount, bobSprintSpeed;  //bob sprint

    //SWAY base amounts
    private float swayPosBase, swayRotBase;     //pos,rot base
    private float swayPosSprint, swayRotSprint;//pos,rot sprint

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


        /*----INITIAL TRANSFORMS----*/

        posStart = transform.localPosition;
        rotStart = transform.localRotation;


        /*----BOB----*/

        //base vals
        bobAmountBase = _bobAmount; //base bob amount
        bobSpeedBase = _bobSpeed;   //base bob speed
        //sprint vals
        bobSprintAmount = _bobAmount * 2;   //bob amount modified on sprint
        bobSprintSpeed = _bobSpeed * 2;     //bob speed modified on sprint


        /*----ROT,POS SWAY----*/

        //base vals
        swayPosBase = _swayPosAmount;   //starting position sway
        swayRotBase = _swayRotAmount;   //starting rotation sway
        //sprint vals
        swayPosSprint = _swayPosAmount * 2; //position sway modified on sprint 
        swayRotSprint = _swayRotAmount * 2; //rotation sway modified on sprint


        /*----PLAYER MOTOR----*/

        //movement
        playerMotor = GetComponentInParent<FPSinput>();
        if (playerMotor == null){Debug.Log("WeaponSway: no player motor detected");}

        //mouselook
        if (!mouseLook){mouseLook = GetComponentInParent<MouseLook>();}

    }

    //update
    void Update()
    {



        //////////////////////
        ///   SET INPUTS   ///
        //////////////////////


        /*------------------ KEYBOARD/MOVEMENT -----------------------*/

        /*  -movement direction along X(side to side) / Z(front to back) 
         *  -axis state: 
         *      (-1) = full left/back, 
         *      (0)  = neutral, 
         *      (1)  = full right/forward 
         */

        float movement_horizontal,movement_vertical;
        bool isMoving, isSprinting = false; //isCrouching = false;
        

        if (playerMotor != null)
        {

            //set moving,sprint
            isMoving = playerMotor.IsMoving;        //MOVING
            isSprinting = playerMotor.isSprinting;  //SPRINTING

            movement_horizontal = playerMotor.MoveInput.x;  //x
            movement_vertical = playerMotor.MoveInput.y;    //y
        }
        else
        {
            //set moving,sprint
            isMoving = false;        //MOVING
            isSprinting = false;  //SPRINTING
            //DEFAULTS
            movement_horizontal = Input.GetAxis("Horizontal");  //defualt x
            movement_vertical = Input.GetAxis("Vertical");      //default y
        }


        /*----------------------- MOUSE ---------------------------*/

        /*  -amount of mouse movement per frame horizontally / vertically
         *  -mouse_horizontal => left / right => yaw
         *  -mouse_vertical =>  up / down => pitch */

        float mouse_horizontal, mouse_vertical;

        if (mouseLook != null)
        {
            //set mouse axes
            mouse_horizontal = mouseLook.MouseDelta.x;  //left/right yaw
            mouse_vertical = mouseLook.MouseDelta.y;    //up/down pitch
        }
        else
        {
            //DEFAULTS
            mouse_horizontal = Input.GetAxis("Horizontal"); //default x
            mouse_vertical = Input.GetAxis("Vertical");     //defualt y
        }


        /////////////////////////////////
        ///                           ///
        ///  SPRINT/CROUCH MODIFIERS  ///
        ///                           ///
        /////////////////////////////////

        if (isMoving)
        {
            if (isSprinting)
            {
                bobCurrentAmount = bobSprintAmount; //bob -> sprint bob
                bobCurrentSpeed = bobSprintSpeed;   //bob -> sprint bob
            }
            else
            {
                bobCurrentAmount = bobAmountBase;   //bob -> sprint bob
                bobCurrentSpeed = bobSpeedBase;     //bob -> sprint bob 
            }
        }
        else
        {
            bobCurrentAmount = _bobIdleAmount;
            bobCurrentSpeed = 0f;
        }


        //bool isSprinting = playerMotor.isSprinting;

        if (isSprinting)
        {

            //change TO modified values

            // bobSprintAmount = _bobAmount;   //bob -> sprint bob
            _swayPosAmount = swayPosSprint; //pos sway -> sprint pos sway
            _swayRotAmount = swayRotSprint;  //rot sway -> sprint rot sway

            // bobCurrentAmount = bobSprintAmount;   //bob -> sprint bob
            // bobCurrentSpeed = bobSprintSpeed;   //bob -> sprint bob



        }
        else
        {

            //change BACK TO base valUes

            //bobAmountBase = _bobAmount;      //bob -> base bob
            _swayPosAmount = swayPosBase;   //pos sway -> base pos sway
            _swayRotAmount = swayRotBase;   //rot sway -> base rot sway

            // bobCurrentAmount = bobAmountBase;   //bob -> sprint bob
            // bobCurrentSpeed = bobSpeedBase;   //bob -> sprint bob
        }


        ///////////////////////
        ///                 ///
        ///     POS SWAY    ///
        ///                 ///
        ///////////////////////

        /* movement sway offset */

        Vector3 movement_offset =
            new Vector3(-movement_horizontal, 0, -movement_vertical) * (_swayPosAmount / 8);//sway mult



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

        float bob = Mathf.Sin(Time.time * bobCurrentSpeed) * bobCurrentAmount;//bob
        sway_pos_mouse_offset.y += bob / _bobIdleAmount;

        //calculate target position with base position + calculated offset + movement offset
        Vector3 sway_pos_target = posStart + sway_pos_mouse_offset + movement_offset;


        sway_pos_target.x =
            Mathf.Clamp(
                sway_pos_target.x,
                swayPosBounderies[0],
                swayPosBounderies[1]
            );

        Vector3 sway_pos_smooth = Vector3.Lerp(transform.localPosition, sway_pos_target, Time.deltaTime * _swayPosSmoothAmount);


        transform.localPosition = sway_pos_smooth;


        ///////////////////////
        ///                 ///
        ///     ROT SWAY    ///
        ///                 ///
        ///////////////////////

        /* rotation sway*/
        float sway_rot_mouse_horizontal = -mouse_horizontal * _swayRotAmount;
        float sway_rot_mouse_vertical = -mouse_vertical * _swayRotAmount;

        Quaternion sway_rot_offset =
        Quaternion.Euler(
            sway_rot_mouse_horizontal,
            sway_rot_mouse_vertical,
            0
        );

        Quaternion sway_rot_target = rotStart * sway_rot_offset;

        Quaternion sway_rot_smooth =
        Quaternion.Slerp(
            transform.localRotation,
            sway_rot_target,
            Time.deltaTime * _swayRotSmoothAmount
        );

        transform.localRotation = sway_rot_smooth;

    }




}
