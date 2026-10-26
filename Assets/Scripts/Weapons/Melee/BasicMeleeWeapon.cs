using Animation;
using System.Collections.Generic;
using UnityEngine;

namespace Weapons
{
    public class BasicMeleeWeapon : MeleeWeapon
    {
        protected Animator animator;
        [SerializeField] protected List<SerializableAnimatorControllerParameter> triggers, attackTriggers;

        protected void OnValidate()
        {
            if (animator == null) animator = GetComponent<Animator>();
            if (animator != null)
            {
                triggers = new();
                attackTriggers = new();
                foreach (var param in animator.parameters)
                {
                    if (param.type == AnimatorControllerParameterType.Trigger)
                    {
                        SerializableAnimatorControllerParameter a = new(param);
                        triggers.Add(a);
                        if (param.name.Contains("Attack")) attackTriggers.Add(a);
                    }
                }
            }
        }

        public override bool PerformAttack(int attack)
        {
            if (attack < attackTriggers.Count && attack >= 0)
            {
                animator.SetTrigger(attackTriggers[attack].Hash);
                return true;
            }
            return false;
        }

        public void ResetAllTriggers()
        {
            for (int i = 0; i < triggers.Count; i++)
            {
                animator.ResetTrigger(triggers[i].Hash);
            }
        }
    }
}