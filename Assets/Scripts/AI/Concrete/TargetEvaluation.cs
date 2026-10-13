using UnityEngine;

namespace HybridBT2
{
    public abstract class TargetEvaluation : ScriptableObject
    {
        public abstract float EvaluateTarget(EntityBase entity, Blackboard blackboard);
    }
}