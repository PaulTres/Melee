using Timers;
using UnityEngine;

public class StaminaComponent : ImprovedMonoBehaviour, IResettable
{
    [field: SerializeField] public float MaxStamina { get; protected set; }
    protected float stamina;
    public float Stamina
    {
        get => stamina;
        protected set
        {
            stamina = Mathf.Clamp(value, 0, MaxStamina);
        }
    }
    [Tooltip("Stamina regeneration amount.")]
    [field: SerializeField] public float StaminaRegen { get; protected set; }
    [Tooltip("Time that needs to pass without taking damage for staminas to begin regenerating.")]
    [field: SerializeField] public float StaminaRegenCD { get; protected set; }
    [Tooltip("Time between stamina value regenerating.")]
    [field: SerializeField] public float StaminaRegenInterval { get; protected set; }
    protected IntervalTimer regenTimer;
    protected CountdownTimer staminaCDTimer;

    protected virtual void Awake()
    {
        regenTimer = new IntervalTimer(Mathf.Infinity, StaminaRegenInterval);
        regenTimer.OnInterval += () =>
        {
            Stamina += StaminaRegen;
            if (Stamina >= MaxStamina)
            {
                regenTimer.Stop();
            }
        };

        staminaCDTimer = new CountdownTimer(StaminaRegenCD);
        staminaCDTimer.OnTimerStop += () => regenTimer.Start();
    }
    private void OnEnable()
    {
        PerformReset();
    }

    public void PerformReset()
    {
        Stamina = MaxStamina;
    }

    public void DrainStamina(float amount)
    {
        if (amount <= 0) return;
        Stamina -= amount;
        regenTimer.Stop();
        staminaCDTimer.Start();
    }

    protected void OnDestroy()
    {
        regenTimer?.Dispose();
        staminaCDTimer?.Dispose();
    }
}
