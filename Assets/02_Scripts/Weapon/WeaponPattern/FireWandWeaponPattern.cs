using NSubstitute.Core;
using System.Collections.Generic;
using UnityEngine;

public class FireWandWeaponPattern : IWeaponPattern
{
    List<EnemyBase> queryResults = new();

    public void Excute(WeaponContext context, RunTimeWeaponlData data)
    {
        context.CombatQuerySystem.QueryCircle(context.OwnerTransform.position, 5f, queryResults);


        Vector3 enemyPosition;
        if (queryResults.Count != 0)
        {
            int randomIndex = Random.Range(0, queryResults.Count);
            enemyPosition = queryResults[randomIndex].transform.position;
        }
        else
        {
            float angel = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            enemyPosition = new Vector3(Mathf.Cos(angel), Mathf.Sin(angel), 0);
        }

        var direction = enemyPosition - context.OwnerTransform.position;

        var fireBall = PoolManager.Instance.SpawnFromPool<FireBall>(data.WeaponId, context.OwnerTransform.position);
        fireBall.Init(context.CombatQuerySystem, data, direction.normalized);
    }
}
