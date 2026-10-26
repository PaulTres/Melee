using UnityEngine;
using UnityEngine.AI;

public class NavAgentTest : ImprovedMonoBehaviour
{
    protected NavMeshAgent agent;
    [SerializeField] protected Transform target;
    private void Awake()
    {
        agent=GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (agent != null && target != null)
        {
            agent.SetDestination(target.position);
        }
    }
}
