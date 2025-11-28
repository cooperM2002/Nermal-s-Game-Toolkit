using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    //private float baseTilt;
    public float tiltDegrees = 10f;        // max roll when holding A/D
    public float tiltSmoothTime = 0.08f;   // lower = snappier, higher = floatier

    private float zVelocity;               // required by SmoothDampAngle







    //start 
    void Start()
    {
      
       // baseAngle = transform.localEulerAngles;
 




    } 

    //update
    void Update()
    {
        // Read input (A/D). If you prefer GetAxis, see note below.
        float input = 0f;
        if (Input.GetKey(KeyCode.A)) input = -1f;   // A = left
        else if (Input.GetKey(KeyCode.D)) input = 1f; // D = right

        // Match your original mapping: A => +tilt, D => -tilt
        float targetZ = -input * tiltDegrees;

        // Smooth only the Z, preserve current X/Y from other scripts
        Vector3 e = transform.localEulerAngles;
        float newZ = Mathf.SmoothDampAngle(e.z, targetZ, ref zVelocity, tiltSmoothTime);
        e.z = newZ;
        transform.localEulerAngles = e;
    }
}
