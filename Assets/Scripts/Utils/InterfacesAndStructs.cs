using AudioSystem;
using EventBus;
using UnityEngine;
using Weapons;

public interface IResettable
{
    void PerformReset();
}

#region Event bus stuff
/// <summary>
/// Fires when an entity's attack should be interrupted due to damage.
/// </summary>
public class AttackInterruptEvent : IEvent { }

/// <summary>
/// Wrapper for transmitting damage information.
/// </summary>
public struct DamageInfo : IEvent
{
    public AttackType AttackType { get; set; }
    public float Amount { get; set; }
    public Transform Source { get; set; }
    public Vector3 Position { get; set; }
}

/// <summary>
/// Fires when an HPComponent reaches 0 hp. Should only be used to trigger death and level events.
/// </summary>
public readonly struct ObjectDestroyed : IEvent
{
    public readonly Transform Killer;
    public readonly int VictimID;

    public ObjectDestroyed(int victimID, Transform killer)
    {
        VictimID = victimID;
        Killer = killer;
        //Debug.Log(Killer == null);
    }

    public override string ToString()
    {
        return "Victim " + VictimID + " was destroyed by " + (Killer != null ? Killer.name : "nothing") + '.';
    }
}

public readonly struct ShowText : IEvent
{
    public readonly Vector2 AnchorMax, AnchorMin;
    public readonly Vector3 AnchoredPosition;
    public readonly string Text;
    public readonly Color TextColor;
    public readonly float LifeTime;
    public readonly SoundData Sound;

    public ShowText(Vector3 anchoredPosition, float lifeTime, string text, Color color, SoundData sound = null)
    {
        AnchoredPosition = anchoredPosition;
        LifeTime = lifeTime;
        AnchorMax = new(0.5f, 0.5f);
        AnchorMin = new(0.5f, 0.5f);
        Text = text;
        TextColor = color;
        Sound = sound;
    }
}
#endregion