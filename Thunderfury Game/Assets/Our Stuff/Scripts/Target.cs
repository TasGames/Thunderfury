using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Target : MonoBehaviour
{
    [System.Serializable]
    protected struct Drops
    {
        public GameObject pickup;
        public float weight;
    }

    public Transform enemyPrefab;

    [Title("Target Stats")]
    [SerializeField] public float health;
    [SerializeField] protected GameObject brokenVersion;
    [SerializeField] protected GameObject damagePopUp;
    [SerializeField] protected DamageValues dv;
    protected GameObject player;

    [Title("Score")]
    [SerializeField] protected int scoreValue;

    [Title("Drops")]
    [SerializeField] protected bool dropsPickup;
    [SerializeField] [ShowIf("dropsPickup", true)] [Range(0, 100)] protected float dropPercentage;
    [SerializeField] [ShowIf("dropsPickup", true)] protected Drops[] dropList;

    protected float totalWeight;
    [HideInInspector] public bool hasBroken = false;
    [HideInInspector] public bool hasGivenScore = false;
    [HideInInspector] public float originalHealth;

    WaveManager waveManager;
    EnemySpawner enemySpawner;

    //Components to disable for ragdoll
    EnemyBehaviour enemyBehaviour;
    Animator enemyAnim;
    UnityEngine.AI.NavMeshAgent enemyAgent;
    

    void OnValidate()
    {
        totalWeight = 0f;
        if (dropList != null)
        {
            foreach (var Drops in dropList)
                totalWeight += Drops.weight;
        }

        originalHealth = health;    //To reset health in EnemySpawner.cs
    }

    void Awake()
    {
        OnValidate();
        player = GameObject.FindGameObjectWithTag("Player");
        //dv = damagePopUp.GetComponentInChildren<DamageValues>();
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (this.gameObject.tag == "Enemy1")    //If enemy is taking damage
        {
            if (enemyAnim == null)
            {
                enemyAnim = GetComponent<Animator>();
            }
            enemyAnim.SetTrigger("Hit");    //Trigger HitByPlayer animation
        }

        if (damagePopUp != null)
        {
            float xOffset = Random.Range(-0.5f, 0.5f);
            float yOffset = Random.Range(0.5f, 1f);
            Vector3 finalPos = transform.position;
            finalPos.x += xOffset;
            finalPos.y += yOffset;
            dv.damageText.text = amount.ToString();
            GameObject damPop = Instantiate(damagePopUp, finalPos, transform.rotation);
            StartCoroutine(RotateRoutine(damPop));
            Destroy(damPop, 1);
        }

        if (health <= 0)
            Destroy();
    }

    void Destroy()
    {
        if (brokenVersion != null && hasBroken == false)
        {
            GameObject broke = Instantiate(brokenVersion, transform.position, transform.rotation);
            hasBroken = true;
            Destroy(broke, 10);
        }

        if (gameObject.tag == "Enemy1")
        {
            waveManager = GameObject.FindGameObjectWithTag("EnemyManager").GetComponent<WaveManager>();
            waveManager.enemiesRemaining = waveManager.enemiesRemaining - 1;    //Reduce # of remaining enemies in the wave

            enemySpawner = GameObject.FindGameObjectWithTag("EnemyManager").GetComponent<EnemySpawner>();
            enemySpawner.activeEnemies.Remove(this.gameObject);                 //Remove dead enemy from list

            enemyBehaviour = gameObject.GetComponent<EnemyBehaviour>();
            enemyBehaviour.canCheckForAttack = false;
            //gameObject.SetActive(false);
            enemyBehaviour.enabled = false;
            enemyAnim.enabled = false;
            GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
            GetComponent<BoxCollider>().enabled = false;
            SetKinematic(false);
            gameObject.layer = 11;
            StartCoroutine(ResetEnemy());
        }
        else
        {
            Destroy(gameObject);
        }

        if (hasGivenScore == false)
        {
            HUD.totalScore += scoreValue;

            if (dropsPickup == true)
                SpawnDrop();

            hasGivenScore = true;
        }

    }

    void SpawnDrop()
    {
        float pick = Random.value * totalWeight;
        int chosenIndex = 0;
        float cumulativeWeight = dropList[0].weight;

        while (pick > cumulativeWeight && chosenIndex < dropList.Length - 1)
        {
            chosenIndex++;
            cumulativeWeight += dropList[chosenIndex].weight;
        }

        int randomNum = Random.Range(0, 100);

        if (randomNum <= dropPercentage)
            Instantiate(dropList[chosenIndex].pickup, transform.position + new Vector3(0, 0.5f, 0), transform.rotation);
    }

    IEnumerator RotateRoutine(GameObject GO)
    {
        while (true)
        {
            GO.transform.LookAt(player.transform);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void SetKinematic(bool newValue)
    {
        Rigidbody[] bodies = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in bodies)
        {
            rb.isKinematic = newValue;
        }
    }

    IEnumerator ResetEnemy(){
        yield return new WaitForSeconds(10.0f);

        //SetKinematic(true);
        //enemyBehaviour.enabled = true;
        //enemyAnim.enabled = true;
        //GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
        //gameObject.SetActive(false);
        //gameObject.layer = 10;
        ObjectPooler.SharedInstance.pooledObjects.Remove(gameObject);
        Destroy(gameObject);

        yield break;
    }
}
