namespace Animation
{
    public interface IFSM<T> : IResettable where T : IState
    {
        void ChangeToDefault();

        void UpdateState(float deltaTime);

        bool ChangeState(T newState);

        void ForceChangeState(T newState);
    }
}