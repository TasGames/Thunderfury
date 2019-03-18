using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [System.Serializable]
    protected struct Enemies
    {
        public GameObject enemy;
        public float spawnWeight;
    }
    protected float totalWeight;

    Enemies enemy;

    [SerializeField] protected Enemies[] enemyList;   //Enemy Object

    Target enemyHealth; //To reset enemy health on spawn

    //[HideInInspector]
    public List<GameObject> activeSpawns = new List<GameObject>();

    public List<GameObject> activeEnemies = new List<GameObject>();

    // Use this for initialization
    void Start()
    {

        if (activeSpawns.Count == 0)
        {
            Debug.LogError("No Spawn Points");
        }
    }

    void OnValidate()
    {
        totalWeight = 0;
        if (enemyList != null)
        {
            foreach (var Enemy in enemyList)
                totalWeight += Enemy.spawnWeight;
        }
    }

    public void PickSpawnLocation()
    {
        if (activeSpawns.Count > 0)
        {
            float pick = Random.value * totalWeight;
            int chosenIndex = 0;
            float cumulativeWeight = enemyList[0].spawnWeight;

            while (pick > cumulativeWeight && chosenIndex < enemyList.Length - 1)
            {
                chosenIndex++;
                cumulativeWeight += enemyList[chosenIndex].spawnWeight;
            }

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
                if (enemyList[chosenIndex].enemy.tag == "Enemy1")   //If randomly selected enemy is Enemy1
                {
                    enemy.enemy = ObjectPooler.SharedInstance.GetPooledObject("Enemy1");
                }
                else if (enemyList[chosenIndex].enemy.tag == "Enemy2")  //If randomly selected enemy is Enemy2
                {
                    enemy.enemy = ObjectPooler.SharedInstance.GetPooledObject("Enemy2");
                }

                
                if (enemy.enemy != null)
                {
                    enemyHealth = enemy.enemy.GetComponent<Target>();
                    enemyHealth.health = enemyHealth.originalHealth;    //Reset health

                    enemy.enemy.transform.position = posToSpawn;
                    enemy.enemy.transform.rotation = activeSpawns[spawnPointIndex].transform.rotation;
                    activeEnemies.Add(enemy.enemy);
                    enemy.enemy.SetActive(true);
                }
                Debug.Log("Spawn Complete");
                return;
            }
            // Spawn enemy success, return
        }
    }

}
