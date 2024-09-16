using UnityEngine;
using Unity.Entities;
using System.Collections.Generic;
using Unity.Mathematics;


public class EnemySpawnerAuthoring : MonoBehaviour
{
    public float spawnCooldown = 10;
    public List<EnemySO> enemiesSO;

    public class EnemySpawnerBaker : Baker<EnemySpawnerAuthoring>
    {
        public override void Bake(EnemySpawnerAuthoring authoring)
        { 
        Entity enemySpawnerAuthoring = GetEntity(TransformUsageFlags.None);

            AddComponent(enemySpawnerAuthoring, new EnemySpawnerComponent
            {
            spawnCooldown = authoring.spawnCooldown
    });

    List<EnemyData> enemyData = new List<EnemyData>();

    foreach(EnemySO e in authoring.enemiesSO)
    {
        enemyData.Add(new EnemyData
        {
            damage = e.damage,
            health = e.health,
            level = e.level,
            moveSpeed = e.moveSpeed,
            prefab = GetEntity(e.prefab, TransformUsageFlags.None)

        });
    }

    AddComponentObject(enemySpawnerAuthoring, new EnemyDataContainer {enemies = enemyData});
        
        }
    }
}