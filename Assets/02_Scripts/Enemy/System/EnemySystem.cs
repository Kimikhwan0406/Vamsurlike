using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

partial struct EnemySystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        //state.RequireForUpdate<EnemyTag>();
        //state.RequireForUpdate<PlayerPositionComponent>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        Entity playerEntity = SystemAPI.GetSingletonEntity<PlayerPositionComponent>();
        var playerPositionComponent = SystemAPI.GetComponentRO<PlayerPositionComponent>(playerEntity);
        var deltaTime = SystemAPI.Time.DeltaTime;

        foreach (var (enemyTransform, enemyComponen) in
            SystemAPI.Query<RefRW<LocalTransform>, RefRO<EnemyComponent>>())
        {
            float3 direction = playerPositionComponent.ValueRO.Position - enemyTransform.ValueRO.Position;
            direction.z = 0f;

            if (math.length(direction) > 0.01f)
            {
                enemyTransform.ValueRW.Position += math.normalize(direction) * enemyComponen.ValueRO.Speed * deltaTime;
            }

            if (direction.x == 0) continue;

            float yRotation = direction.x > 0 ? 0 : math.PI;
            if (math.abs(enemyTransform.ValueRO.Rotation.value.y - yRotation) > 0.01f)
                enemyTransform.ValueRW.Rotation = quaternion.RotateY(yRotation);
        }
    }
}
