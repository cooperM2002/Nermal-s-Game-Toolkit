using UnityEngine;

public class DoorOpenDevice : MonoBehaviour
{

    //offset the pos by THIS when opening
    [SerializeField] Vector3 doorPos;


    private bool open;


    public void Operate()
    {
        if (open)
        {
            Vector3 pos = transform.position -doorPos;
            transform.position = pos;
        }
        else
        {
            Vector3 pos = transform.position + doorPos;
            transform.position = pos;
        }
        open = !open;
    }

    //start
    void Start()
    {
        
    }

    //update
    void Update()
    {
        
    }
}
