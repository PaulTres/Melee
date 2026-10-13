namespace Animation
{
    public class CompositeState<T> : State where T : State
    {
        protected T currentState;
        protected T defaultState;

        public override void EnterState()
        {
            base.EnterState();
            currentState = defaultState;
            currentState.EnterState();
        }

        public override void ExitState()
        {
            base.ExitState();
            currentState.ExitState();
        }

        public override void UpdateState(float deltaTime)
        {
            if (currentState != null) currentState.UpdateState(deltaTime);
        }

        public bool ChangeState(T newState)
        {
            if (newState == null) return false;
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

        public void ForceChangeState(T newState)
        {
            if (newState == null) return;
            if (currentState != null) currentState.ExitState();
            currentState = newState;
            currentState.EnterState();
        }
    }
}