using PrimeTween;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
namespace Animation.Tweening
{
    public class TweenStabReleaseState : TweenDamageState
    {
        [SerializeField] protected List<TweenKeyFrame> keyFrames = new();
        [SerializeField] protected float recoveryDelay = 0.5f;

        public override Sequence BuildSequence()
        {
            var sequence = base.BuildSequence();
            foreach (var keyFrame in keyFrames)
            {
                sequence = sequence.GroupKeyFrame(keyFrame);
            }
            sequence = sequence.ChainCallback(DisableDamage);
            if(recoveryDelay > 0f)
            {
                sequence = sequence.ChainDelay(recoveryDelay);
            }
            return sequence;
        }
    }
}