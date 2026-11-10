using UnityEngine;
using Utilities;

namespace Animation
{
    public class AttackReleaseBehaviour : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //get the weapon and tell it to enable damage
            animator.EnterDamageState();
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //get the weapon and tell it to disable damage
            animator.ExitDamageState();
        }
    }
}