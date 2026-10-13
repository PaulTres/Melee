using System;
using UnityEngine;

namespace HybridBT2
{
    [CreateAssetMenu(menuName = "HybridBT2/Services/TargetAcquisition", fileName = "TargetAcquisition")]
    public class TargetAcquisitionService : ServiceData
    {
        [SerializeField] protected TargetEvaluation evaluation;

        public override Action<Node, Blackboard> Action => (n, ctx) =>
        {
            var current = ctx.GetData<EntityBase>(Blackboard.Keys.Target);
            var currentScore = evaluation.EvaluateTarget(current, ctx);
            foreach (var enemy in ctx.NPC.GetEnemies())
            {
                var score = evaluation.EvaluateTarget(enemy, ctx);
                if (score > currentScore)
                {
                    currentScore = score;
                    current = enemy;
                }
            }
            ctx.SetData(Blackboard.Keys.Target, current);
        };
    }
}