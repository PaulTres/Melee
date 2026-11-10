using PrimeTween;
using UnityEngine;

namespace Animation
{
    public class ArmController : ImprovedMonoBehaviour
    {
        [SerializeField] protected Transform ikTarget;
        [SerializeField] protected Transform idleReference, windupReference, releaseReference;
        [SerializeField] protected float toIdleDuration = 0.1f, windupDuration = 0.5f, releaseDuration = 0.2f, recoveryDelay = .05f, recoveryDuration = 0.5f;
        [SerializeField] protected TweenSettings<Vector3> toIdlePositionSettings, windupPositionSettings, releasePositionSettings, recoveryPositionSettings;
        [SerializeField] protected TweenSettings<Quaternion> toIdleRotationSettings, windupRotationSettings, releaseRotationSettings, recoveryRotationSettings;
        protected Sequence sequence;

        protected void KillActiveSequence()
        {
            if (sequence.isAlive)
            {
                sequence.Stop();
            }
        }

        protected void OnValidate()
        {
            if (idleReference == null || windupReference == null || releaseReference == null)
            {
                Debug.LogWarning("One or more reference transforms are not assigned. Please assign them in the inspector.");
            }
            toIdlePositionSettings = new TweenSettings<Vector3>(idleReference.localPosition, toIdleDuration);
            toIdleRotationSettings = new TweenSettings<Quaternion>(idleReference.localRotation, toIdleDuration);
            windupPositionSettings = new TweenSettings<Vector3>(windupReference.localPosition, windupDuration);
            windupRotationSettings = new TweenSettings<Quaternion>(windupReference.localRotation, windupDuration);
            releasePositionSettings = new TweenSettings<Vector3>(releaseReference.localPosition, releaseDuration);
            releaseRotationSettings = new TweenSettings<Quaternion>(releaseReference.localRotation, releaseDuration);
            recoveryPositionSettings = new TweenSettings<Vector3>(idleReference.localPosition, recoveryDuration, startDelay: recoveryDelay);
            recoveryRotationSettings = new TweenSettings<Quaternion>(idleReference.localRotation, recoveryDuration, startDelay: recoveryDelay);
        }

        protected void Awake()
        {
            toIdlePositionSettings = new TweenSettings<Vector3>(idleReference.localPosition, toIdleDuration);
            toIdleRotationSettings = new TweenSettings<Quaternion>(idleReference.localRotation, toIdleDuration);
            windupPositionSettings = new TweenSettings<Vector3>(windupReference.localPosition, windupDuration);
            windupRotationSettings = new TweenSettings<Quaternion>(windupReference.localRotation, windupDuration);
            releasePositionSettings = new TweenSettings<Vector3>(releaseReference.localPosition, releaseDuration);
            releaseRotationSettings = new TweenSettings<Quaternion>(releaseReference.localRotation, releaseDuration);
            recoveryPositionSettings = new TweenSettings<Vector3>(idleReference.localPosition, recoveryDuration, startDelay: recoveryDelay);
            recoveryRotationSettings = new TweenSettings<Quaternion>(idleReference.localRotation, recoveryDuration, startDelay: recoveryDelay);
        }

        private void Start()
        {
            GoToIdle();
        }

        public void GoToIdle()
        {
            KillActiveSequence();
            sequence = Sequence.Create(Tween.LocalPosition(ikTarget, toIdlePositionSettings)).
                Group(Tween.LocalRotation(ikTarget, toIdlePositionSettings));
        }

        protected void OnDisable()
        {
            KillActiveSequence();
        }

        [ContextMenu("Execute Stab")]
        public void ExecuteStab()
        {
            KillActiveSequence();

            sequence = Sequence.Create(Tween.LocalPosition(ikTarget, windupPositionSettings)).
                Group(Tween.LocalRotation(ikTarget, windupRotationSettings)).
                Chain(Tween.LocalPosition(ikTarget, releasePositionSettings)).
                Group(Tween.LocalRotation(ikTarget, releaseRotationSettings)).
                ChainCallback(() => Debug.Log("Stab")).
                Chain(Tween.LocalPosition(ikTarget, recoveryPositionSettings)).
                Group(Tween.LocalRotation(ikTarget, recoveryRotationSettings));
        }
    }
}