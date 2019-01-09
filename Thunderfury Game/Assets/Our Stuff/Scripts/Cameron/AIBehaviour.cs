// MoveTo.cs
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIBehaviour : MonoBehaviour
{

    public Transform goal;

    UnityEngine.AI.NavMeshAgent agent;
    public PlayerHealth player;
    Animator anim;

    public float damageToDeal;
    [SerializeField] protected float damageWait;
    protected bool dealtDamage = false;

    float distance = 2.0f;
    LayerMask layerMask = 1 << 9;

    public bool animOver;

    void Start()
    {
        anim = GetComponent<Animator>();

        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        //player = GetComponent<PlayerHealth>();

        //animOver = false;
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

            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), distance, layerMask))
            {
                if (dealtDamage == false)
                {
                    anim.SetTrigger("Attack");
                    agent.isStopped = true;
                }

                if (animOver)
                {
                    Debug.Log("hello");
                    DoIDealDamage();
                    AnimationOver(false);
                    agent.isStopped = false;
                }
            }
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    agent.isStopped = true;
    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    agent.isStopped = false;
    //}

    IEnumerator DealtDamageRoutine()
    {
        yield return new WaitForSeconds(damageWait);

        dealtDamage = false;
    }

    public void DoIDealDamage()
    {
        if (dealtDamage == false)
        {
            dealtDamage = true;
            player.PlayerTakeDamage(damageToDeal);
            DealtDamageRoutine();
            Debug.Log("Dealt Damage");
        }
    }

    public bool AnimationOver(bool result)
    {
        animOver = result;
        Debug.Log(animOver);
        return animOver;
    }
}

