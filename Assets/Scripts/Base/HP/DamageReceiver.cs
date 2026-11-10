using EventBus;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using Weapons;

namespace HP
{
    public class DamageReceiver : ImprovedMonoBehaviour
    {
        [SerializeField] protected List<HPComponent> hpComponents = new();
        [Tooltip("This transform is used to determine the direction of incoming damage. If not set, the transform of this component will be used.")]
        [SerializeField] protected Transform body;
        [SerializeField] protected TargetType targetType;

        protected BlockComponent[] blockComponents;
        public BlockComponent[] BlockComponents
        {
            get
            {
                if (blockComponents.IsEmpty())
                {
                    blockComponents = Transform.root.GetComponentsInChildren<BlockComponent>(true);
                }
                return blockComponents;
            }
        }

        protected void Awake()
        {
            if (!EventBus<DamageInfo>.AddActions(transform.GetEntityId(), ReceiveDamage))
            {
                Debug.LogWarning($"{transform} unable to add DamageInfo actions to event bus binding.");
            }
            if (body == null) body = transform;
            Debug.Assert(!hpComponents.IsEmpty(), $"{this} has no HPComponents!");
        }

        protected void OnDestroy()
        {
            EventBus<DamageInfo>.RemoveBinding(transform.GetEntityId());
        }

        protected void OnValidate()
        {
            if (hpComponents.IsEmpty())
            {
                hpComponents ??= new();
                if (TryGetComponent<HPComponent>(out var hp))
                {
                    hpComponents.Add(hp);
                }
                else if (!TryGetComponent<Collider>(out _)) Debug.LogError($"{this} has no collider or HPComponent!");
                //if we do not have a parent, this is equivalent to the previous code block
                if (transform.parent != null && transform.root.TryGetComponent<HPComponent>(out var hproot)) hpComponents.Add(hproot);
            }
            else
            {
                var set = new HashSet<HPComponent>(hpComponents);
                if (set.Count != hpComponents.Count)
                {
                    Debug.LogError($"{this} has duplicated hp components!");
                }
            }
            if (body == null) body = transform;
        }

        protected void ReceiveDamage(DamageInfo damageInfo)
        {
            foreach (var block in blockComponents)
            {
                if (block == null || !block.Blocking) continue;
                var result = block.TryBlock(damageInfo);
                if (result == BlockResult.Failure) continue;
                if (result == BlockResult.Perfect)
                {
                    //gain a riposte
                    EventBus<RiposteEvent>.Raise(Transform.GetEntityId());
                }
                //apply stamina damage?
                return;
            }
            damageInfo.Amount = CalculateDamage(damageInfo);
            if (damageInfo.Amount <= 0) return;
            for (int i = 0; i < hpComponents.Count; i++)
            {
                hpComponents[i].TakeDamage(damageInfo);
            }
        }

        protected float CalculateDamage(DamageInfo damageInfo)
        {
            return GlobalConfig.DamageModifier(targetType, damageInfo.AttackType) * damageInfo.Amount;
        }
    }
}