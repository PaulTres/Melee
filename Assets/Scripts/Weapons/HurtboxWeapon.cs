using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace Weapons
{
    /// <summary>
    /// This weapon relies on trigger colliders to deal damage.
    /// </summary>
    public abstract class HurtboxWeapon : WeaponBase, IResettable
    {
        protected HashSet<int> hits = new();

        public virtual void PerformReset()
        {
            hits.Clear();
        }

        protected virtual void OnEnable() => PerformReset();

        protected virtual void OnTriggerEnter(Collider collider) => EvaluateCandidate(collider.transform);

        /// <summary>
        /// Evaluate a transform. If valid, will deal damage to the transform object.
        /// </summary>
        /// <param name="hit"></param>
        protected virtual void EvaluateCandidate(Transform hit)
        {
            Debug.Log("Evaluating " + hit);
            var root = hit.root;
            var id = root.GetEntityId();
            if (id == Transform.root.GetEntityId()) return;
            if (hits.Add(id)) DealDamage(hit);
        }

        protected virtual void DealDamage(Transform transform)
        {
            transform.TakeDamage(damageInfo);
        }
    }
}