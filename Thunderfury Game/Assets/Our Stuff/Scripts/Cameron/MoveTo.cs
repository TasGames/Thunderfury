// MoveTo.cs
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveTo : MonoBehaviour
{

    public Transform goal;

    UnityEngine.AI.NavMeshAgent agent;

    Animator anim;

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
    }

    private void OnCollisionEnter(Collision collision)
    {
        agent.isStopped = true;
        anim.SetTrigger("Attack");
    }

    private void OnCollisionExit(Collision collision)
    {
        anim.SetTrigger("Run");
        agent.isStopped = false;
    }

}

