using UnityEngine;

namespace Animation
{
    public abstract class TailState : State
    {
        [SerializeField] protected Tail tail;
        protected float elapsedTime;

        public override void EnterState()
        {
            base.EnterState();
            elapsedTime = 0;
        }

        public override void UpdateState(float deltaTime)
        {
            elapsedTime += deltaTime;
        }
    }
}