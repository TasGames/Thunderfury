using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {

    [SerializeField] Transform goal;

    UnityEngine.AI.NavMeshAgent agent;  //NavMeshAgent component attached to the enemy
    public PlayerHealth player;     //To call function inside the PlayerHealth script
    Animator anim;  //Animator component attached to the enemy

    LayerMask layMask = 1 << 9; //Enemy will only detect objects on 9th layer (Player)

    [SerializeField] float damageToDeal;    //Amount to damage player by
    [SerializeField] float attackRate;
    float nextAttack;                       //Time to wait before able to damage again
    [SerializeField] bool damageIsDealt;    //Does the enemy deal damage
    [SerializeField] float attackDistance;  //Range for the raycast


	// Use this for initialization
	void Start () {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        anim = GetComponent<Animator>();

        damageIsDealt = false;
    }

    // Update is called once per frame
    void Update () {
        goal = GameObject.FindGameObjectWithTag("Player").transform;       //Finds player and sets as the enemy's goal
        if (goal != null)
        {
            agent.destination = goal.position;
        }

        if (Vector3.Distance(transform.position, goal.position) < attackDistance)
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * attackDistance, Color.red);

            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), attackDistance, layMask))
            {


                nextAttack = Time.time + attackRate;
            }
        }

    }
}
