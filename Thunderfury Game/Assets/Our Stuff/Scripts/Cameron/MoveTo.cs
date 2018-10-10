// MoveTo.cs
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveTo : MonoBehaviour
{

    public Transform goal;

    void Update()
    {
            UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        if (goal != null)
            agent.destination = goal.position;
    }
}

