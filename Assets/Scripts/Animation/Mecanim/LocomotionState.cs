using UnityEngine;
using UnityEngine.AI;

namespace Animation
{
    public class LocomotionState : AnimatorStateMachineState
    {
        static readonly int xVelocityHash = Animator.StringToHash("XVelocity");
        static readonly int zVelocityHash = Animator.StringToHash("ZVelocity");
        [SerializeField] protected NavMeshAgent agent;

        protected override void OnValidate()
        {
            base.OnValidate();
            agent = GetComponentInParent<NavMeshAgent>();
        }

        public override void UpdateState(float _)
        {
            var localVelocity = stateMachine.Transform.InverseTransformDirection(agent.velocity);
            stateMachine.Animator.SetFloat(xVelocityHash, localVelocity.x);
            stateMachine.Animator.SetFloat(zVelocityHash, localVelocity.z);
        }
    }
}