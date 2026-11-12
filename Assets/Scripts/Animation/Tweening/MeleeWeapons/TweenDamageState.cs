using UnityEngine;
using Weapons;

namespace Animation.Tweening
{
    public abstract class TweenDamageState : TweenChainState
    {
        [SerializeField] protected MeleeWeapon meleeWeapon;

        public override void EnterState()
        {
            base.EnterState();
            EnableDamage();
        }

        public override void ExitState()
        {
            base.ExitState();
            DisableDamage();
        }

        public void EnableDamage() => meleeWeapon.EnableDamage();

        public void DisableDamage() => meleeWeapon.DisableDamage();
    }
}