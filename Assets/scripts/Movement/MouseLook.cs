using UnityEngine;

public class MouseLook : MonoBehaviour
{

    public enum RotationAxes
    {
        MouseXandY = 0,
        MouseX = 1,
        MouseY = 2,
    }


    //camera object reference
    private Camera cam;
    private float baseFov;
    private float zoomFov;

    //
    public RotationAxes axes = RotationAxes.MouseXandY;

    //x speed
    public float defaultSensitivity=5.0f;
    private float sensitivityX;
    private float sensitivityY;
    private float zoomSensitivity;
    


    //clamped vertical view bounds
    public float minY = -45.0f;
    public float maxY = 45.0f;

    //vertical angle
    public float verticalRot = 0;
    //horizontal angle
    public float horizontalRot = 0;





    void Start()
    {
        Rigidbody body = GetComponent<Rigidbody>();

        sensitivityX=defaultSensitivity;
        sensitivityY = defaultSensitivity;
        zoomSensitivity = defaultSensitivity * 0.75f;


        //check if component exists

        if (body != null) { 
            body.freezeRotation = true;
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        cam = GetComponentInChildren<Camera>();

        baseFov = cam.fieldOfView;
        zoomFov = baseFov * .75f;
    }

    void Update()
    {



        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");


        if (axes == RotationAxes.MouseX){

            transform.Rotate(0, mouseX * sensitivityX, 0);
        }
        else if (axes == RotationAxes.MouseY){

            //increment vertical angle on mouse
            verticalRot -= mouseY * sensitivityY;

            //clamp angle at +- 45deg on y axis
            verticalRot = Mathf.Clamp(verticalRot, minY, maxY);

            //keep same Y angle, no horizontal rotation
            horizontalRot = transform.localEulerAngles.y;

            //new vector containing rotation values
            transform.localEulerAngles = new Vector3(verticalRot, horizontalRot, 0);


            ///(old) transform.Rotate(mouseY * sensitivityY, 0, 0);
        }
        else{
            //both x and y
            ///(old)transform.Rotate(mouseY*sensitivityY, mouseX*sensitivityX, 0);

            verticalRot -= mouseY * sensitivityY;
            verticalRot = Mathf.Clamp(verticalRot, minY, maxY);

            //amount to change rotation by
            float delta = mouseX * sensitivityX;
            //increment rotation angle by delta
            horizontalRot = transform.localEulerAngles.y+delta;

            transform.localEulerAngles = new Vector3(verticalRot, horizontalRot, 0);


        }
        bool rightPressed = Input.GetMouseButton(1);

        if (rightPressed)
        {
            cam.fieldOfView = zoomFov;
            sensitivityX = zoomSensitivity;
            sensitivityY = zoomSensitivity;



        }
        else
        {
            cam.fieldOfView = baseFov;
            sensitivityX = defaultSensitivity;
            sensitivityY = defaultSensitivity;

        }



    }
}
