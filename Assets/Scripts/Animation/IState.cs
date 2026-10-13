namespace Animation
{
    public interface IState
    {
        int Priority { get; }
        bool CanInterruptSelf { get; }

        void EnterState();

        void ExitState();

        void UpdateState(float dt);
    }
}