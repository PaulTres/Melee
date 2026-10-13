using UnityEngine;

namespace Player
{
    public class PlayerEntity : EntityBase
    {
        static Camera playerCamera;
        public static Camera PlayerCamera
        {
            get
            {
                if (playerCamera == null) playerCamera = Camera.main;
                return playerCamera;
            }
        }

        protected override void Die(ObjectDestroyed @event)
        {
            throw new System.NotImplementedException();
        }
    }
}