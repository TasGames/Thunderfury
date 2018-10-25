using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    [SerializeField] GameObject enemy;
    [SerializeField] int maxEnemies = 20;
    [SerializeField] float spawnTime = 3.0f;
    [SerializeField] Transform[] spawnPoints;
    static int enemies;
    

    // Use this for initialization
    void Start () {
        enemies = 0;
        InvokeRepeating("Spawn", spawnTime, spawnTime);
	}
	
	// Update is called once per frame
	void Update () {

        if(enemies >= maxEnemies)
        {
            CancelInvoke("Spawn");
        }
        else
        {
            int spawnPointIndex = Random.Range(0, spawnPoints.Length);

            Instantiate(enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
            enemies++;
        }
	}
}
