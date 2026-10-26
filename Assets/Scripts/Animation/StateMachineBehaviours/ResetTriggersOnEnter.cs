using UnityEngine;

namespace Animation
{
    public class ResetTriggersOnEnter : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            foreach(var param in animator.parameters)
            {
                if(param.type==AnimatorControllerParameterType.Trigger)
                {
                    animator.ResetTrigger(param.nameHash);
                }
            }
        }
    }
}