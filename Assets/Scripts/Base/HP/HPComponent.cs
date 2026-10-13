using Effects;
using EventBus;
using System;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace HP
{
    public class HPComponent : ImprovedMonoBehaviour, IResettable
    {
        #region Fields
        [field: SerializeField] public float MaxHP { get; set; } = 10;
        [Tooltip("The unit will spawn with maxHP*this number.")]
        [Range(0, 1)][SerializeField] protected float startingHPFraction = 1;
        [SerializeField] protected float currentHP;
        public float CurrentHP
        {
            get => currentHP;
            set
            {
                value = Mathf.Clamp(value, 0, MaxHP);
                if (currentHP != value)
                {
                    currentHP = value;
                    if (healthBar != null)
                    {
                        healthBar.fillAmount = currentHP / MaxHP;
                    }
                }
            }
        }
        protected bool firedEvent = false;
        protected EntityBase entity;
        public EntityBase Entity
        {
            get
            {
                if (entity == null) entity = Transform.root.GetComponent<EntityBase>();
                return entity;
            }
        }
        [Tooltip("Scale factor that will be applied to effects spawned when this component receives damage.")]
        [SerializeField] protected float effectScaleFactor = .5f;
        [SerializeField] protected PoolableEffect effectOnDamage;
        [Tooltip("If true, this entity will not take damage.")]
        [SerializeField] protected bool invulnerable = false;
        [SerializeField] protected bool causeInterruptsOnDamage = true;
        [SerializeField] protected bool debug;
        [SerializeField] protected Image healthBar;
        #endregion

        #region Setup
        protected void OnValidate()
        {
            if (Transform.GetComponentsInChildren<DamageReceiver>() == null) Debug.LogError($"{Transform} has no damage receivers!");
            if (GetComponent<DamageReceiver>() == null)
            {
                Debug.Log($"{this} has no damage receiver, adding damage receiver!");
                gameObject.AddComponent<DamageReceiver>();
            }
            MaxHP = Mathf.Max(MaxHP, .1f);
        }

        public void PerformReset()
        {
            firedEvent = false;
            CurrentHP = MaxHP * startingHPFraction;
        }

        protected void OnEnable() => PerformReset();
        #endregion

        public void TakeDamage(DamageInfo damageInfo)
        {
            if (invulnerable) return;
            if (debug) Debug.Log($"{this} took {damageInfo.Amount} damage from {damageInfo.Source}.");
            CurrentHP -= damageInfo.Amount;
            if (causeInterruptsOnDamage) EventBus<AttackInterruptEvent>.Raise(Transform.root.GetEntityId(), null);
            if (CurrentHP <= 0 && !firedEvent)
            {
                firedEvent = true;
                ObjectDestroyed @event = new(Transform.GetEntityId(), damageInfo.Source);
                EventBus<ObjectDestroyed>.Raise(Transform.GetEntityId(), @event);
            }
            else
            {
                effectOnDamage.SpawnEffect(damageInfo.Position, Quaternion.identity, effectScaleFactor);
            }
        }
    }
}