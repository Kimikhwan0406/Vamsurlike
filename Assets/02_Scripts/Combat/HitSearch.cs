using UnityEngine;

public static class HitSerach
{
    public static bool Circle(Vector2 aCenter, float aRadius, Vector2 bCenter, float bRadius)
    {
        float radius = aRadius + bRadius;
        return Vector2.SqrMagnitude(aCenter - bCenter) <= radius * radius;
    }
}
