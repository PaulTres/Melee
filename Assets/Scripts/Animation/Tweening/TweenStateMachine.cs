using PrimeTween;
using UnityEngine;
using Utilities;

namespace Animation.Tweening
{
    public class TweenStateMachine : ImprovedMonoBehaviour, ITweenStateMachine
    {
        [field: SerializeField] public TweenState DefaultState { get; protected set; }
        [field: SerializeField] public TweenState CurrentState { get; protected set; }
        protected Sequence sequence;

        protected virtual void OnEnable()
        {
            PerformReset();
        }

        public void PerformReset()
        {
            ChangeState(DefaultState, true);
        }

        /// <summary>
        /// Sets current sequence to the provided sequence and unpauses it. If there is an active sequence, it will be killed first.
        /// </summary>
        /// <param name="sequence"></param>
        public void SetSequence(Sequence sequence)
        {
            KillActiveSequence();
            this.sequence = sequence;
            if (this.sequence.isAlive) this.sequence.isPaused = false;
        }

        public void KillActiveSequence()
        {
            if (sequence.isAlive)
            {
                sequence.Stop();
            }
        }

        public bool ChangeState(TweenState newState, bool force = false)
        {
            if (newState == null) return false;
            if (force || CurrentState == null || CurrentState.CanChangeState(newState))
            {
                if (CurrentState != null) CurrentState.ExitState();
                CurrentState = newState;
                CurrentState.EnterState();
                return true;
            }
            return false;
        }
    }
}
