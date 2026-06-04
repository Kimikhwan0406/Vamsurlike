using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


// 주의점!
// 몬스터 위치 그대로 넘기는 것이 아닌, HiPosition과 HitRadius를 밀어 넣는다.

public class CombatQuerySystem
{
    public List<EnemyBase> QueryCircle(Vector2 center, float radius, List<EnemyBase> resultBuffer)
    {
        resultBuffer.Clear();

        foreach (var enemy in GameManager.EnemySystemHandler.ActivatedEnemys)
        {
            if (!IsValidEnemy(enemy))
                continue;

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

        foreach (var enemy in GameManager.EnemySystemHandler.ActivatedEnemys)
        {
            if (!IsValidEnemy(enemy))
                continue;

            float radius = enemy.HitRadius + projectileRadius;

            if(EnemySearch.FindSegmentSerach(start, end, enemy.HitPosition, radius))
            {
                resultBuffer.Add(enemy);
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="center"> 타원 중심 </param>
    public void QueryEllipse(Vector2 center, float radiusX, float radiusY, List<EnemyBase> resultBuffer)
    {
        resultBuffer.Clear();

        foreach (var enemy in GameManager.EnemySystemHandler.ActivatedEnemys)
        {
            if (!IsValidEnemy(enemy))
                continue;

            if (EnemySearch.FindEllipse(enemy.HitPosition, center, radiusX, radiusY, enemy.HitRadius))
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
