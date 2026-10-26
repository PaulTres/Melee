using UnityEngine;
using Utilities;

namespace Animation
{
    public class TailWindupState : TailState
    {
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
            if (elapsedTime >= stabStartTime)
            {
                //Debug.Log("Stabbing");
                tail.ChangeState(tail.StabState);
                return;
            }
            tail.IKTarget.position = Vector3.Lerp(tail.IKTarget.position,
                tail.WindupPoint.position, elapsedTime / windupTime);
            //Debug.DrawLine(tail.IKTarget.position, tail.WindupPoint.position, Color.blue);
        }
    }
}