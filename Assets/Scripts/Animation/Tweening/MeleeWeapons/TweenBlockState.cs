using PrimeTween;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using Weapons;

namespace Animation.Tweening
{
    public class TweenBlockState : TweenState
    {
        [SerializeField] protected List<TweenKeyFrame> keyFrames = new();
        [SerializeField] protected BlockComponent blockComponent;

        public override Sequence BuildSequence()
        {
            var sequence = Sequence.Create();
            foreach (var keyFrame in keyFrames)
            {
                sequence = sequence.GroupKeyFrame(keyFrame);
            }
            sequence.ChainCallback(EnableBlock);
            return sequence;
        }

        public void EnableBlock()
        {
            blockComponent.StartBlocking();
        }

        public override void ExitState()
        {
            blockComponent.StopBlocking();
            base.ExitState();
        }
    }
}
