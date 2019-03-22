using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [System.Serializable]
    protected struct EnemieTypes
    {
        //public GameObject enemy;
        public string typeName;
        public float typeHealth;
        public float typeDamage;
        public float typeSpeed;
        public float spawnWeight;
    }
    protected float totalWeight;

    [System.Serializable]
    protected enum Type
    {
        Type1,
        Type2,
        Type3
    }

    Type currentType;

    EnemieTypes enemyType;

    [SerializeField] protected EnemieTypes[] enemyList;   //Enemy Object

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
            currentType = (Type)chosenIndex;    //Selected enemy type is the randomly chosen index

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
                enemyHealth = enemy.GetComponent<Target>();
                
                //Set values based on enemy type
                switch (currentType)    
                {
                    case Type.Type1:
                        enemyHealth.health = enemyList[0].typeHealth;
                        enemy.GetComponent<EnemyBehaviour>().damageToDeal = enemyList[0].typeDamage;
                        enemy.GetComponent<UnityEngine.AI.NavMeshAgent>().speed = enemyList[0].typeSpeed;
                        break;

                    case Type.Type2:
                        enemyHealth.health = enemyList[1].typeHealth;
                        enemy.GetComponent<EnemyBehaviour>().damageToDeal = enemyList[1].typeDamage;
                        enemy.GetComponent<UnityEngine.AI.NavMeshAgent>().speed = enemyList[1].typeSpeed;
                        break;

                    case Type.Type3:
                        enemyHealth.health = enemyList[2].typeHealth;
                        enemy.GetComponent<EnemyBehaviour>().damageToDeal = enemyList[2].typeDamage;
                        enemy.GetComponent<UnityEngine.AI.NavMeshAgent>().speed = enemyList[2].typeSpeed;
                        break;
                }

                if (enemy != null)
                {
                    enemyHealth.health = enemyHealth.originalHealth;    //Reset health

                    enemy.transform.position = posToSpawn;
                    enemy.transform.rotation = activeSpawns[spawnPointIndex].transform.rotation;
                    activeEnemies.Add(enemy);
                    enemy.SetActive(true);
                }
                Debug.Log("Spawn Complete");
                return;
            }
            // Spawn enemy success, return
        }
    }

}
