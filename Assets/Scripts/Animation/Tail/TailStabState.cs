using UnityEngine;
using Utilities;

namespace Animation
{
    public class TailStabState : TailState
    {
        [SerializeField] protected float trackingTime = .5f;
        [SerializeField] protected float stabTime = .5f;
        protected Vector3 targetPosition;

        protected override void OnValidate()
        {
            base.OnValidate();
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
            //Debug.Log(elapsedTime);
            if (!tail.Target.Active() || elapsedTime >= stabTime)
            {
                //Debug.Log("Recovering");
                tail.ChangeState(tail.RecoveryState);
                return;
            }
            base.UpdateState(deltaTime);
            //Debug.DrawLine(tail.IKTarget.position, targetPosition, Color.red);
            tail.IKTarget.position = Vector3.Lerp(tail.IKTarget.position, targetPosition, elapsedTime / stabTime);
            tail.IKTarget.LookAt(targetPosition);
            //tail.IKTarget.rotation=Quaternion.Lerp(tail.IKTarget.rotation,
        }
    }
}