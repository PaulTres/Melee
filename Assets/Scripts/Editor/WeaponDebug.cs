using UnityEditor;
using UnityEngine;
using Weapons;
[CustomEditor(typeof(MeleeWeapon))]
public class WeaponDebug : Editor
{
    private void OnSceneGUI()
    {
        var wpn = (MeleeWeapon)target;
        Handles.color = Color.softGreen;
        var maxRotPos = Mathf.Sin(wpn.BlockAngle * Mathf.Deg2Rad) * wpn.Transform.right + Mathf.Cos(wpn.BlockAngle * Mathf.Deg2Rad) * wpn.Transform.forward;
        var minRotPos = Mathf.Sin(-wpn.BlockAngle * Mathf.Deg2Rad) * wpn.Transform.right + Mathf.Cos(-wpn.BlockAngle * Mathf.Deg2Rad) * wpn.Transform.forward;
        Handles.DrawLine(wpn.Transform.position + maxRotPos, wpn.Transform.position);
        Handles.DrawLine(wpn.Transform.position, wpn.Transform.position + minRotPos);
        Handles.DrawWireArc(wpn.Transform.position, wpn.Transform.up, minRotPos, Mathf.Abs(wpn.BlockAngle) + Mathf.Abs(wpn.BlockAngle), 1);
        var maxElevationPos = Mathf.Sin(wpn.BlockAngle * Mathf.Deg2Rad) * wpn.Transform.up + Mathf.Cos(wpn.BlockAngle * Mathf.Deg2Rad) * wpn.Transform.forward;
        var minElevationPos = Mathf.Sin(-wpn.BlockAngle * Mathf.Deg2Rad) * wpn.Transform.up + Mathf.Cos(wpn.BlockAngle * Mathf.Deg2Rad) * wpn.Transform.forward;
        Handles.DrawLine(wpn.Transform.position + maxElevationPos, wpn.Transform.position);
        Handles.DrawLine(wpn.Transform.position, wpn.Transform.position + minElevationPos);
        Handles.DrawWireArc(wpn.Transform.position, wpn.Transform.right, maxElevationPos, Mathf.Abs(wpn.BlockAngle) + Mathf.Abs(wpn.BlockAngle), 1);
    }
}
