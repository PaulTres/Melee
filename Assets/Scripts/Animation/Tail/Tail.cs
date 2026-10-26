using System.Collections;
using UnityEngine;
using Utilities;
using Weapons;

namespace Animation
{
    [RequireComponent(typeof(TailIdleState))]
    /// <summary>
    /// governs movements for tails/tentacles
    /// </summary>
    public class Tail : StateMachine<TailState>
    {
        [field: SerializeField] public Transform IKTarget { get; protected set; }
        [field: SerializeField] public Transform RecoveryPoint { get; protected set; }
        [field: SerializeField] public Transform WindupPoint { get; protected set; }
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

        protected void OnValidate()
        {
            defaultState = GetComponent<TailIdleState>();
            RecoveryState = GetComponentInChildren<TailRecoveryState>();
            StabState = GetComponentInChildren<TailStabState>();
            WindupState = GetComponentInChildren<TailWindupState>();
        }

        public override void PerformReset()
        {
            base.PerformReset();
            Target = null;
        }

        public bool PerformAttack()
        {
            if (!Target.Active()) return false;
            return ChangeState(WindupState);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            StartCoroutine(Tick());
        }

        protected void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(position, maxRange);
        }

        protected IEnumerator Tick()
        {
            Target = t;
            while (Target.Active())
            {
                yield return new WaitUntil(() => Vector3.Distance(t.position, position) <= maxRange);
                PerformAttack();
                yield return new WaitForSeconds(attackCooldown);
            }
        }
    }
}