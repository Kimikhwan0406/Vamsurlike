using System.Collections.Generic;
using UnityEngine;

public class CombatQuerySystem
{
    public List<EnemyBase> QueryCircle(Vector2 center, float radius, List<EnemyBase> resultBuffer)
    {
        resultBuffer.Clear();

        foreach (var enemy in GameManager.EnemySpawnPool.ActivatedEnemys)
        {
            if (EnemySearch.FindCircleSearch(center, radius, enemy.transform.position, enemy.HitRadius))
            {
                resultBuffer.Add(enemy);
            }
        }

        return resultBuffer;
    }

    public void QuerySegment(Vector2 start, Vector2 end, float projectileRadius, List<EnemyBase> resultBuffer)
    {
        resultBuffer.Clear();

        foreach (var enemy in GameManager.EnemySpawnPool.ActivatedEnemys)
        {
            if (!IsValidEnemy(enemy))
                continue;

            float dadius = enemy.HitRadius + projectileRadius;

            if(EnemySearch.QuerySegmentSerach(start, end, enemy.transform.position, dadius))
            {
                resultBuffer.Add(enemy);
            }
        }
    }

    static bool IsValidEnemy(EnemyBase enemy)
    {
        return !enemy.IsDead;
    }
}
