using PrimeTween;
using System;
using UnityEngine;

namespace Animation.Tweening
{
    public abstract class TweenState : ImprovedMonoBehaviour, IState
    {
        [SerializeField] protected int priority;
        public virtual int Priority => priority;
        [SerializeField] protected bool canInterruptSelf;
        public bool CanInterruptSelf => canInterruptSelf;
        public event Action OnEnter = delegate { }, OnExit = delegate { };
        [SerializeField] protected Transform stateMachineHolder;
        protected ITweenStateMachine stateMachine;

        public abstract Sequence BuildSequence();

        protected virtual void OnValidate()
        {
            if (stateMachineHolder == null || stateMachine == null)
            {
                var parent = Transform;
                while (parent != null)
                {
                    stateMachine = parent.GetComponent<ITweenStateMachine>();
                    if (stateMachine != null)
                    {
                        stateMachineHolder = parent;
                        break;
                    }
                    parent = parent.parent;
                }
                if (stateMachine == null)
                {
                    Debug.LogWarning($"No TweenStateMachine found in parent hierarchy of {name}.");
                }
            }
        }

        protected void OnDestroy()
        {
            OnEnter = null;
            OnExit = null;
        }

        public virtual void EnterState()
        {
            if (stateMachine == null)
            {
                throw new NullReferenceException($"TweenStateMachine component not found for {this}.");
            }
            stateMachine.SetSequence(BuildSequence());
            OnEnter?.Invoke();
        }

        public virtual void ExitState()
        {
            OnExit?.Invoke();
        }

        /// <summary>
        /// Attempts to set current state machine state to this. Returns the result.
        /// </summary>
        public bool ChangeToThis(bool force = false)
        {
            return stateMachine.ChangeState(this, force);
        }

        public void UpdateState(float _)
        {
            throw new NotImplementedException("TweenState does not implement UpdateState method.");
        }
    }
}
