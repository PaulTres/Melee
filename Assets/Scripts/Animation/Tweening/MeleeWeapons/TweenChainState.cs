using PrimeTween;
using UnityEngine;
namespace Animation.Tweening
{
    public abstract class TweenChainState : TweenState
    {
        [SerializeField] protected TweenState nextState;

        public override Sequence BuildSequence()
        {
            var seq = Sequence.Create();
            if (nextState != null) seq = seq.OnComplete(() => stateMachine.ChangeState(nextState, true));
            return seq;
        }

        protected virtual void Awake()
        {
            if (nextState == null)
            {
                Debug.LogWarning("Next state is not assigned. Please assign it in the inspector.");
            }
        }
    }
}