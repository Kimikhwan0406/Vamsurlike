using System.Collections.Generic;
using UnityEngine;

public class FireWandWeaponPattern : IWeaponPattern
{
    List<EnemyBase> queryResults = new();

    Vector3 centerDirection;
    float spreadAngle = 35f;    // 부채꼴 범위

    public void Excute(WeaponContext context, RunTimeWeaponlData data)
    {
        context.CombatQuerySystem.QueryCircle(context.OwnerTransform.position, 10f, queryResults);


        Vector3 enemyPosition;
        if (queryResults.Count != 0)
        {
            int randomIndex = Random.Range(0, queryResults.Count);
            enemyPosition = queryResults[randomIndex].transform.position;

            centerDirection = enemyPosition - context.OwnerTransform.position;
        }
        else
        {
            float angel = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            centerDirection = new Vector3(Mathf.Cos(angel), Mathf.Sin(angel), 0);
        }

        centerDirection.z = 0;
        centerDirection.Normalize();

        if (data.ProjectileCount == 1)
        {
            var fireBall = PoolManager.Instance.SpawnFromPool<FireBall>(data.WeaponId, context.OwnerTransform.position);
            fireBall.Init(context.CombatQuerySystem, data, centerDirection);
        }
        else
        {
            Vector3 startDirection = Quaternion.AngleAxis(-spreadAngle * 0.5f, Vector3.forward) * centerDirection;
            float angleStep = spreadAngle / (data.ProjectileCount - 1);

            for (int i = 0; i < data.ProjectileCount; i++)
            {
                Vector3 direction = Quaternion.AngleAxis(angleStep * i, Vector3.forward) * startDirection;
                direction.z = 0;

                var fireBall = PoolManager.Instance.SpawnFromPool<FireBall>(data.WeaponId, context.OwnerTransform.position);
                fireBall.Init(context.CombatQuerySystem, data, direction.normalized);
            }
        }
    }
}
