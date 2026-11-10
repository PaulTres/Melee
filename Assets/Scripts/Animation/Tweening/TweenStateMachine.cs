using PrimeTween;
using UnityEngine;
using Utilities;

namespace Animation.Tweening
{
    public class TweenStateMachine : ImprovedMonoBehaviour, ITweenStateMachine
    {
        [field: SerializeField] public TweenState DefaultState { get; protected set; }
        public TweenState CurrentState { get; protected set; }
        protected Sequence sequence;

        protected virtual void OnEnable()
        {
            PerformReset();
        }

        public void PerformReset()
        {
            ForceChangeState(DefaultState);
        }

        public void SetSequence(Sequence sequence)
        {
            this.sequence = sequence;
            this.sequence.isPaused = false;
        }

        public void KillActiveSequence()
        {
            if (sequence.isAlive)
            {
                sequence.Stop();
            }
        }

        public virtual void ChangeToDefault(bool force = true)
        {
            if (force) ForceChangeState(DefaultState);
            else ChangeState(DefaultState);
        }

        public bool ChangeState(TweenState newState)
        {
            if (newState == null) return false;
            if (CurrentState == null || CurrentState.CanChangeState(newState))
            {
                if (CurrentState != null) CurrentState.ExitState();
                CurrentState = newState;
                CurrentState.EnterState();
                return true;
            }
            return false;
        }

        public void ForceChangeState(TweenState newState)
        {
            if (newState == null) return;
            if (CurrentState != null) CurrentState.ExitState();
            CurrentState = newState;
            CurrentState.EnterState();
        }
    }
}
