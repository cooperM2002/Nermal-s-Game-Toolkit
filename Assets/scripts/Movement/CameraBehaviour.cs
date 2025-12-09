using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{

    /////////////////////////
    ///   CLASS IMPORTS   ///
    /////////////////////////
    private FPSinput playerMotor;


    public float tiltDegrees = 10f;        //max roll when holding A/D
    public float tiltSmoothTime = 0.08f;   //lower = snappier, higher = floatier
    private float zVelocity;               //required by SmoothDampAngle



    //start 
    void Start()
    {

        /*----PLAYER MOTOR----*/

        playerMotor = GetComponentInParent<FPSinput>();
        
        if (playerMotor == null)
        {
            Debug.Log("CameraBehaviour: no player motor detected");
        }

    }



    //update
    void Update()
    {

        Vector3 MoveDir = playerMotor.MoveInput;
        bool left, right;

        if (playerMotor != null)
        {
            left = MoveDir.x < 0;    //left in MoveDir 
            right = MoveDir.x > 0;   //right in MoveDir
        }
        else
        {
            left = Input.GetKey(KeyCode.A);     //left key press
            right = Input.GetKey(KeyCode.D);     //right key press
        }



        ////GET INPUT DIRECTION
        float input = 0f;

        if (left)
        {
            input = -1f;    //left 
        }   
        else if (right)
        { 
            input = 1f;     //right
        }


        ApplyTilt(input);
    }



    private void ApplyTilt(float inputDir)
    {
        float targetRotationZ = -inputDir * tiltDegrees;   //A = +tilt, D = -tilt

        Vector3 tiltBy = transform.localEulerAngles;

        float newZ =
        Mathf.SmoothDampAngle(
            tiltBy.z,
            targetRotationZ,
            ref zVelocity,
            tiltSmoothTime
        );

        tiltBy.z = newZ;

        transform.localEulerAngles = tiltBy;

    }
    




}
