using System;
using UnityEngine;

namespace Animation.Tweening
{
    public abstract class TweenState : ImprovedMonoBehaviour, IState
    {
        [SerializeField] protected int priority;
        public int Priority => priority;
        [SerializeField] protected bool canInterruptSelf;
        public bool CanInterruptSelf => canInterruptSelf;
        public event Action OnEnter = delegate { }, OnExit = delegate { };
        [SerializeField] protected Transform stateMachineHolder;
        protected ITweenStateMachine stateMachine;
        [Tooltip("If true, the state machine will return to the default state when this state exits. Uses ChangeToDefault() method.")]
        [SerializeField] protected bool returnToDefaultOnExit = true;

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
            stateMachine.KillActiveSequence();
            OnEnter?.Invoke();
        }

        public virtual void ExitState()
        {
            OnExit?.Invoke();
            if (returnToDefaultOnExit) stateMachine.ChangeToDefault();
        }

        public void UpdateState(float _)
        {
            throw new NotImplementedException("TweenState does not implement UpdateState method.");
        }
    }
}
