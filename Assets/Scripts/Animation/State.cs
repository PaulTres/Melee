using System;
using UnityEngine;
namespace Animation
{
    public abstract class State : ImprovedMonoBehaviour, IState
    {
        [SerializeField] protected int priority;
        public int Priority => priority;
        [SerializeField] protected bool canInterruptSelf;
        public bool CanInterruptSelf => canInterruptSelf;
        public event Action OnEnter = delegate { }, OnExit = delegate { };

        public virtual void EnterState()
        {
            OnEnter?.Invoke();
        }

        public abstract void UpdateState(float deltaTime);

        public virtual void ExitState()
        {
            OnExit?.Invoke();
        }
    }
}