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

    [Header("Enemy Attack Job")]
    NativeList<float> enemyPowers;
    NativeList<float> damageBuffer;
    float attackRange = 1.5f;

    void Awake()
    {
        playerTransform = GameManager.Instance.GetPlayer().transform;

        enemyTransforms = new TransformAccessArray(1024);
        enemySpeeds = new NativeList<float>(1024, Allocator.Persistent);
        enemyPowers = new NativeList<float>(1024, Allocator.Persistent);
        damageBuffer = new NativeList<float>(1024, Allocator.Persistent);
    }

    void Update()
    {
        moveJobHandle.Complete();

        if(enemyTransforms.length <= 0)
            return;

        var job = new EnemyMoveJob
        {
            PlayerPosition = playerTransform.position,
            DeltaTime = Time.deltaTime,
            AttackRange = attackRange,
            EnemySpeeds = enemySpeeds.AsArray(),
            EnemyPowers = enemyPowers.AsArray(),
            DamageBuffer = damageBuffer.AsArray(),
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

        enemyPowers.Add(enemy.Power);
        damageBuffer.Add(0f);
    }

    public void UnregisterEnemy(int removeIndex)
    {
        moveJobHandle.Complete();

        if (removeIndex < 0 || removeIndex >= enemyTransforms.length)
            return;

        enemyTransforms.RemoveAtSwapBack(removeIndex);
        enemySpeeds.RemoveAtSwapBack(removeIndex);

        enemyPowers.RemoveAtSwapBack(removeIndex);
        damageBuffer.RemoveAtSwapBack(removeIndex);
    }

    void LateUpdate()
    {
        moveJobHandle.Complete();

        float totalDamage = 0f;
        foreach (var damage in damageBuffer)
        {
            totalDamage += damage;
        }

        if (totalDamage > 0f)
        {
            GameManager.Instance.GetPlayer().TakeDamage(totalDamage);
        }
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
    public float AttackRange;

    [ReadOnly] public NativeArray<float> EnemySpeeds;
    [ReadOnly] public NativeArray<float> EnemyPowers;

    public NativeArray<float> DamageBuffer;

    public void Execute(int index, TransformAccess transform)
    {
        float3 currentEnemyPosition = transform.position;

        float3 direction = PlayerPosition - currentEnemyPosition;
        direction.z = 0f;

        float distanceSq = math.lengthsq(direction);
        direction = math.normalizesafe(direction);

        currentEnemyPosition += direction * EnemySpeeds[index] * DeltaTime;

        transform.position = currentEnemyPosition;

        // TODO 콜라이더를 사용한다면 삭제
        DamageBuffer[index] = 0f;
        if (distanceSq <= AttackRange * AttackRange)
        {
            DamageBuffer[index] = EnemyPowers[index];
        }
    }
}