using EventBus;
using UnityEngine;

namespace Weapons
{
    public abstract class MeleeWeapon : HurtboxWeapon
    {
        public class EnterDamageState : IEvent { }
        public class ExitDamageState : IEvent { }

        [Tooltip("This object will be used to toggle the trigger colliders on/off.")]
        [SerializeField] protected GameObject triggerParent;
        [field: SerializeField] public float BlockAngle { get; protected set; } = 60;

        #region Setup
        protected override void Awake()
        {
            base.Awake();
            EventBus<AttackInterruptEvent>.AddActions(Transform.root.GetEntityId(), null, DisableDamage);
        }

        public override void PerformReset()
        {
            base.PerformReset();
            DisableDamage();
            //triggerParent.SetActive(false);
        }

        protected virtual void OnDestroy()
        {
            EventBus<AttackInterruptEvent>.RemoveActions(Transform.root.GetEntityId(), null, DisableDamage);
        }
        #endregion

        public void EnableDamage()
        {
            triggerParent.SetActive(true);
        }

        public void DisableDamage()
        {
            triggerParent.SetActive(false);
        }
    }
}