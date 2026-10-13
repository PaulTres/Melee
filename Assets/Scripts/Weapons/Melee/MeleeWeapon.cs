using EventBus;
using System;
using UnityEngine;

namespace Weapons
{
    public class MeleeWeapon : HurtboxWeapon
    {
        [Tooltip("This object will be used to toggle the trigger colliders on/off.")]
        [SerializeField] protected GameObject triggerParent;

        #region Setup
        protected override void Awake()
        {
            base.Awake();
            EventBus<AttackInterruptEvent>.AddActions(Transform.root.GetEntityId(), null, InterruptAttack);
        }

        protected virtual void OnDestroy()
        {
            EventBus<AttackInterruptEvent>.RemoveActions(Transform.root.GetEntityId(), null, InterruptAttack);
        }
        #endregion

        public void InterruptAttack()
        {
            //crossfade to idle and deactivate colliders
            DisableDamage();
            throw new NotImplementedException();
        }

        public void BeginBlocking()
        {
            DisableDamage();
            throw new NotImplementedException();
        }

        public void EnableDamage()
        {
            triggerParent.SetActive(true);
        }

        public void DisableDamage()
        {
            triggerParent.SetActive(false);
        }

        public override bool PerformAttack(int attack)
        {
            throw new NotImplementedException();
        }
    }
}