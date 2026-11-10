using PrimeTween;
using UnityEngine;

namespace Animation.Tweening
{
    [System.Serializable]
    public class TweenKeyFrame
    {
        public Transform Transform;
        [SerializeField] TweenSettings<Vector3> vector3Settings;
        [SerializeField] bool applyOnPosition;
        [SerializeField] TweenSettings<Quaternion> quaternionSettings;
        [SerializeField] bool applyOnRotation;
        public TweenSettings<Vector3>? Vector3Settings => applyOnPosition ? vector3Settings : null;
        public TweenSettings<Quaternion>? QuaternionSettings => applyOnRotation ? quaternionSettings : null;
    }
}
