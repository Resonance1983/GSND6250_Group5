using UnityEngine;
using UnityEngine.AI;

namespace LostFrame
{
    public class NPC_Navigation : MonoBehaviour
    {
        public Transform goalTransform;
        private NavMeshAgent navAgent;

        private void Start()
        {
            navAgent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            navAgent.destination = goalTransform.position;
        }
    }
}