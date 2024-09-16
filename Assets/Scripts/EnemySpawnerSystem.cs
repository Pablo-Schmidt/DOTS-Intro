using Unity.Entities;
using Random = Unity.Mathematics.Random;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial class EnemySpawnerSystem : SystemBase
{
    private EnemySpawnerComponent enemySpawnerComponent;
    private EnemyDataContainer enemyDataContainerComponent;
    private Entity enemySpawnerEntity;
    private float nextSpawnTime;
    private Random random;

    protected override void OnCreate()
    {
        random = Random.CreateFromIndex((uint)System.DateTime.Now.Millisecond);

        nextSpawnTime = 0f;
    }

    protected override void OnUpdate()
    {
        if (!SystemAPI.TryGetSingletonEntity<EnemySpawnerComponent>(out enemySpawnerEntity))
        {
            return;
        }

        enemySpawnerComponent = EntityManager.GetComponentData<EnemySpawnerComponent>(enemySpawnerEntity);
        enemyDataContainerComponent = EntityManager.GetComponentObject<EnemyDataContainer>(enemySpawnerEntity);

        // Check if it's time to spawn a new wave of enemies
        if (SystemAPI.Time.ElapsedTime > nextSpawnTime)
        {
            SpawnWaveOfEnemies();
        }

        MoveEnemiesDownward();
    }

    private void SpawnWaveOfEnemies()
    {
        int level = 2;
        List<EnemyData> availableEnemies = new List<EnemyData>();

        foreach (EnemyData enemyData in enemyDataContainerComponent.enemies)
        {
            if (enemyData.level <= level)
            {
                availableEnemies.Add(enemyData);
            }
        }

        int index = random.NextInt(availableEnemies.Count);

        // Spawn 5 enemies in a wave
        for (int i = 0; i < 5; i++)
        {
            Entity newEnemy = EntityManager.Instantiate(availableEnemies[index].prefab);

            // Set the spawn position with a slight offset in the x direction
            float xOffset = -5f + (i * 2f); 
            EntityManager.SetComponentData(newEnemy, new LocalTransform
            {
                Position = new float3(xOffset, Camera.main.orthographicSize + 2, 0), 
                Rotation = quaternion.identity,
                Scale = 1
            });

            EntityManager.AddComponentData(newEnemy, new EnemyComponent { currentHealth = availableEnemies[index].health });
        }

        // Set the next spawn time to 3 seconds from the current time
        nextSpawnTime = (float)SystemAPI.Time.ElapsedTime + 3f;
    }

    private void MoveEnemiesDownward()
    {
        foreach (var (enemyComponent, transformComponent) in SystemAPI.Query<EnemyComponent, RefRW<LocalTransform>>())
        {
            // Move enemy downwards
            float3 position = transformComponent.ValueRW.Position;
            position.y -= SystemAPI.Time.DeltaTime * 2f; 

            transformComponent.ValueRW.Position = position;
        }
    }
}
