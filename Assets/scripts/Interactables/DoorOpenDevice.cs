using UnityEngine;

public class DoorOpenDevice : MonoBehaviour
{
    public Transform Hinge;

    public float openAngle = 80f;
    public float openSpeed = 80f;
    public bool hingeOnRight = true;

    private bool open;
    private float currentAngle;
    private float targetAngle;


    void Awake()
    {


        //get collider of door object
        var doorCollider = GetComponent<BoxCollider>();
        if (!doorCollider)
        {
            Debug.LogError("Door needs a BoxCollider", this);
            enabled = false;
            return;
        }

        //set hinge position to bottom (left or right) of door collider
        Vector3 size = doorCollider.size * 0.5f;
        Vector3 center = doorCollider.center;

        Vector3 hingePos =
            transform.TransformPoint
            (
                hingeOnRight ? (center.x + size.x) : (center.x - size.x),
                center.y - size.y,
                center.z
            );

        Hinge.position = hingePos;
        Hinge.rotation = transform.rotation;


    }

    public void Operate()
    {

        open = !open;

        if (open) targetAngle = openAngle;
        else targetAngle = 0f;
    }


    //update
    void Update()
    {
        float angle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, openSpeed*Time.deltaTime);
        float angleDiff = Mathf.DeltaAngle(currentAngle, angle);

        if (Mathf.Abs(angleDiff) > 0.0001f)
        {
            transform.RotateAround(Hinge.position, Hinge.up, angleDiff);
        }

        currentAngle= angle;
                
    }
}
