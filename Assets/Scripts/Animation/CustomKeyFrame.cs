using System;
using UnityEngine;
using UnityEngine.Events;

namespace Animation
{
    [Serializable]
    public class CustomKeyFrame
    {
        public float time;
        public Transform point;
        public UnityEvent OnKeyframeReached;
    }
}