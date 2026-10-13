using UnityEngine;
using Utilities;

namespace Animation
{
    public abstract class StateMachine<T> : ImprovedMonoBehaviour where T : State
    {
        protected T currentState;
        [SerializeField] protected T defaultState;

        #region Setup
        protected virtual void OnEnable()
        {
            GlobalUpdater.RegisterUpdate(PerformUpdate);
            if (defaultState != null) ForceChangeState(defaultState);
        }

        protected virtual void OnDisable()
        {
            GlobalUpdater.DeregisterUpdate(PerformUpdate);
        }
        #endregion

        public virtual void ChangeToDefault() => ChangeState(defaultState);

        protected virtual void PerformUpdate(float dt)
        {
            if (currentState != null) currentState.UpdateState(dt);
        }

        public bool ChangeState(T newState)
        {
            if (newState == null) return false;
            if (currentState == null || currentState.CanChangeState(newState))
            {
                if (currentState != null) currentState.ExitState();
                currentState = newState;
                currentState.EnterState();
                return true;
            }
            return false;
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