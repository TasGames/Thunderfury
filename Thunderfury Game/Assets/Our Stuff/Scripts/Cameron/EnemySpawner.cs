using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    //MODULAR PIECES//
    //Basic
    Transform legsBasic;
    bool legsBasicActive;
    Transform bellyBasic;
    bool bellyBasicActive;
    //Speedy
    Transform legsSpeedy;
    bool legsSpeedyActive;
    //Heavy
    Transform chestHeavy;
    bool chestHeavyActive;
    Transform crotchHeavy;
    bool crotchHeavyActive;
    Transform leftHipHeavy;
    bool leftHipHeavyActive;
    Transform rightHipHeavy;
    bool rightHipHeavyActive;
    //Explodey
    Transform bellyExplodey;
    bool bellyExplodeyActive;
    //MODULAR PIECES//

    [System.Serializable]
    protected struct EnemyTypes
    {
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
        Basic,
        Speedy,
        Heavy,
        QuickHeavy,
        Explodey
    }
    Type currentType;

    EnemyTypes enemyType;

    [SerializeField] protected EnemyTypes[] enemyList;   //Enemy Object

    Target enemyHealth; //To reset enemy health on spawn

    //[HideInInspector]
    public List<GameObject> activeSpawns = new List<GameObject>();

    public List<GameObject> activeEnemies = new List<GameObject>();

    //INITIAL VALUES
    Vector3 enVelocity;
    Vector3 enAngVelocity;



    // Use this for initialization
    void Start()
    {
        if (activeSpawns.Count == 0)
        {
            Debug.LogError("No Spawn Points");
        }

        totalWeight = 0;
        if (enemyList != null)
        {
            foreach (var Enemy in enemyList)
                totalWeight += Enemy.spawnWeight;
        }

        //Ragdoll initial values
        // GameObject enemyRagdoll = ObjectPooler.SharedInstance.GetPooledObject("Enemy1");
        // enemyRagdoll.GetComponent<ResetRagdoll>().GetChildPositions();

    }

    public void PickSpawnLocation()
    {
        GameObject enemy = ObjectPooler.SharedInstance.GetPooledObject("Enemy1");
        enemyHealth = enemy.GetComponent<Target>();

        //Reset modular pieces
        legsBasicActive = false;
        legsSpeedyActive = false;
        crotchHeavyActive = false;
        leftHipHeavyActive = false;
        rightHipHeavyActive = false;
        chestHeavyActive = false;
        bellyBasicActive = false;
        bellyExplodeyActive = false;

        legsBasic = enemy.transform.Find("Legs_Basic_Skinned");
        legsSpeedy = enemy.transform.Find("Legs_speedy_Skinned");

        crotchHeavy = enemy.transform.Find("Armature");
        crotchHeavy = crotchHeavy.transform.Find("Hips");

        leftHipHeavy = crotchHeavy;
        rightHipHeavy = crotchHeavy;
        chestHeavy = crotchHeavy;

        crotchHeavy = crotchHeavy.transform.Find("HeavyCrotch_Hips");
        leftHipHeavy = leftHipHeavy.transform.Find("HeavySide_Left_Hips");
        rightHipHeavy = rightHipHeavy.transform.Find("HeavySide_Right_Hips");

        chestHeavy = chestHeavy.transform.Find("Spine");
        chestHeavy = chestHeavy.transform.Find("Chest");
        chestHeavy = chestHeavy.transform.Find("HeavyChest");

        bellyBasic = enemy.transform.Find("Belly_Basic_Skinned");
        bellyExplodey = enemy.transform.Find("Belly_Explodey_Skinned");

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
            currentType = (Type)chosenIndex;


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


                switch (currentType)
                {
                    case Type.Basic:    //If basic enemy
                        enemyHealth.health = enemyList[0].typeHealth;
                        enemy.GetComponent<EnemyBehaviour>().damageToDeal = enemyList[0].typeDamage;
                        enemy.GetComponent<EnemyBehaviour>().canExplode = false;
                        enemy.GetComponent<UnityEngine.AI.NavMeshAgent>().speed = enemyList[0].typeSpeed;
                        legsBasicActive = true;
                        bellyBasicActive = true;
                        break;

                    case Type.Speedy:   //If speedy enemy
                        enemyHealth.health = enemyList[1].typeHealth;
                        enemy.GetComponent<EnemyBehaviour>().damageToDeal = enemyList[1].typeDamage;
                        enemy.GetComponent<EnemyBehaviour>().canExplode = false;
                        enemy.GetComponent<UnityEngine.AI.NavMeshAgent>().speed = enemyList[1].typeSpeed;
                        legsSpeedyActive = true;
                        bellyBasicActive = true;
                        break;

                    case Type.Heavy:    //If heavy enemy
                        enemyHealth.health = enemyList[2].typeHealth;
                        enemy.GetComponent<EnemyBehaviour>().damageToDeal = enemyList[2].typeDamage;
                        enemy.GetComponent<EnemyBehaviour>().canExplode = false;
                        enemy.GetComponent<UnityEngine.AI.NavMeshAgent>().speed = enemyList[2].typeSpeed;
                        legsBasicActive = true;
                        crotchHeavyActive = true;
                        leftHipHeavyActive = true;
                        rightHipHeavyActive = true;
                        chestHeavyActive = true;
                        bellyBasicActive = true;
                        break;
                    case Type.QuickHeavy:   //If quick heavy enemy
                        enemyHealth.health = enemyList[3].typeHealth;
                        enemy.GetComponent<EnemyBehaviour>().damageToDeal = enemyList[3].typeDamage;
                        enemy.GetComponent<EnemyBehaviour>().canExplode = false;
                        enemy.GetComponent<UnityEngine.AI.NavMeshAgent>().speed = enemyList[3].typeSpeed;
                        legsSpeedyActive = true;
                        crotchHeavyActive = true;
                        leftHipHeavyActive = true;
                        rightHipHeavyActive = true;
                        chestHeavyActive = true;
                        bellyBasicActive = true;
                        break;
                    case Type.Explodey:     //If explodey enemy
                        enemyHealth.health = enemyList[4].typeHealth;
                        enemy.GetComponent<EnemyBehaviour>().damageToDeal = enemyList[4].typeDamage;
                        enemy.GetComponent<EnemyBehaviour>().canExplode = true;
                        enemy.GetComponent<UnityEngine.AI.NavMeshAgent>().speed = enemyList[4].typeSpeed;
                        legsBasicActive = true;
                        chestHeavyActive = true;
                        bellyExplodeyActive = true;
                        break;
                }

                if (enemy != null)
                {
                    enemyHealth.hasBroken = false;

                    //Position & Rotation
                    enemy.transform.position = posToSpawn;
                    enemy.transform.rotation = activeSpawns[spawnPointIndex].transform.rotation;

                    //Set as Active
                    activeEnemies.Add(enemy);

                    //enemyHealth.SetKinematic(true);

                    //Enable enemy
                    enemy.SetActive(true);

                    //Enable modular pieces
                    legsBasic.gameObject.SetActive(legsBasicActive);
                    legsSpeedy.gameObject.SetActive(legsSpeedyActive);
                    crotchHeavy.gameObject.SetActive(crotchHeavyActive);
                    leftHipHeavy.gameObject.SetActive(leftHipHeavyActive);
                    rightHipHeavy.gameObject.SetActive(rightHipHeavyActive);
                    chestHeavy.gameObject.SetActive(chestHeavyActive);
                    bellyBasic.gameObject.SetActive(bellyBasicActive);
                    bellyExplodey.gameObject.SetActive(bellyExplodeyActive);
                }
                //Debug.Log("Spawn Complete");
                return;
            }
            // Spawn enemy success, return
        }
    }

}
