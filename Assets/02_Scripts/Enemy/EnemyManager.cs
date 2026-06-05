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

        if (!GameManager.Instance.IsPlaying)
            return;

        if (enemyTransforms.length <= 0)
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

        int expectCheckIndex = enemyTransforms.length;

        if (expectCheckIndex != enemy.ManagerIndex)
        {
            Debug.LogError("Enemy index mismatch.");
            return;
        }

        enemyTransforms.Add(enemy.transform);
        enemySpeeds.Add(enemy.MoveSpeed);

        enemyPowers.Add(enemy.Power);
        damageBuffer.Add(0f);
    }

    public bool UnregisterEnemy(int removeIndex)
    {
        moveJobHandle.Complete();

        if (removeIndex < 0 || removeIndex >= enemyTransforms.length)
        {
            Debug.Log($"{GetType()}: UnregisterEnemy return false");
            return false;
        }

        enemyTransforms.RemoveAtSwapBack(removeIndex);
        enemySpeeds.RemoveAtSwapBack(removeIndex);

        enemyPowers.RemoveAtSwapBack(removeIndex);
        damageBuffer.RemoveAtSwapBack(removeIndex);

        return true;
    }

    public void Release()
    {
        moveJobHandle.Complete();

        if (enemyTransforms.isCreated)
            enemyTransforms.Dispose();

        if (enemySpeeds.IsCreated)
            enemySpeeds.Dispose();

        if (enemyPowers.IsCreated)
            enemyPowers.Dispose();

        if (damageBuffer.IsCreated)
            damageBuffer.Dispose();
    }

    void LateUpdate()
    {
        moveJobHandle.Complete();

        if(!GameManager.Instance.IsPlaying)
            return;

        if(enemyTransforms.length <= 0)
            return;

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
        Release();
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

        DamageBuffer[index] = 0f;

        if (distanceSq <= AttackRange * AttackRange)
        {
            DamageBuffer[index] = EnemyPowers[index];
            return;
        }

        direction = math.normalizesafe(direction);
        currentEnemyPosition += direction * EnemySpeeds[index] * DeltaTime;
        transform.position = currentEnemyPosition;
    }
}