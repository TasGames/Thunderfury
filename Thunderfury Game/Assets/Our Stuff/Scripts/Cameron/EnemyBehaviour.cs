using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class EnemyBehaviour : MonoBehaviour
{

    Transform goal;

    //[ShowInInspector, ReadOnly]
    UnityEngine.AI.NavMeshAgent agent;      //NavMeshAgent component attached to the enemy
    //[ShowInInspector, ReadOnly]
    Animator anim;                          //Animator component attached to the enemy
    [ShowInInspector, ReadOnly]
    PlayerHealth player;                    //To call function inside the PlayerHealth script

    LayerMask layMask = 1 << 9;             //Enemy will only detect objects on 9th layer (Player)

    [SerializeField] float damageToDeal;    //Amount to damage player by
    [SerializeField] float attackRate;
    float nextAttack;                       //Time to wait before able to damage again
    [SerializeField] float attackDistance = 2;  //Range for the raycast
    bool canCheckForAttack;
    bool isInTrigger;

    [SerializeField] protected float beginWalkDelay;

    // Use this for initialization
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        anim = GetComponent<Animator>();

        goal = GameObject.Find("Player").transform;       //Finds player and sets as the enemy's goal
        player = GameObject.Find("Player").GetComponent<PlayerHealth>();

        canCheckForAttack = true;
        StartCoroutine(CheckForAttack());

        isInTrigger = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (goal != null)
        {
            agent.SetDestination(goal.position);
        }

        //Check if able to move again based on current animation
        if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Running") && agent.isStopped == true)
            BeginMovement();
    }

    public void DealDamage()    //Triggered in ZombieAttack animation timeline
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), attackDistance, layMask))
        {
            if (player != null)
            {
                player.PlayerTakeDamage(damageToDeal);
                Debug.Log("Dealt Damage");
            }
        }
    }

    public void BeginMovement() //Triggered in ZombieAttack animation timeline
    {
        if (agent.isStopped && isInTrigger == false)
            agent.isStopped = false;
    }

    public void StopMovement() //Triggered in ZombieAttack animation timeline
    {
        if (!agent.isStopped)
            agent.isStopped = true;
    }

    void AttackAnimation()
    {
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
            if (Vector3.Distance(transform.position, goal.position) < attackDistance)
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * attackDistance, Color.red);

                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), attackDistance, layMask) && Time.time > nextAttack)  //If raycast hits player and cooldown is over
                {
                    nextAttack = Time.time + attackRate;    //Set next attack to be after current time + attack rate/cooldown

                    AttackAnimation();
                    //StopMovement();                //Stop movement
                }
            }

            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StopMovement();
            isInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isInTrigger = false;
    }

    // void OnCollisionEnter(Collision col)
    // {
    //     if (col.gameObject.tag == "Player")
    //         player.PlayerTakeDamage(damageToDeal);
    // }
}
