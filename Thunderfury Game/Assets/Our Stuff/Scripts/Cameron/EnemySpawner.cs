using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    [SerializeField] GameObject enemy;
    [SerializeField] int maxEnemies = 20;
    [SerializeField] int enemyCount;
    [SerializeField] float spawnTime = 0.75f;
    [SerializeField] Transform[] spawnPoints;
    static int enemies;

    // Use this for initialization
    void Start () {
        enemyCount = 0;
        //InvokeRepeating("Spawn", spawnTime, spawnTime);
        StartCoroutine(SpawnWave());
	}

    IEnumerator SpawnWave ()
    {
        yield return new WaitForSeconds(spawnTime);

        // Within a while loop
        while (enemyCount < maxEnemies)
        {
            pickSpawnLocation();
            yield return new WaitForSeconds(spawnTime);
        }
    }

	// Update is called once per frame
	void Update () {

        //       if(enemies >= maxEnemies)
        //       {
        //           CancelInvoke("Spawn");
        //       }
        //       else
        //       {
        //           int spawnPointIndex = Random.Range(0, spawnPoints.Length);

        //           Instantiate(enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
        //           enemies++;
        //       }
	}

    void pickSpawnLocation ()
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
            enemyCount++;
            return;
        }
        // Spawn enemy success, return
    }

}
