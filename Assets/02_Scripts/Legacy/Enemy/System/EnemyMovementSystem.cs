using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

partial struct EnemyMovementSystem : ISystem
{
    Entity playerEntity;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<EnemyTag>();
        state.RequireForUpdate<PlayerPositionComponent>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if (playerEntity == Entity.Null || !SystemAPI.Exists(playerEntity))
            playerEntity = SystemAPI.GetSingletonEntity<PlayerPositionComponent>();

        var playerPositionComponent = SystemAPI.GetComponent<PlayerPositionComponent>(playerEntity);

        state.Dependency = new EnemyMovementJob
        {
            DeltaTime = SystemAPI.Time.DeltaTime,
            PlayerPosition = playerPositionComponent.Position,
        }.ScheduleParallel(state.Dependency);
    }
}

[BurstCompile]
public partial struct EnemyMovementJob : IJobEntity
{
    public float DeltaTime;
    public float3 PlayerPosition;

    public void Execute(ref EnemyComponent enemyComponent, ref LocalTransform enemyTransform, Entity entity)
    {
        float3 direction = PlayerPosition - enemyTransform.Position;
        direction.z = 0f;

        if (math.length(direction) > 0.01f)
        {
            enemyTransform.Position += math.normalize(direction) * enemyComponent.Speed * DeltaTime;
        }

        if (direction.x == 0) return;

        float yRotation = direction.x < 0 ? 0 : math.PI;
        if (math.abs(enemyTransform.Rotation.value.y - yRotation) > 0.01f)
            enemyTransform.Rotation = quaternion.RotateY(yRotation);
    }
}