using EventBus;
using HP;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

[RequireComponent(typeof(HPComponent))]
public abstract class EntityBase : ImprovedMonoBehaviour, IRegisterableComponent
{
    /// <summary>
    /// This is used for quick removal from the unit manager lists. 
    /// DO NOT FUCK WITH THIS!
    /// </summary>
    public int UnitManagerIndex { get; set; } = -1;
    [field: SerializeField] public int Team { get; set; } = 2;
    protected HPComponent hpComponent;
    /// <summary>
    /// HPComponent for this EntityBase. Lazily loaded from the Transform.
    /// </summary>
    public HPComponent HPComponent
    {
        get
        {
            if (hpComponent == null) hpComponent = Transform.GetComponent<HPComponent>();
            return hpComponent;
        }
    }
    public float CurrentHP => HPComponent.CurrentHP;
    public float CurrentHPPercentage => HPComponent.CurrentHP / HPComponent.MaxHP;
    [field: SerializeField] public bool IsDying { get; protected set; } = false;
    [field: SerializeField] public bool Untargetable { get; set; } = false;
    [SerializeField] protected Collider[] colliders;

    #region Setup
    protected virtual void OnValidate()
    {
        if (colliders.IsEmpty())
        {
            colliders = GetComponentsInChildren<Collider>();
        }
    }

    protected virtual void Awake()
    {
        ComponentRegister<EntityBase>.Register(transform.root, this);
        EventBus<ObjectDestroyed>.AddActions(Transform.GetEntityId(), Die);
    }

    protected virtual void OnEnable()
    {
        UnitManager.Register(this);
    }

    public void SetObjectLayer()
    {
        var layer = GlobalConfig.GetLayer(Team);
        foreach (var col in colliders)
        {
            col.gameObject.layer = layer;
        }
    }

    protected virtual void OnDisable()
    {
        UnitManager.Unregister(this);
    }

    protected virtual void OnDestroy()
    {
        ComponentRegister<EntityBase>.Unregister(transform.root);
        EventBus<ObjectDestroyed>.RemoveBinding(Transform.GetEntityId());
    }
    #endregion

    #region Functionality
    public List<EntityBase> GetEnemies()
    {
        if (Team == GlobalConfig.EnemyTeam) return UnitManager.GetEntities(GlobalConfig.PlayerTeam);
        if (Team == GlobalConfig.PlayerTeam) return UnitManager.GetEntities(GlobalConfig.EnemyTeam);
        Debug.LogWarning($"{this} is on team {Team}, which is invalid!");
        return null;
    }

    protected abstract void Die(ObjectDestroyed @event);
    #endregion
}
