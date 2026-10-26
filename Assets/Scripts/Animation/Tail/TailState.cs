using UnityEngine;

namespace Animation
{
    public abstract class TailState : State
    {
        [SerializeField] protected Tail tail;
        protected float elapsedTime;

        protected virtual void OnValidate()
        {
            if (tail == null) tail = GetComponentInParent<Tail>();
        }

        public override void EnterState()
        {
            base.EnterState();
            //Debug.Log("Entering " + this);
            elapsedTime = 0;
        }

        public override void ExitState()
        {
            base.ExitState();
            //Debug.Log("Exiting " + this);
        }

        public override void UpdateState(float deltaTime)
        {
            elapsedTime += deltaTime;
        }
    }
}