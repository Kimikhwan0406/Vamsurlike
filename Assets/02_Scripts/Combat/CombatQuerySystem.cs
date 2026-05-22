using System.Collections.Generic;
using UnityEngine;

public class CombatQuerySystem
{
    private readonly List<EnemyBase> resultBuffer = new();

    public List<EnemyBase> QueryCircle(Vector2 center, float radius)
    {
        resultBuffer.Clear();

        foreach (var enemy in GameManager.EnemySpawnPool.ActivatedEnemys)
        {
            if (HitSerach.Circle(center, radius, enemy.transform.position, enemy.HitRadius))
            {
                resultBuffer.Add(enemy);
            }
        }

        return resultBuffer;
    }

    public static bool SegmentCircle( Vector2 start, Vector2 end, Vector2 circleCenter, float radius)
    {
        Vector2 segment = end - start;
        float segmentLengthSqr = segment.sqrMagnitude;

        if (segmentLengthSqr <= 0.00001f)
        {
            return (circleCenter - start).sqrMagnitude <= radius * radius;
        }

        float t = Vector2.Dot(circleCenter - start, segment) / segmentLengthSqr;
        t = Mathf.Clamp01(t);

        Vector2 closestPoint = start + segment * t;

        return (circleCenter - closestPoint).sqrMagnitude <= radius * radius;
    }
}
