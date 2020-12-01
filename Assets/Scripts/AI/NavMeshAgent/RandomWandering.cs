using Panda;
using UnityEngine;
using UnityEngine.AI;

namespace UnityTools
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class RandomWandering : MonoBehaviour
    {
        public float wanderIntervalSeconds = 1f;
        public float wanderRadius = 5f;
        public float stoppingDistance = 0.5f;

        [SerializeField] private NavMeshAgent navMeshAgent;
        private float idleStartTime;

        private void OnValidate()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        [Task]
        public void WanderToRandomPosition()
        {
            if (Task.current.isStarting)
            {
                Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
                randomDirection += transform.position;

                NavMeshHit hit;
                NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, 1);

                navMeshAgent.destination = hit.position;
                navMeshAgent.avoidancePriority = Random.Range(50, 99);

                return;
            }

            if (navMeshAgent.pathPending)
            {
                return;
            }

            if (navMeshAgent.pathStatus == NavMeshPathStatus.PathInvalid || navMeshAgent.pathStatus == NavMeshPathStatus.PathPartial || navMeshAgent.remainingDistance == Mathf.Infinity)
            {
                Task.current.Fail();
            }

            if (navMeshAgent.remainingDistance <= stoppingDistance)
            {
                Task.current.Succeed();
            }
        }

        [Task]
        public void Idle()
        {
            if (Task.current.isStarting)
            {
                idleStartTime = Time.time;
                return;
            }

            if (Time.time - idleStartTime > wanderIntervalSeconds)
            {
                Task.current.Succeed();
            }
        }
    }
}