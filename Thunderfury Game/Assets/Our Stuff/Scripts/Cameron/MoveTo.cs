// MoveTo.cs
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveTo : MonoBehaviour
{

    public Transform goal;

    UnityEngine.AI.NavMeshAgent agent;
    public PlayerHealth player;
    Animator anim;

    public float damageToDeal;
    [SerializeField] protected float damageWait;
    protected bool dealtDamage = false;

    float distance = 2.0f;


    void Start()
    {
        anim = GetComponent<Animator>();

        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

    }

    void Update()
    {
        goal = GameObject.FindGameObjectWithTag("Player").transform;
        if (goal != null)
        {
            agent.destination = goal.position;
        }


        if(Vector3.Distance(transform.position, goal.position) < distance)
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * distance, Color.red);

            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), distance))
            {
                if (dealtDamage == false)
                {
                    dealtDamage = true;
                    player.PlayerTakeDamage(damageToDeal);
                    Debug.Log("Dealt Damage to player");
                    Debug.Log(player.pShield);
                    Debug.Log(player.pHealth);
                }
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        anim.SetTrigger("Attack");
        agent.isStopped = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (dealtDamage == false)
        {
            dealtDamage = true;
            player.PlayerTakeDamage(damageToDeal);
            Debug.Log("Dealt Damage to player");
            Debug.Log(player.pShield);
            Debug.Log(player.pHealth);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        anim.SetTrigger("Run");
        agent.isStopped = false;
    }

    IEnumerator DealtDamageRoutine()
    {
        yield return new WaitForSeconds(damageWait);

        dealtDamage = false;
    }
}

