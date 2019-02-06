﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public GameObject enemy;   //Enemy Object
    //public Transform[] spawnPoints;

    //[HideInInspector]
    public List<GameObject> activeSpawns = new List<GameObject>();

    // Use this for initialization
    void Start()
    {

        if (activeSpawns.Count == 0)
        {
            Debug.LogError("No Spawn Points");
        }
    }

    public void PickSpawnLocation()
    {
        int spawnPointIndex = Random.Range(0, activeSpawns.Count);

        // Generate a random position
        //Vector3 posToSpawn = (Random.insideUnitSphere) + spawnPoints[spawnPointIndex].position;
        Vector3 posToSpawn = (Random.insideUnitSphere) + activeSpawns[spawnPointIndex].transform.position;
        posToSpawn.y = activeSpawns[spawnPointIndex].transform.position.y;

        // Check it with a Physics.OverlapSphere
        //Collider[] hitColliders = Physics.OverlapSphere(posToSpawn, 0.2f);
        Collider[] hitColliders = Physics.OverlapBox(posToSpawn, activeSpawns[spawnPointIndex].transform.localScale / 2, Quaternion.identity);

        // If it returns any colliders (hitColliders.Length > 0)
        if (hitColliders.Length > 0)
        {
            // Then you cannot spawn here
            // So generate a new position (i.e. next loop)
            Debug.Log("Failed to spawn: Collision");
            return;
        }
        else
        {
            GameObject enemy = ObjectPooler.SharedInstance.GetPooledObject("Enemy1");
            if (enemy != null)
            {
                enemy.transform.position = posToSpawn;
                enemy.transform.rotation = activeSpawns[spawnPointIndex].transform.rotation;
                enemy.SetActive(true);
            }
            return;
        }
        // Spawn enemy success, return
    }

}
