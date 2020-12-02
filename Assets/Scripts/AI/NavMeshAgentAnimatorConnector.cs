using UnityEngine;
using UnityEngine.AI;

public class NavMeshAgentAnimatorConnector : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public Animator animator;
    public string animatorSpeedVariable = "speed";
    public float speedMultiplier = 1f;

    private void OnValidate()
    {
        navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat(animatorSpeedVariable, navMeshAgent.velocity.magnitude * speedMultiplier);
    }
}