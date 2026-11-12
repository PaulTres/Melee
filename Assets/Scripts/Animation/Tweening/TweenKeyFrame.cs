using PrimeTween;
using UnityEngine;

namespace Animation.Tweening
{
    [System.Serializable]
    public class TweenKeyFrame
    {
        public Transform Transform;
        [SerializeField] bool applyOnPosition;
        [SerializeField] TweenSettings<Vector3> vector3Settings;
        [SerializeField] bool applyOnRotation;
        [SerializeField] TweenSettings<Quaternion> quaternionSettings;
        public TweenSettings<Vector3>? Vector3Settings => applyOnPosition ? vector3Settings : null;
        public TweenSettings<Quaternion>? QuaternionSettings => applyOnRotation ? quaternionSettings : null;

        /// <summary>
        /// Draws a wire sphere at the end position of the tween if applyOnPosition is true.
        /// </summary>
        /// <param name="color"></param>
        public void DrawGizmos(Transform transform, Color color)
        {
            if (applyOnPosition)
            {
                Gizmos.color = color;
                Vector3 newPosition = transform.position +
                    transform.localScale.z * Vector3Settings.Value.endValue.z * transform.forward +
                    transform.localScale.x * Vector3Settings.Value.endValue.x * transform.right +
                    transform.localScale.y * Vector3Settings.Value.endValue.y * transform.up;
                Gizmos.DrawWireSphere(newPosition, 0.1f);
                if (applyOnRotation)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawRay(newPosition, QuaternionSettings.Value.endValue * transform.right * 0.15f);
                    Gizmos.color = Color.blue;
                    Gizmos.DrawRay(newPosition, QuaternionSettings.Value.endValue * transform.forward * 0.15f);
                    Gizmos.color = Color.green;
                    Gizmos.DrawRay(newPosition, QuaternionSettings.Value.endValue * transform.up * 0.15f);
                }
            }
        }
    }
}
