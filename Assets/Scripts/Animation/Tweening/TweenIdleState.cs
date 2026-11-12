using PrimeTween;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace Animation.Tweening
{
    public class TweenIdleState : TweenState
    {
        [SerializeField] protected List<TweenKeyFrame> keyFrames = new();

        public override Sequence BuildSequence()
        {
            var sequence = Sequence.Create();
            foreach (var keyFrame in keyFrames)
            {
                sequence = sequence.GroupKeyFrame(keyFrame);
            }
            return sequence;
        }
    }
}
