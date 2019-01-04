using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public GameObject enemy;   //Enemy Object
    public Transform[] spawnPoints;

    // Use this for initialization
    void Start () {

        if (spawnPoints.Length == 0)
        {
            Debug.LogError("No Spawn Points");
        }
    }

    public void PickSpawnLocation ()
    {
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);

        // Generate a random position
        Vector3 posToSpawn = (Random.insideUnitSphere) + spawnPoints[spawnPointIndex].position;
        posToSpawn.y = spawnPoints[spawnPointIndex].position.y;

        // Check it with a Physics.OverlapSphere
        //Collider[] hitColliders = Physics.OverlapSphere(posToSpawn, 0.2f);
        Collider[] hitColliders = Physics.OverlapBox(posToSpawn, spawnPoints[spawnPointIndex].transform.localScale /2, Quaternion.identity);

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
            Instantiate(enemy, posToSpawn, spawnPoints[spawnPointIndex].rotation);
            return;
        }
        // Spawn enemy success, return
    }

}
