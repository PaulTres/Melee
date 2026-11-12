using PrimeTween;

namespace Animation.Tweening
{
    public interface ITweenStateMachine : IResettable
    {
        TweenState CurrentState { get; }
        TweenState DefaultState { get; }
        bool ChangeState(TweenState newState, bool force = false);
        void KillActiveSequence();
        void SetSequence(Sequence sequence);
    }
}
