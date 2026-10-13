using UnityEngine.AI;

/// <summary>
/// An EntityBase which has a NavMeshAgent.
/// </summary>
public abstract class AgentEntity : EntityBase
{
    protected NavMeshAgent agent;
    /// <summary>
    /// The NavMeshAgent of this EntityBase. Lazily loaded from the root transport.
    /// </summary>
    public NavMeshAgent Agent
    {
        get
        {
            if (agent == null) agent = Transform.root.GetComponent<NavMeshAgent>();
            return agent;
        }
    }
}
