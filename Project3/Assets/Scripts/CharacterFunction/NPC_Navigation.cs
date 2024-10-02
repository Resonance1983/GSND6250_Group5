using System;
using UnityEngine;
using UnityEngine.AI;

public class NPC_Navigation : MonoBehaviour
{
    public Transform goalTransform;
    private NavMeshAgent navAgent;
    
    private void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }

    void Update () { 
        navAgent.destination = goalTransform.position; 
    }
    
}
