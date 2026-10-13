using UnityEngine;

namespace Animation
{
    public class TailIdleState : TailState
    {
        [SerializeField] protected float waiveMagnitude = .1f;

        public override void EnterState()
        {
            base.EnterState();
            tail.Weapon.DisableDamage();
        }

        public override void UpdateState(float deltaTime)
        {
            base.UpdateState(deltaTime);
            tail.IKTarget.position = tail.RecoveryPoint.position + Random.insideUnitSphere * waiveMagnitude;
        }
    }
}