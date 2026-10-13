using UnityEngine;
using Utilities;

namespace Animation
{
    public class TailWindupState : TailState
    {
        [SerializeField] protected Transform windupPosition;
        [SerializeField] protected float windupTime = 1, stabStartTime = 1.5f;

        public override void UpdateState(float deltaTime)
        {
            if (!tail.Target.Active())
            {
                tail.ChangeToDefault();
                return;
            }
            //move to the windup point and stay there for a bit
            base.UpdateState(deltaTime);
            if (elapsedTime >=stabStartTime)
            {
                tail.ChangeState(tail.StabState);
                return;
            }
            tail.IKTarget.position = Vector3.Lerp(tail.IKTarget.position, 
                tail.WindupPoint.position, elapsedTime / windupTime);
        }
    }
}