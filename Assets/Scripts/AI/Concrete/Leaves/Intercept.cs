using System;
using UnityEngine;
using Utilities;

namespace HybridBT2
{
    public class Intercept : LeafNodeData
    {
        [SerializeField] protected float maxTargetError = .1f;

        protected override Action<Node, Blackboard> onEnter => (n, ctx) =>
        {
            ctx.Agent.isStopped = false;
        };

        protected override Func<LeafNode, Blackboard, NodeState> onEvaluate => (n, ctx) =>
        {
            var target = ctx.GetData<EntityBase>(Blackboard.Keys.Target);
            if (!target.Active()) return NodeState.FAILURE;
            var pos = n.GetData<Vector3?>(0);
            if (pos == null || Vector3.Distance(pos.Value, target.position) > maxTargetError)
            {
                ctx.Agent.SetDestination(target.position);
                n.SetData(0, target.position);
            }
            return NodeState.RUNNING;
        };
    }
}