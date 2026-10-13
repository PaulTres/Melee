using UnityEngine;

namespace Animation
{

    public class AnimatorStateMachineState : State
    {
        [field: SerializeField] public string StateName { get; protected set; }
        [SerializeField] protected AnimationClip clip;
        [SerializeField] protected AnimatorStateMachine stateMachine;
        [SerializeField] protected float crossfadeTime = .1f;
        protected int stateHash;

        protected virtual void OnValidate()
        {
            if (clip != null)
            {
                StateName = clip.name;
            }
            if (StateName.Length > 0) stateHash = Animator.StringToHash(StateName);
            if (stateMachine == null) stateMachine = GetComponentInParent<AnimatorStateMachine>();
        }
        public override void EnterState()
        {
            base.EnterState();
            stateMachine.Animator.CrossFade(stateHash, crossfadeTime);
        }

        public override void UpdateState(float deltaTime)
        {
            //nothing
        }
    }
}