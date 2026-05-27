using System.Collections.Generic;
using UnityEngine;


// 주의점!
// 몬스터 위치 그대로 넘기는 것이 아닌, HiPosition과 HitRadius를 밀어 넣는다.

public class CombatQuerySystem
{
    public List<EnemyBase> QueryCircle(Vector2 center, float radius, List<EnemyBase> resultBuffer)
    {
        resultBuffer.Clear();

        foreach (var enemy in GameManager.EnemySpawnPool.ActivatedEnemys)
        {
            if (EnemySearch.FindCircleSearch(center, radius, enemy.HitPosition, enemy.HitRadius))
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

            if(EnemySearch.QuerySegmentSerach(start, end, enemy.HitPosition, dadius))
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
