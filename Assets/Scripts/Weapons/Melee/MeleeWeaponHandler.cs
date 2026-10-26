using EventBus;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utilities;

namespace Weapons
{
    public class MeleeWeaponHandler : ImprovedMonoBehaviour
    {
        [SerializeField] protected List<MeleeWeapon> weapons;

        protected virtual void OnValidate()
        {
            if (weapons.IsEmpty()) weapons = GetComponentsInChildren<MeleeWeapon>(true).ToList();
        }

        protected void OnEnable()
        {
            EventBus<MeleeWeapon.EnterDamageState>.AddActions(Transform.GetEntityId(), ChangeWeaponState);
            EventBus<MeleeWeapon.ExitDamageState>.AddActions(Transform.GetEntityId(), ChangeWeaponState);
        }

        protected void OnDisable()
        {
            EventBus<MeleeWeapon.EnterDamageState>.RemoveActions(Transform.GetEntityId(), ChangeWeaponState);
            EventBus<MeleeWeapon.ExitDamageState>.RemoveActions(Transform.GetEntityId(), ChangeWeaponState);
        }

        public void ChangeWeaponState(MeleeWeapon.ExitDamageState @event)
        {
            if (weapons.IsEmpty()) Debug.LogWarning($"{this} has no weapons!");
            for (int i = 0; i < weapons.Count; i++) weapons[i].DisableDamage();
        }

        public void ChangeWeaponState(MeleeWeapon.EnterDamageState @event)
        {
            if (weapons.IsEmpty()) Debug.LogWarning($"{this} has no weapons!");
            for (int i = 0; i < weapons.Count; i++) weapons[i].EnableDamage();
        }
    }
}