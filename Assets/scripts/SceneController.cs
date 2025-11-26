using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    private GameObject enemy;



    //Start
    void Start()
    {
        
    }




    //Update 
    void Update()
    {
        //spawn iff no enemy is present
        if (enemy == null)
        { 

            //copy prefab
            enemy = Instantiate(enemyPrefab) as GameObject;


            enemy.transform.position = new Vector3(0, 1, 0);
            float angle = Random.Range(0, 360);
            enemy.transform.Rotate(0, angle, 0);
        }

    }
}
