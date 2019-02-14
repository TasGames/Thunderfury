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


    // Use this for initialization
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        anim = GetComponent<Animator>();

        goal = GameObject.Find("Player").transform;       //Finds player and sets as the enemy's goal
        player = GameObject.Find("Player").GetComponent<PlayerHealth>();


    }

    // Update is called once per frame
    void Update()
    {
        if (goal != null)
        {
            agent.SetDestination(goal.position);
        }

        if (Vector3.Distance(transform.position, goal.position) < attackDistance)
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * attackDistance, Color.red);

            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), attackDistance, layMask) && Time.time > nextAttack)  //If raycast hits player and cooldown is over
            {
                nextAttack = Time.time + attackRate;    //Set next attack to be after current time + attack rate/cooldown
                anim.SetTrigger("Attack");              //Play attack animation
                agent.isStopped = true;                 //Stop movement
            }
        }
    }

    public void DealDamage()
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

    public void BeginMovement()
    {
        agent.isStopped = false;
    }
}
