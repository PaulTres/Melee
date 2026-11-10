using UnityEngine;
using Utilities;

namespace Animation
{
    public class DisableMeleeDamageOnEnterBehaviour : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //get the weapon and tell it to disable damage
            animator.ExitDamageState();
        }
    }
}