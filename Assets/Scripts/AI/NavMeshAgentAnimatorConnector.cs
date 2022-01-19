using UnityEngine;
using UnityEngine.AI;

namespace UnityTools
{
    public class NavMeshAgentAnimatorConnector : MonoBehaviour
    {
        public NavMeshAgent navMeshAgent;
        public Animator animator;
        [Tooltip("Hash for this is calculated OnEnable")] public string animatorSpeedVariable = "speed";
        public float speedMultiplier = 1f;

        [SerializeField] private int animatorSpeedVariableHash;

        public void Awake()
        {
            navMeshAgent = GetComponentInChildren<NavMeshAgent>();
            animator = GetComponentInChildren<Animator>();
        }

        public void OnEnable()
        {
            animatorSpeedVariableHash = Animator.StringToHash(animatorSpeedVariable);
        }

        public void Update()
        {
            animator.SetFloat(animatorSpeedVariableHash, navMeshAgent.velocity.magnitude * speedMultiplier);
        }
    }
}