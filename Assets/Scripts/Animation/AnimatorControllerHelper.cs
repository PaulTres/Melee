using System.Collections.Generic;
using UnityEngine;

namespace Animation
{
    public class AnimatorControllerHelper : ImprovedMonoBehaviour
    {
        [SerializeField] protected Animator animator;
        [SerializeField] protected List<SerializableAnimatorControllerParameter> triggers, attackTriggers;

        protected virtual void OnValidate()
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

        /// <summary>
        /// Returns false if the attack does not exist.
        /// Resets all triggers.<br/>
        /// Then sets the trigger in the list identified by the attack parameter.
        /// </summary>
        /// <param name="attack">The trigger to execute. Located in attackTriggers.</param>
        /// <returns>True if the attack exists.</returns>
        public bool PerformAttack(int attack)
        {
            if (attack < attackTriggers.Count && attack >= 0)
            {
                ResetAllTriggers();
                animator.SetTrigger(attackTriggers[attack].Hash);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Reset all triggers.<br/> 
        /// Uses the triggers list in this script.
        /// </summary>
        public void ResetAllTriggers()
        {
            for (int i = 0; i < triggers.Count; i++)
            {
                animator.ResetTrigger(triggers[i].Hash);
            }
        }

        /// <summary>
        /// Resets all triggers except the specified one.<br/> 
        /// Uses the triggers list in this script.<br/>
        /// Comparison is done with .Equals().
        /// </summary>
        /// <param name="exception"></param>
        public void ResetTriggers(string exception)
        {
            for (int i = 0; i < triggers.Count; i++)
            {
                if (triggers[i].Name.Equals(exception)) continue;
                animator.ResetTrigger(triggers[i].Hash);
            }
        }

        public bool b;
        private void Update()
        {
            if (b)
            {
                b = false;
                PerformAttack(0);
            }
        }
    }
}