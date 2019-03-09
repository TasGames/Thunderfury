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
    [SerializeField] public float health;
    [SerializeField] protected GameObject brokenVersion;
    [SerializeField] protected GameObject damagePopUp;
    [SerializeField] protected DamageValues dv;
    [SerializeField] protected GameObject player;

    [Title("Score")]
    [SerializeField] protected int scoreValue;

    [Title("Drops")]
    [SerializeField] protected bool dropsPickup;
    [SerializeField] [ShowIf("dropsPickup", true)] [Range(0, 100)] protected float dropPercentage;
    [SerializeField] [ShowIf("dropsPickup", true)] protected Drops[] dropList;

    protected float totalWeight;
    [HideInInspector]
    public float originalHealth;

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

        originalHealth = health;    //To reset health in EnemySpawner.cs
    }

    void Awake()
    {
        OnValidate();
        player = GameObject.FindGameObjectWithTag("Player");
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
        if (brokenVersion != null)
        {
            GameObject broke = Instantiate(brokenVersion, transform.position, transform.rotation);
            Destroy(broke, 10);
        }

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

    IEnumerator RotateRoutine(GameObject GO)
	{
		while (true)
		{
			GO.transform.LookAt(player.transform);
			yield return new WaitForSeconds(0.1f);
		}
	}

}
