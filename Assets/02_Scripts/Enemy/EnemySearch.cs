using UnityEngine;

public static class EnemySearch
{
    public static float SqrDistance(Vector3 a, Vector3 b)
    {
        float dx = a.x - b.x;
        float dy = a.y - b.y;
        return dx * dx + dy * dy;
    }

    public static bool FindCircleSearch(Vector2 aCenter, float aRadius, Vector2 bCenter, float bRadius)
    {
        float radius = aRadius + bRadius;
        return SqrDistance(aCenter, bCenter) <= radius * radius;
    }

    public static bool FindCircleSearchExcept(Vector2 aCenter, float aRadius, Vector2 bCenter, float bRadius)
    {
        float radius = aRadius + bRadius;
        return SqrDistance(aCenter, bCenter) <= radius * radius;
    }

    /// <summary>
    /// 투사체의 경우 한 프레임에 충돌된 적을 감지못하는 터널링 현상이 발생할 수 있다.
    /// 이를 방지하기 위해 투사체 이동 경로에 몬스터를 탐지하는 세그먼트 탐색이다.
    /// </summary>
    public static bool FindSegmentSerach(Vector3 start, Vector3 end, Vector3 enemyPosition, float radius)
    {
        float startX = start.x;
        float startY = start.y;
        float endX = end.x;
        float endY = end.y;
        float enemyX = enemyPosition.x;
        float enemyY = enemyPosition.y;

        float startToEnemyX = enemyX - startX;
        float startToEnemyY = enemyY - startY;

        float segmentX = endX - startX;
        float segmentY = endY - startY;
        float segmentLengthSqr = segmentX * segmentX + segmentY * segmentY;

        if(segmentLengthSqr <= 0.0001f)
        {
            float diffX = enemyX - startX;
            float diffY = enemyY - startY;
            return diffX * diffX + diffY * diffY <= radius * radius;
        }

        float delta = (startToEnemyX * segmentX + startToEnemyY * segmentY) / segmentLengthSqr;
        if (delta < 0f)
            delta = 0f;
        else if (delta > 1f)
            delta = 1f;

        float projectionX = startX + segmentX * delta;
        float projectionY = startY + segmentY * delta;

        float distanceX = enemyX - projectionX;
        float distanceY = enemyY - projectionY;
        float distanceSqr = distanceX * distanceX + distanceY * distanceY;

        return distanceSqr <= radius * radius;
    }

    public static bool FindEllipse(Vector2 enemy, Vector2 center, float radiusX, float radiusY, float enemyRadius)
    {
        float diifX = enemy.x - center.x;
        float diifY = enemy.y - center.y;

        float totalRadiusX = radiusX + enemyRadius;
        float totalRadiusY = radiusY + enemyRadius;

        float value = (diifX * diifX) / (totalRadiusX * totalRadiusX) + (diifY * diifY) / (totalRadiusY * totalRadiusY);
        return value <= 1f;
    }
}
