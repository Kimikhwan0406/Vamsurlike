using Unity.Entities;

public struct EnemySpawnComponent : IComponentData
{
    public Entity Prefab;
    public int EnemyCount;
    public float MinSpeed;
    public float MaxSpeed;
    public float MinSpawnRadius;
    public float MaxSpawnRadius;
    public float SpawnInterval;
    public uint RandomSeed;
}

public struct EnemySpawnTag : IComponentData { }