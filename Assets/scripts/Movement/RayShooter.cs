using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayShooter : MonoBehaviour
{

    //camera object reference
    private Camera cam;
    private float baseFov;
    private float zoomFov;

    //[Range(0, 1), SerializeField] public float zoomAmount;
    //private float zoomModifier;





    void Start()
    {

        //access other components attached to same object
        cam = GetComponent<Camera>();

        //baseFov = cam.fieldOfView;
        //zoomFov = baseFov * .75f;
        

    }





    //--------------------crosshair
    private void OnGUI()
    {
        int size = 18;
        float posX = cam.pixelWidth / 2 - size / 4;
        float posY = cam.pixelHeight / 2 - size / 2;

        GUI.Label(new Rect(posX, posY, size, size), "•");
    }








    //-------------------------update
    void Update()
    {
        //bool rightPressed = Input.GetMouseButton(1);

        //if (rightPressed)
        //{
        //    cam.fieldOfView = zoomFov;
        //}
        //else
        //{
        //    cam.fieldOfView = baseFov;
        //}

        // tf is left mouse clicked 
        bool leftPressed = Input.GetMouseButtonDown(0);

        //center of the screen
        float centerX = cam.pixelWidth / 2;
        float centerY = cam.pixelHeight / 2;


        if (leftPressed)
        {

            

            //new vector point eminating from center of screen
            //center of screen is (x,y,z): x=w/2,y=h/2,z=0
            Vector3 center = new Vector3(centerX, centerY, 0);

            //create the ray at this center point
            Ray ray = cam.ScreenPointToRay(center);
            RaycastHit hit;


            //raycast fills  a referenced variable with info
            if (Physics.Raycast(ray, out hit))
            {

                //object that was hit by ray
                GameObject hitObject = hit.transform.gameObject;
                ReactiveTarget target = hitObject.GetComponent<ReactiveTarget>();

                if (target != null) 
                {
                    target.ReactToHit();
                    Debug.Log("Target hit.");

                }
                else 
                {

                   

                    //launch coroutine in response to a hit
                    StartCoroutine(ImpactIndicator(hit.point));

                }

                    

            }

        }

    }


    //-------------------------------impact indicator
    [SerializeField] private GameObject arrowIndicator;
    //coroutine for removing ray indicator after 1s
    private IEnumerator ImpactIndicator(Vector3 pos)
    {

        //face the arrow normalized to players rotation
        Vector3 dir = (pos - transform.position).normalized;
        Quaternion rotatePos = Quaternion.LookRotation(dir);

        //use arrow prefab
        GameObject arrow = Instantiate(arrowIndicator, pos, rotatePos);

        //pause at 1s
        yield return new WaitForSeconds(10);

        //destroy object
        Destroy(arrow);

    }





}
