using UnityEngine;

namespace Weapons
{
    public abstract class WeaponBase : ImprovedMonoBehaviour
    {
        [SerializeField] protected AttackType attackType;
        [SerializeField] protected float damage;
        protected DamageInfo damageInfo;

        protected virtual void Awake()
        {
            damageInfo = new()
            {
                Amount = damage,
                AttackType = attackType,
                Source = Transform.root
            };
        }

        /// <summary>
        /// Perform a specific attack. Returns true if the attack could be performed.
        /// </summary>
        /// <param name="attack">The type of attack to perform.</param>
        public abstract bool PerformAttack(int attack);
    }
}