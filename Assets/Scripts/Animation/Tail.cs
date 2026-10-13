using System.Collections;
using UnityEngine;
using Utilities;
using Weapons;

namespace Animation
{
    /// <summary>
    /// governs movements for tails/tentacles
    /// </summary>
    public class Tail : StateMachine<TailState>
    {
        [field: SerializeField] public Transform IKTarget { get; protected set; }
        [field: SerializeField] public Transform RecoveryPoint { get; protected set; }
        [field: SerializeField] public Transform WindupPoint { get; protected set; }
        [field: SerializeField] public Transform StabPoint { get; protected set; }
        [field: SerializeField] public TailWindupState WindupState { get; protected set; }
        [field: SerializeField] public TailStabState StabState { get; protected set; }
        [field: SerializeField] public TailRecoveryState RecoveryState { get; protected set; }
        [SerializeField] protected float attackCooldown = 1;
        [SerializeField] protected float maxRange = 3;
        public Transform Target { get; set; }
        protected MeleeWeapon weapon;
        public MeleeWeapon Weapon
        {
            get
            {
                if (weapon == null) weapon = GetComponentInChildren<MeleeWeapon>();
                return weapon;
            }
        }

        public Transform t;

        protected override void OnEnable()
        {
            base.OnEnable();
            PerformReset();
            Target = t;
        }

        public void PerformReset() => Target = null;

        public bool PerformAttack()
        {
            if (!Target.Active()) return false;
            ChangeState(WindupState);
            return true;
        }

        protected IEnumerator Tick()
        {
            while (t.Active())
            {
                yield return new WaitUntil(() => Vector3.Distance(t.position, position) <= maxRange);
                PerformAttack();
                yield return new WaitForSeconds(attackCooldown);
            }
        }

        private void Update()
        {
            if (t.Active())
            {
                Target = t;
                PerformAttack();
            }
        }
    }
}