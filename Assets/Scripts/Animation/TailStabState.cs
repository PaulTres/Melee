using UnityEngine;
using Utilities;

namespace Animation
{
    public class TailStabState : TailState
    {
        [SerializeField] protected float trackingTime = .5f;
        [SerializeField] protected float stabTime = .5f;
        protected Vector3 targetPosition;

        protected void OnValidate()
        {
            trackingTime = Mathf.Min(trackingTime, stabTime);
        }

        public override void EnterState()
        {
            base.EnterState();
            if (!tail.Target.Active())
            {
                tail.ChangeToDefault();
                return;
            }
            targetPosition = tail.Target.position;
            tail.Weapon.EnableDamage();
        }

        public override void ExitState()
        {
            base.ExitState();
            tail.Weapon.DisableDamage();
        }

        public override void UpdateState(float deltaTime)
        {
            if (!tail.Target.Active() || elapsedTime >= stabTime)
            {
                tail.ChangeState(tail.RecoveryState);
                return;
            }
            base.UpdateState(deltaTime);

            tail.IKTarget.position = Vector3.Lerp(tail.IKTarget.position, targetPosition, elapsedTime / stabTime);
        }
    }
}