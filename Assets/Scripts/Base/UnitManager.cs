using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using Utilities;

public static class UnitManager
{
    /// <summary>
    /// All active entities. The key is an entity's team.
    /// </summary>
    readonly static Dictionary<int, List<EntityBase>> entities = new();
    public static int AllyCount => entities[GlobalConfig.PlayerTeam].Count;
    public static int EnemyCount => entities[GlobalConfig.EnemyTeam].Count;

    public static void Clear() => entities.Clear();

    public static IEnumerable<EntityBase> GetAllEntities()
    {
        return entities.SelectMany(kv => kv.Value);
    }

    /// <summary>
    /// Get all the ships belonging to a specific team.
    /// </summary>
    /// <param name="team"></param>
    /// <returns></returns>
    public static List<EntityBase> GetEntities(int team)
    {
        if (entities.TryGetValue(team, out var value)) return value;
        return new();
    }

    public static void Register(EntityBase entity)
    {
        if (entity == null)
        {
            Debug.LogError("Cannot register null entity");
            return;
        }
        int team = entity.Team;
        if (!entities.ContainsKey(team))
        {
            entities[team] = new();
        }
        //Debug.Log($"{ship.Transform} registered on team {team}.");
        entities[team].Add(entity);
        entity.UnitManagerIndex = entities[team].Count - 1;
        entity.SetObjectLayer();
        //Debug.Log($"Registered unit {ship.Transform} with index {ship.UnitManagerIndex}.");
    }

    /// <summary>
    /// Remove the entity from the list of entities and set its index to -1, otherwise 
    /// it can try to unregister again and that fucks with everything.
    /// </summary>
    /// <param name="entity"></param>
    public static void Unregister(EntityBase entity)
    {
        if (!entity.Active(true) || entity.UnitManagerIndex == -1) return;
        int team = entity.Team;
        if (entities.ContainsKey(team))
        {
            int index = entity.UnitManagerIndex;
            entities[team].RemoveAtSwapBack(entity.UnitManagerIndex);
            entity.UnitManagerIndex = -1;
            if (entities[team].Count > index) entities[team][index].UnitManagerIndex = index;
        }
    }
}
