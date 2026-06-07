using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Jobs;

public class EnemySystem
{
    public event Action<int, int> OnEnemyFacingChanged;

    [Header("Player Caching")]
    Transform playerTransform;

    [Header("Enemy Move Job")]
    TransformAccessArray enemyTransforms;
    NativeList<float> enemySpeeds;
    JobHandle moveJobHandle;

    [Header("Enemy Facing Job")]
    NativeQueue<EnemyFacingChange> facingChangeQueue;
    NativeList<float> enemyFacingDir;

    [Header("Enemy Attack Job")]
    NativeList<float> enemyPowers;
    NativeList<float> damageBuffer;

    // TODO attackRandge 몬스터마다 다른텐데 -> Eenemy 마다 HitRadius가 존재함.
    float attackRange = 1.5f;

    public void Init()
    {
        playerTransform = GameManager.Instance.GetPlayer().transform;

        enemyTransforms = new TransformAccessArray(1024);
        enemySpeeds = new NativeList<float>(1024, Allocator.Persistent);
        enemyPowers = new NativeList<float>(1024, Allocator.Persistent);
        damageBuffer = new NativeList<float>(1024, Allocator.Persistent);
        enemyFacingDir = new NativeList<float>(1024, Allocator.Persistent);
        facingChangeQueue = new NativeQueue<EnemyFacingChange>(Allocator.Persistent);
    }

    public void Update()
    {
        moveJobHandle.Complete();

        if (!GameManager.Instance.IsPlaying)
            return;

        if (enemyTransforms.length <= 0)
            return;

        facingChangeQueue.Clear();

        var job = new EnemyMoveJob
        {
            PlayerPosition = playerTransform.position,
            DeltaTime = Time.deltaTime,
            AttackRange = attackRange,
            EnemySpeeds = enemySpeeds.AsArray(),
            EnemyPowers = enemyPowers.AsArray(),
            DamageBuffer = damageBuffer.AsArray(),
            EnemyFacingDir = enemyFacingDir.AsArray(),
            FacingChangeQueue = facingChangeQueue.AsParallelWriter(),
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

        enemyFacingDir.Add(-1f);
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

        enemyFacingDir.RemoveAtSwapBack(removeIndex);

        return true;
    }

    void UpdateEnemyFacing()
    {
        while (facingChangeQueue.TryDequeue(out EnemyFacingChange change))
        {
            OnEnemyFacingChanged?.Invoke(change.Index, change.FacingX);
        }
    }

    public void LateUpdate()
    {
        moveJobHandle.Complete();

        if (!GameManager.Instance.IsPlaying)
            return;

        if (enemyTransforms.length <= 0)
            return;

        UpdateEnemyFacing();

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

        if (enemyFacingDir.IsCreated)
            enemyFacingDir.Dispose();

        if(facingChangeQueue.IsCreated)
            facingChangeQueue.Dispose();
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

    public NativeArray<float> EnemyFacingDir;
    public NativeArray<float> DamageBuffer;
    public NativeQueue<EnemyFacingChange>.ParallelWriter FacingChangeQueue;

    public void Execute(int index, TransformAccess transform)
    {
        float3 currentEnemyPosition = transform.position;

        float3 direction = PlayerPosition - currentEnemyPosition;
        direction.z = 0f;

        int nextFacingX = direction.x >= 0 ? 1 : -1;

        if (EnemyFacingDir[index] != nextFacingX)
        {
            EnemyFacingDir[index] = nextFacingX;

            FacingChangeQueue.Enqueue(new EnemyFacingChange
            {
                Index = index,
                FacingX = nextFacingX,
            });
        }

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