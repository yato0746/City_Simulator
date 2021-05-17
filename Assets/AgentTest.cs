using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentTest : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform destination;

    private void Start()
    {
        agent.SetDestination(destination.position);
    }
}
