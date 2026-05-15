using Unity.Entities;
using UnityEngine;

class EnemySpawnAuthoring : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int enemyCount = 10000;
    public float minSpeed = 0.2f;
    public float maxSpeed = 1f;
    public float minSpawnRadius = 10f;
    public float maxSpawnRadius = 50f;
    public float spawnInterval = 2f;
    public uint randomSeed = 12345;
}

class EnemySpawnAuthoringBaker : Baker<EnemySpawnAuthoring>
{
    public override void Bake(EnemySpawnAuthoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.None);

        AddComponent(entity, new EnemySpawnComponent
        {
            Prefab = GetEntity(authoring.enemyPrefab, TransformUsageFlags.Dynamic),
            EnemyCount = authoring.enemyCount,
            MinSpeed = authoring.minSpeed,
            MaxSpeed = authoring.maxSpeed,
            MinSpawnRadius = authoring.minSpawnRadius,
            MaxSpawnRadius = authoring.maxSpawnRadius,
            SpawnInterval = authoring.spawnInterval,
            RandomSeed = authoring.randomSeed
        });

        AddComponent<EnemyTag>(entity);
    }
}
