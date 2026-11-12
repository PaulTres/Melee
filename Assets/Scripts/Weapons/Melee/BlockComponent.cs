using UnityEngine;
using Utilities;

namespace Weapons
{
    public class BlockComponent : ImprovedMonoBehaviour, IResettable
    {
        protected static float parryWindowExtension = .2f;
        [SerializeField] protected float parryStaminaModifier = .1f;
        [SerializeField] protected float parryWindow = .5f;
        [SerializeField] protected float blockAngle;
        [SerializeField] protected float blockStaminaCost = .1f, blockStaminaInitialCost = .2f;
        protected StaminaComponent staminaComponent;
        public StaminaComponent StaminaComponent
        {
            get
            {
                if (staminaComponent == null) staminaComponent = GetComponentInParent<StaminaComponent>();
                return staminaComponent;
            }
        }
        [SerializeField] protected bool blocking;
        protected float blockStartTime;
        public bool Blocking => blocking;

        protected virtual void OnEnable()
        {
            PerformReset();
        }

        public void PerformReset()
        {
            blocking = false;
            blockStartTime = -parryWindow;
        }

        public void PerformFixedUpdate(float dt)
        {
            if (!Blocking) return;
            //drain stamina if blocking
            if (StaminaComponent != null)
            {
                StaminaComponent.DrainStamina(dt * blockStaminaCost);
                if (StaminaComponent.Stamina <= 0) StopBlocking();
            }
        }

        public void StartBlocking()
        {
            if (Blocking) return;
            if (StaminaComponent != null)
            {
                if (StaminaComponent.Stamina < blockStaminaInitialCost) return;
                StaminaComponent.DrainStamina(blockStaminaInitialCost);
                GlobalUpdater.RegisterFixedUpdate(PerformFixedUpdate);
            }
            else blocking = true;
        }


        public void StopBlocking()
        {
            if (!Blocking) return;
            blocking = false;
            if (StaminaComponent != null)
            {
                GlobalUpdater.DeregisterFixedUpdate(PerformFixedUpdate);
            }
        }

        public BlockResult TryBlock(DamageInfo damageInfo)
        {
            if (!Blocking) return BlockResult.Failure;
            var angle = Vector3.Angle(Transform.forward, damageInfo.Position - position);
            if (angle > blockAngle) return BlockResult.Failure;
            //we are blocking
            var result = BlockResult.Success;
            float modifier = 1f;
            if (Time.time - blockStartTime <= parryWindow)
            {
                blockStartTime += parryWindowExtension;
                modifier = parryStaminaModifier;
                result = BlockResult.Perfect;
            }
            if (StaminaComponent != null)
            {
                StaminaComponent.DrainStamina(damageInfo.StaminaDrain * modifier);
            }
            return result;
        }
    }
}