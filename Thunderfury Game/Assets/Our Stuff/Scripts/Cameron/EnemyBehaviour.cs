using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class EnemyBehaviour : MonoBehaviour
{

    Transform goal;                         //Player's location
    UnityEngine.AI.NavMeshAgent agent;      //NavMeshAgent component attached to the enemy
    Animator anim;                          //Animator component attached to the enemy
    PlayerHealth player;                    //To call function inside the PlayerHealth script

    public float damageToDeal;    //Amount to damage player by
    [SerializeField] float attackRate;
    [HideInInspector] public float nextAttack;  //Time to wait before able to damage again
    [SerializeField] float attackDistance = 2;  //Range for the raycast
    [HideInInspector] public bool canCheckForAttack;
    public bool isInTrigger;

    [SerializeField] protected float beginWalkDelay;
    [SerializeField] GameObject handTrigger;
    [HideInInspector] public bool canExplode;

    // Use this for initialization
    void OnEnable()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        anim = GetComponent<Animator>();

        goal = GameObject.Find("Player").transform;       //Finds player and sets as the enemy's goal
        player = GameObject.Find("Player").GetComponent<PlayerHealth>();

        canCheckForAttack = true;
        StartCoroutine(CheckForAttack());
        StartCoroutine(CheckMovement());

        isInTrigger = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (goal != null)
        {
            var offset = (transform.position - goal.position).normalized * 1.0f;    // Destination offset so the enemy doesn't try to enter the player's stomach
            agent.SetDestination(goal.position + offset);    //Update player's location
        }
    }

    public void DealDamage()    //Triggered in ZombieAttack animation timeline
    {
        if (player != null)
        {
            player.PlayerTakeDamage(damageToDeal);
            //Debug.Log("Dealt Damage");

            DisableHandTrigger();
        }
    }

    IEnumerator BeginMovement() //Triggered in ZombieAttack animation timeline
    {
        yield return new WaitForSeconds(beginWalkDelay);
        if (agent.isStopped && isInTrigger == false)
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsTag("PunchAttack") && !anim.GetCurrentAnimatorStateInfo(0).IsTag("DownSwingAttack"))
            {
                agent.isStopped = false;
                //Debug.Log("Beginning Movement");
                yield break;
            }
        }
        yield break;
    }

    public void StartMovement()
    {
        if (agent.isStopped && isInTrigger == false)
        {
            //if (!anim.GetCurrentAnimatorStateInfo(0).IsTag("PunchAttack") && !anim.GetCurrentAnimatorStateInfo(0).IsTag("DownSwingAttack"))
            //{
            agent.isStopped = false;
            //Debug.Log("Beginning Movement");
            //}
        }
    }

    public void StopMovement() //Triggered in ZombieAttack animation timeline
    {
        if (!agent.isStopped || isInTrigger)
        {
            agent.isStopped = true;
            //Debug.Log("Stopping Movement");
        }
    }

    void AttackAnimation()
    {
        //Debug.Log("Attack Animation");
        StopMovement();
        int randNum = Random.Range(0, 2);

        switch (randNum)
        {
            case 0:
                anim.SetTrigger("Attack");
                break;
            case 1:
                anim.SetTrigger("Attack2");
                break;
        }
    }

    IEnumerator CheckForAttack()
    {
        while (canCheckForAttack)
        {
            if (Vector3.Distance(transform.position, goal.position) < attackDistance)   //If enemy within a certain distance of player
            {
                if (isInTrigger)  //If player is in the box trigger
                {
                    if (Time.time > nextAttack) //If cooldown is over
                    {
                        nextAttack = Time.time + attackRate;    //Set next attack to be after current time + attack rate/cooldown

                        AttackAnimation();
                        //StopMovement();                //Stop movement
                    }

                }
            }
            if (Vector3.Distance(transform.position, goal.position) > attackDistance)   //Temporary fix for bug where isInTrigger would stay true
            {
                if (isInTrigger)
                {
                    isInTrigger = false;
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StopMovement();
            isInTrigger = true;

        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isInTrigger = false;
        }
    }

    public void EnableHandTrigger()
    {
        if (!handTrigger.activeSelf)
            handTrigger.SetActive(true);
    }

    void DisableHandTrigger()
    {
        if (handTrigger.activeSelf)
            handTrigger.SetActive(false);
    }

    IEnumerator CheckMovement()
    {

        while (canCheckForAttack)
        {
            //Check if able to move again based on current animation
            if (anim.GetAnimatorTransitionInfo(0).IsName("PunchAttack -> Zombie Running") || anim.GetAnimatorTransitionInfo(0).IsName("DownSwingAttack -> Zombie Running") || anim.GetCurrentAnimatorStateInfo(0).IsTag("Running"))
            {
                if (agent.isStopped)
                {
                    //StartCoroutine(BeginMovement());
                    StartMovement();
                }
            }
            yield return new WaitForSeconds(0.3f);
        }

    }
}
