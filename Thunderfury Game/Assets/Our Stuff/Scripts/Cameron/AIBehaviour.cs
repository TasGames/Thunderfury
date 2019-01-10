// MoveTo.cs
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;

public class AIBehaviour : MonoBehaviour
{

    public Transform goal;

    [ShowInInspector, ReadOnly]
    UnityEngine.AI.NavMeshAgent agent;
    public PlayerHealth player;
    Animator anim;

    public float damageToDeal;
    [SerializeField] protected float damageWait;
    protected bool dealtDamage;

    float distance = 2.0f;
    LayerMask layerMask = 1 << 9;

    void Start()
    {
        anim = GetComponent<Animator>();

        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        //player = GetComponent<PlayerHealth>();

        dealtDamage = false;
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

                //if (animOver)
                //{
                //    Debug.Log("hello");
                //    DoIDealDamage();
                //    AnimationOver(false);
                    
                //}
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

    public void AnimationOver()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), distance, layerMask))
        {
            Debug.Log(dealtDamage);
            DoIDealDamage();
        }
    }
}

