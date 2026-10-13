using System;
using UnityEngine;
namespace Utilities
{
    public sealed class GlobalUpdater : Singleton<GlobalUpdater>
    {
        Action<float> update = delegate { }, lateUpdate = delegate { }, fixedUpdate = delegate { };

        protected override void ClearSingleton()
        {
            base.ClearSingleton();
            update = fixedUpdate = lateUpdate = null;
        }

        public static void RegisterUpdate(Action<float> action)
        {
            TryGetInstance(true).update += action;
        }

        public static void DeregisterUpdate(Action<float> action)
        {
            if (HasInstance) instance.update -= action;
        }

        public static void RegisterLateUpdate(Action<float> action)
        {
            TryGetInstance(true).lateUpdate += action;
        }

        public static void DeregisterLateUpdate(Action<float> action)
        {
            if (HasInstance) instance.lateUpdate -= action;
        }

        public static void RegisterFixedUpdate(Action<float> action)
        {
            TryGetInstance(true).fixedUpdate += action;
        }

        public static void DeregisterFixedUpdate(Action<float> action)
        {
            if (HasInstance) instance.fixedUpdate -= action;
        }

        private void Update()
        {
            update(Time.deltaTime);
        }

        private void LateUpdate()
        {
            lateUpdate(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            fixedUpdate(Time.fixedDeltaTime);
        }
    }
}