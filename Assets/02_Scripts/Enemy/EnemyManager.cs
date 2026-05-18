using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Jobs;

public class EnemyManager : MonoBehaviour
{
    [Header("Enemy Spawn")]
    EnemySpawnManager enemySpawnManager;
    List<EnemyBase> spawnedEnemies;

    [Header("Player Caching")]
    Transform playerTransform;

    [Header("Enemy Move Job")]
    TransformAccessArray enemyTransforms;
    NativeArray<float> enemySpeeds;
    JobHandle moveJobHandle;

    void Awake()
    {
        playerTransform = GetComponent<InGameCore>().player.transform;
        enemySpawnManager = new();

        spawnedEnemies = enemySpawnManager.TestSpawnEnemy();
        enemyTransforms = new TransformAccessArray(spawnedEnemies.Count);
        enemySpeeds = new NativeArray<float>(spawnedEnemies.Count, Allocator.Persistent);
    }

    void Start()
    {
        for (int i = 0; i < spawnedEnemies.Count; i++)
        {
            enemyTransforms.Add(spawnedEnemies[i].transform);
            enemySpeeds[i] = spawnedEnemies[i].MoveSpeed;
        }   
    }

    void Update()
    {
        //moveJobHandle.Complete();

        var job = new EnemyMoveJob
        {
            PlayerPosition = playerTransform.position,
            DeltaTime = Time.deltaTime,
            EnemySpeeds = enemySpeeds
        };

        moveJobHandle = job.Schedule(enemyTransforms);
    }

    void LateUpdate()
    {
        moveJobHandle.Complete();
    }

    void OnDestroy()
    {
        enemyTransforms.Dispose();
        enemySpeeds.Dispose();
    }
}

[BurstCompile]
public struct EnemyMoveJob : IJobParallelForTransform
{
    public float3 PlayerPosition;
    public float DeltaTime;

    //public NativeArray<float3> EnemyPositions;
    [ReadOnly] public NativeArray<float> EnemySpeeds;

    public void Execute(int index, TransformAccess transform)
    {
        float3 currentEnemyPosition = transform.position;

        float3 direction = PlayerPosition - currentEnemyPosition;
        direction.z = 0f;

        direction = math.normalizesafe(direction);

        currentEnemyPosition += direction * EnemySpeeds[index] * DeltaTime;

        transform.position = currentEnemyPosition;
    }
}