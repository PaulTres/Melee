using UnityEngine;

namespace Animation
{

    public class TailRecoveryState : TailState
    {
        [SerializeField] protected float lingerTime = .1f;
        [SerializeField] protected float recoveryTime = 1;

        protected void OnValidate()
        {
            lingerTime = Mathf.Min(lingerTime, recoveryTime);
        }

        public override void EnterState()
        {
            base.EnterState();
            tail.Weapon.DisableDamage();
        }

        public override void UpdateState(float deltaTime)
        {
            //move to the recovery point
            base.UpdateState(deltaTime);
            if (elapsedTime > lingerTime)
            {
                tail.IKTarget.position = Vector3.Lerp(tail.IKTarget.position, tail.RecoveryPoint.position, elapsedTime / recoveryTime);
                if (elapsedTime >= recoveryTime)
                {
                    tail.ChangeToDefault();
                }
            }
        }
    }
}