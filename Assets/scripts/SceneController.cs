using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject spawnLocation;

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

            //spawn location
            enemy.transform.position = spawnLocation.transform.position;//new Vector3(-1, .2f, 0);

            // //spawn rotation
            // float angle = Random.Range(0, 360);
            // enemy.transform.Rotate(0, angle, 0);
        }

    }
}
