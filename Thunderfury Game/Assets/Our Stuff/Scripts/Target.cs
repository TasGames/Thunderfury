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

    [Title("Target Stats")]
    [SerializeField] protected float health;
    [SerializeField] protected GameObject brokenVersion;

    [Title("Score")]
    [SerializeField] protected int scoreValue;

    [Title("Drops")]
    [SerializeField] protected bool dropsPickup;
    [SerializeField] [ShowIf("dropsPickup", true)] [Range(0, 100)] protected float dropPercentage;
    [SerializeField] [ShowIf("dropsPickup", true)] protected Drops[] dropList;

    protected float totalWeight;

    WaveManager waveManager;
    EnemySpawner enemySpawner;

    Animator enemyAnim;

    void OnValidate()
    {
        totalWeight = 0f;
        if (dropList != null)
        {
            foreach (var Drops in dropList)
                totalWeight += Drops.weight;
        }
    }

    void Awake()
    {
        OnValidate();
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (this.gameObject.tag == "Enemy1")
        {
            if (enemyAnim == null)
            {
                enemyAnim = GetComponent<Animator>();
            }
            enemyAnim.SetTrigger("Hit");
        }

        if (health <= 0)
            Destroy();
    }

    void Destroy()
    {
        if (brokenVersion != null)
            Instantiate(brokenVersion, transform.position, transform.rotation);

        if (gameObject.tag == "Enemy1")
        {
            waveManager = GameObject.FindGameObjectWithTag("EnemyManager").GetComponent<WaveManager>();
            waveManager.enemiesRemaining = waveManager.enemiesRemaining - 1;    //Reduce # of remaining enemies in the wave

            enemySpawner = GameObject.FindGameObjectWithTag("EnemyManager").GetComponent<EnemySpawner>();
            enemySpawner.activeEnemies.Remove(this.gameObject);                 //Remove dead enemy from list

            gameObject.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }

        HUD.totalScore += scoreValue;

        if (dropsPickup == true)
            SpawnDrop();
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
            Instantiate(dropList[chosenIndex].pickup, transform.position, transform.rotation);
    }

}
