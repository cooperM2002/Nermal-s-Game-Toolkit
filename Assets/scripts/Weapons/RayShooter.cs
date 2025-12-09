/*


RayShooter
    -draws crosshair
    -on leftclicked shoots a ray through center of screen, then hits a point
    -ray

****DIRECTION OF ONE THING RELATIVE TO OTHER***************
Vector3 dir = (pos - transform.position).normalized;
normalized => length 1
pos => other thing  -   tranform.position => my position
************************************************************
*/




using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayShooter : MonoBehaviour
{

    /////////////////////
    ///   variables   ///
    /////////////////////
    private Camera cam;
    private float baseFov;
    private float zoomFov;

    //[Range(0, 1), SerializeField] public float zoomAmount;
    //private float zoomModifier;


    ///////////////////////
    ///   initialize    ///
    ///////////////////////
    void Start()
    {
        cam = GetComponent<Camera>();   //access camera component
        //baseFov = cam.fieldOfView;
        //zoomFov = baseFov * .75f;
    }



    ///////////////////////
    ///    CROSSHAIR    ///
    ///////////////////////
    private void OnGUI()
    {
        int size = 18;
        float posX = cam.pixelWidth / 2 - size / 4;
        float posY = cam.pixelHeight / 2 - size / 2;

        GUI.Label(new Rect(posX, posY, size, size), "�");
    }



    ///////////////////////
    ///  LEFT CLICKED   ///
    ///////////////////////
    void Update()
    {

        //left click
        bool leftPressed = Input.GetMouseButtonDown(0); //left mouse clicked 

        //center of the screen
        float centerX = cam.pixelWidth / 2;     //halfway on h
        float centerY = cam.pixelHeight / 2;    //halfway on v

        //OLD ZOOM IN LOGIC
        // bool rightPressed = Input.GetMouseButton(1);
        // if (rightPressed){cam.fieldOfView = zoomFov;}
        // else{cam.fieldOfView = baseFov;}


        if (leftPressed)
        {

            //vector center of screen
            Vector3 center =
            new Vector3(
                centerX,    //halfway on x
                centerY,    //halfway on y
                0           //0
            );

            //ray at center of screen
            Ray ray = cam.ScreenPointToRay(center); //from center to point
            RaycastHit hit;                         //information of the raycast



            //raycast fills a referenced variable with info
            if (Physics.Raycast(ray, out hit))
            {

                //object that was hit by ray
                ReactiveTarget target = hit.transform.GetComponentInParent<ReactiveTarget>();

                //GameObject hitObject = hit.transform.gameObject;
                //ReactiveTarget target = hitObject.GetComponent<ReactiveTarget>();


                if (target != null)
                {
                    //rag doll logic
                    target.ReactToHit(hit.transform,hit.point,hit.normal);

                    //Debug.Log("Target hit.");
                    Debug.Log("Target hit: " + hit.transform.name + " (root: " + target.gameObject.name + ")");
                }
                else
                {
                    //launch coroutine in response to a hit
                    StartCoroutine(ImpactIndicator(hit.point));
                }

            }//end of raycast
        }//end of leftpressed
    }//end of update




    //////////////////////////
    ///  impact indicator  ///
    //////////////////////////

    //public impact model
    [SerializeField] private GameObject indicator;



    //coroutine for triggering impact indicator
    private IEnumerator ImpactIndicator(Vector3 pos)
    {

        //face relative to players position
        Vector3 dir = (pos - transform.position).normalized;
        Quaternion rotatePos = Quaternion.LookRotation(dir);

        //use arrow prefab
        GameObject arrow =
        Instantiate(
            indicator,  //object
            pos,        //position
            rotatePos   //rotation
            );

        //pause at 1s
        yield return new WaitForSeconds(10);

        //destroy object
        Destroy(arrow);

    }



}
