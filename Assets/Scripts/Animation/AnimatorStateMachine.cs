using UnityEngine;

namespace Animation
{

    [RequireComponent(typeof(Animator))]
    public class AnimatorStateMachine : ImprovedMonoBehaviour
    {
        protected Animator animator;
        public Animator Animator => animator;
        protected AnimatorStateMachineState currentState;

        protected void OnValidate()
        {
            animator = GetComponent<Animator>();
        }

        public bool ChangeState(AnimatorStateMachineState newState)
        {
            throw new System.NotImplementedException();
            //if (currentState == null || currentState.CanChangeToNewState(newState))
            //{
            //    if (currentState != null) currentState.ExitState();
            //    currentState = newState;
            //    currentState.EnterState();
            //    return true;
            //}
            //return false;
        }

        public void ForceChangeState(AnimatorStateMachineState newState)
        {
            if (currentState != null) currentState.ExitState();
            currentState = newState;
            currentState.EnterState();
        }
    }
}