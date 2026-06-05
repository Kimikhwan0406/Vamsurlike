
using UnityEngine;

public class OrbitWeaponPattern : IWeaponPattern
{
    int direction;

    public OrbitWeaponPattern(int direction = 1)
    {
        this.direction = direction;
    }

    public void Excute(WeaponContext context, RunTimeWeaponlData data)
    {
        int count = data.ProjectileCount;
        if (count <= 0) return;

        for (int i = 0; i < count; i++)
        {
            float angle = 360f / count * i;

            var orbit 
                = PoolManager.Instance.SpawnFromPool<OrbitWeaponObject>(data.WeaponId, context.OwnerTransform.position);


            orbit.Init(direction, data.WeaponId, new OrbitWeaponData
            {
                OwnerTransform = context.OwnerTransform,
                StartAngle = angle,
                Range = data.Range,
                Duration = data.Duration,
                Damage = data.BaseDamage,
                RotateSpeed = data.ProjectileSpeed,
                HitInterval = data.RepeatInterval,
            });

        }
    }
}
