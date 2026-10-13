using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class TestDummy : EntityBase
{
    protected override void Die(ObjectDestroyed @event)
    {
        Debug.Log($"{this} is dead!");
    }
}
