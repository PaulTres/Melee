using PrimeTween;

namespace Animation.Tweening
{
    public interface ITweenStateMachine : IResettable
    {
        TweenState CurrentState { get; }
        TweenState DefaultState { get; }
        void ChangeToDefault(bool force = true);
        bool ChangeState(TweenState newState);
        void ForceChangeState(TweenState newState);
        void KillActiveSequence();
        void SetSequence(Sequence sequence);
    }
}
