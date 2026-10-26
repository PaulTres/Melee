using UnityEngine;
using UnityEngine.AI;
using Utilities;

namespace Animation
{
    public class TwoLegController : ImprovedMonoBehaviour
    {
        [SerializeField] protected NavMeshAgent agent;
        [SerializeField] protected Animator animator;

        protected virtual void OnValidate()
        {
            if (agent == null) agent = GetComponentInParent<NavMeshAgent>();
            if (animator == null) animator = GetComponent<Animator>();
        }

        protected readonly int xHash = Animator.StringToHash("X"), yHash = Animator.StringToHash("Y");

        protected virtual void OnEnable()
        {
            GlobalUpdater.RegisterFixedUpdate(UpdateAnimatorParams);
        }

        protected virtual void OnDisable()
        {
            GlobalUpdater.DeregisterFixedUpdate(UpdateAnimatorParams);
        }

        protected void UpdateAnimatorParams(float _)
        {
            if (agent == null || animator == null) return;
            var velocity = Transform.InverseTransformDirection(agent.velocity / agent.speed);
            animator.SetFloat(xHash, velocity.x);
            animator.SetFloat(yHash, velocity.z);
        }
    }
}
