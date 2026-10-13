using Animation;
using AudioSystem;
using Effects;
using EventBus;
using HybridBT2;
using System.Collections.Generic;
using UnityEngine;
namespace Utilities
{
    public static class Extensions
    {
        public static bool CanChangeState(this IState state, IState newState)
        {
            if (state == newState) return state.CanInterruptSelf;
            return state.Priority == 0 || state.Priority < newState.Priority;
        }

        public static void Play(this SoundData soundData, Vector3 position, bool createManager = true, bool requiresPlayer = true)
        {
            if (soundData != null)
            {
                var m = SoundManager.TryGetInstance(createManager);
                if (m != null)
                {
                    if (requiresPlayer)
                    {
                        if (!GameManager.Player.Active()) return;
                        var dist = Vector3.Distance(position, GameManager.Player.position);
                        if (dist > soundData.maxDistance) return;
                    }
                    m.Play(soundData, position);
                }
            }
        }

        public static void SpawnEffect(this PoolableEffect effect, Vector3 position, float scaleFactor)
        {
            SpawnEffect(effect, position, Quaternion.identity, scaleFactor);
        }

        public static void SpawnEffect(this PoolableEffect effect, Vector3 position, Quaternion rotation, float scaleFactor)
        {
            PoolableEffect.Spawn(effect, position, rotation, scaleFactor * Vector3.one);
        }

        #region NPCBase  
        /// <summary>
        /// Returns true if the NPCBase is not null, its GameObject 
        /// is active, and if it is not dying (unless allowDying is true).
        /// </summary>
        /// <param name="object"></param>
        /// <param name="allowDying"></param>
        /// <returns></returns>
        public static bool Active(this EntityBase @object, bool allowDying = false)
        {
            return @object != null && @object.Transform.gameObject.activeSelf;
        }

        /// <summary>
        /// Returns the instance ID of the ImprovedMonoBehaviour's Transform. This is used to uniquely identify the object in the scene.
        /// </summary>
        /// <param name="object"></param>
        /// <returns></returns>
        public static int GetTransformInstanceID(this EntityBase @object)
        {
            return @object.Transform.root.GetInstanceID();
        }

        public static bool IsEqual(this EntityBase @object, EntityBase other)
        {
            if (@object == null && other == null) return true;
            if (@object == null || other == null) return false;
            return @object.GetInstanceID() == other.GetInstanceID();
        }
        #endregion

        #region Collections
        public static string Output<T>(this ICollection<T> collection)
        {
            if (collection == null) return "null";
            if (collection.Count == 0) return "empty";
            var output = $"Count: {collection.Count}\n";
            foreach (var item in collection)
            {
                output += $"{item}\n";
            }
            return output;
        }

        /// <summary>
        /// Returns true if the collection is null or has no elements in it.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static bool IsEmpty<T>(this ICollection<T> collection)
        {
            return collection == null || collection.Count == 0;
        }

        /// <summary>
        /// Shuffles the element order of the specified list.
        /// </summary>
        public static void Shuffle<T>(this IList<T> ts)
        {
            var count = ts.Count;
            var last = count - 1;
            for (var i = 0; i < last; ++i)
            {
                var r = UnityEngine.Random.Range(i, count);
                (ts[r], ts[i]) = (ts[i], ts[r]);
            }
        }
        #endregion

        /// <summary>
        /// If an angle is > 180, will subtract 360. If it is < 180, will add 360.
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static float NormalizeAngle(float angle)
        {
            if (angle > 180f) angle -= 360f;
            if (angle < -180) angle += 360f;
            return angle;
        }

        /// <summary>
        /// Returns a vector that is perpendicular to the input vector. If the input vector 
        /// is zero, returns zero. If normalize is true, returns a normalized vector.
        /// </summary>
        /// <param name="inputVector">The input vector.</param>
        /// <param name="normalize">Whether to normalize the resulting vector.</param>
        /// <returns>A vector that is perpendicular to the input vector.</returns>
        public static Vector3 Perpendicular(this Vector3 inputVector, bool normalize = true)
        {
            if (inputVector == Vector3.zero) return Vector3.zero;
            Vector3 v;
            if (inputVector.z != 0)
            {
                v = new Vector3(1, 1, -(inputVector.x + inputVector.y) / inputVector.z);
            }
            else if (inputVector.x != 0)
            {
                v = new Vector3(-(inputVector.y + inputVector.z) / inputVector.x, 1, 1);
            }
            else
            {
                v = new Vector3(1, -(inputVector.x + inputVector.z) / inputVector.y, 1);
            }
            return normalize ? v.normalized : v;
        }

        #region Transform
        public static bool Active(this Transform transform)
        {
            return transform != null && transform.gameObject.activeSelf;
        }

        public static bool IsPlayer(this Transform transform)
        {
            if (transform == null) return false;
            if (!GameManager.Player.Active()) return false;
            return transform == GameManager.Player.transform.root;
        }

        public static Blackboard GetBlackboard(this Transform transform)
        {
            if (transform == null) throw new System.ArgumentNullException();
            var comp = ComponentRegister<BT>.Get(transform);
            if (comp != null)
            {
                return comp.Context;
            }
            return null;
        }

        public static float GetHP(this Transform transform, bool percentage = false)
        {
            if (transform == null) throw new System.ArgumentNullException();
            var comp = ComponentRegister<EntityBase>.Get(transform);
            if (comp == null) return 0;

            return percentage ? comp.CurrentHPPercentage : comp.CurrentHP;
        }

        public static T GetComponentViaRegister<T>(this Transform transform) where T : Component, IRegisterableComponent
        {
            if (transform == null) throw new System.ArgumentNullException();
            return ComponentRegister<T>.Get(transform);
        }

        public static T GetOrAddRegisterableComponent<T>(this Transform transform) where T : Component, IRegisterableComponent
        {
            if (transform == null) throw new System.ArgumentNullException();
            T component = ComponentRegister<T>.Get(transform);
            if (component == null)
            {
                component = transform.GetComponent<T>();
                if (component == null)
                {
                    component = transform.gameObject.AddComponent<T>();
                }
                ComponentRegister<T>.Register(transform, component);
            }
            return component;
        }

        public static T GetOrAddComponent<T>(this Transform transform) where T : Component
        {
            if (transform == null) throw new System.ArgumentNullException();
            if (!transform.TryGetComponent<T>(out var component))
            {
                component = transform.gameObject.AddComponent<T>();
            }
            return component;
        }

        /// <summary>
        /// Applies a DamageInfo to the DamageReceiver of the given transform. Returns true if 
        /// damage was applied, false otherwise.
        /// </summary>
        /// <param name="transform">The transform to apply damage to.</param>
        /// <param name="damageInfo">The DamageInfo to apply.</param>
        /// <returns>True if damage was applied, false otherwise.</returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static bool TakeDamage(this Transform transform, DamageInfo damageInfo)
        {
            if (transform == null) throw new System.ArgumentNullException();
            if (!transform.gameObject.activeSelf) throw new System.ArgumentException();
            return EventBus<DamageInfo>.Raise(transform.GetEntityId(), damageInfo);
        }
        #endregion
    }
}