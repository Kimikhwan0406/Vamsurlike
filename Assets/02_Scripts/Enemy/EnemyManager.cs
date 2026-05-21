using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Jobs;

public class EnemyManager : MonoBehaviour
{
    [Header("Player Caching")]
    Transform playerTransform;

    [Header("Enemy Move Job")]
    TransformAccessArray enemyTransforms;
    NativeList<float> enemySpeeds;
    JobHandle moveJobHandle;

    void Awake()
    {
        playerTransform = GameManager.Instance.GetPlayer().transform;

        enemyTransforms = new TransformAccessArray(1024);
        enemySpeeds = new NativeList<float>(1024, Allocator.Persistent);
    }

    void Update()
    {
        moveJobHandle.Complete();

        if(GameManager.EnemySpawnPool.ActivatedEnemys.Count <= 0)
            return;

        var job = new EnemyMoveJob
        {
            PlayerPosition = playerTransform.position,
            DeltaTime = Time.deltaTime,
            EnemySpeeds = enemySpeeds.AsArray()
        };

        moveJobHandle = job.Schedule(enemyTransforms);
    }

    /// <summary>
    /// EnemyBase의 id를 먼저 설정하고 등록해야 함.
    /// </summary>
    /// <param name="enemy"></param>
    public void RegisterEnemy(EnemyBase enemy)
    {
        moveJobHandle.Complete();

        enemyTransforms.Add(enemy.transform);
        enemySpeeds.Add(enemy.MoveSpeed);
    }

    public void UnregisterEnemy(int removeIndex)
    {
        moveJobHandle.Complete();

        if (removeIndex < 0 || removeIndex > enemyTransforms.length)
            return;

        enemyTransforms.RemoveAtSwapBack(removeIndex);
        enemySpeeds.RemoveAtSwapBack(removeIndex);
    }

    void LateUpdate()
    {
        moveJobHandle.Complete();
    }

    void OnDestroy()
    {
        moveJobHandle.Complete();

        if (enemyTransforms.isCreated)
            enemyTransforms.Dispose();

        if (enemySpeeds.IsCreated)
            enemySpeeds.Dispose();
    }
}

[BurstCompile]
public struct EnemyMoveJob : IJobParallelForTransform
{
    public float3 PlayerPosition;
    public float DeltaTime;

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