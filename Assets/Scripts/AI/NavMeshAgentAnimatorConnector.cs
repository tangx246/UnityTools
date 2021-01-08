using UnityEngine;
using UnityEngine.AI;

namespace UnityTools
{
    public class NavMeshAgentAnimatorConnector : MonoBehaviour
    {
        public NavMeshAgent navMeshAgent;
        public Animator animator;
        public string animatorSpeedVariable = "speed";
        public float speedMultiplier = 1f;
        public float minVelocity = 0.1f;

        private void OnValidate()
        {
            navMeshAgent = GetComponentInChildren<NavMeshAgent>();
            animator = GetComponentInChildren<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            var agentVelocity = navMeshAgent.velocity.magnitude;
            if (agentVelocity < minVelocity)
            {
                agentVelocity = 0;
            }
            animator.SetFloat(animatorSpeedVariable, agentVelocity * speedMultiplier);
        }
    }
}