using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

partial struct EnemySpawnSystem : ISystem
{
    float timer;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<EnemySpawnComponent>();
        state.RequireForUpdate<PlayerTag>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EnemySpawnComponent enemySpawnComponent = SystemAPI.GetSingleton<EnemySpawnComponent>();
        timer += SystemAPI.Time.DeltaTime;

        if(timer < enemySpawnComponent.SpawnInterval)
        {
            return;
        }
        timer = 0f;

        EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);


        var random = new Random(enemySpawnComponent.RandomSeed);

        using (var enemies = state.EntityManager.Instantiate(
            enemySpawnComponent.Prefab,
            5,
            state.WorldUpdateAllocator))
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                Entity enemyEntity = enemies[i];

                // 랜덤 위치
                float angle = random.NextFloat(0, math.PI * 2); // 0 ~ 360도
                float distance = random.NextFloat(enemySpawnComponent.MinSpawnRadius, enemySpawnComponent.MaxSpawnRadius); // 10 ~ spawnRadius
                float3 position = new float3(
                    math.cos(angle) * distance,
                    math.sin(angle) * distance,
                    enemyEntity.Index * 0.001f);    // Z-Fihting 방지

                // 컴포넌트 추가

                entityCommandBuffer.SetComponent(enemyEntity, LocalTransform.FromPositionRotationScale(
                    position, quaternion.identity, 2.0f)
                    );

                var enemyComponent = new EnemyComponent
                {
                    Speed = random.NextFloat(enemySpawnComponent.MinSpeed, enemySpawnComponent.MaxSpeed),
                    CurrentHp = 100f,
                    MaxHp = 100f,
                    Damage = 0.1f
                };
                entityCommandBuffer.AddComponent(enemyEntity, enemyComponent);

                //var spawnTimeComponent = new EnemySpawnTime
                //{
                //    Value = random.NextFloat(0f, 10f)
                //};
                //entityCommandBuffer.AddComponent(enemyEntity, spawnTimeComponent);
            }
        }

        enemySpawnComponent.RandomSeed = random.NextUInt();
        SystemAPI.SetSingleton(enemySpawnComponent);

        entityCommandBuffer.Playback(state.EntityManager);
        entityCommandBuffer.Dispose();
    }
}
