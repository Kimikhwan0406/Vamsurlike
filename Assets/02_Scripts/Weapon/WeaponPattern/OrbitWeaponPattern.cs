using Unity.VisualScripting;
using UnityEngine;

public class OrbitWeaponPattern : IWeaponPattern
{
    public void Excute(WeaponContext context, WeaponData weaponData)
    {
        // TODO 플레이어 중심으로 원형으로 회전
        // King Bible: CoolTime마다 소환되어 Duration동안 회전 
        // 몇개? 반지름 회전 속도

        int count = weaponData.ProjectileCount;
        if (count <= 0) return;

        for (int i = 0; i < count; i++)
        {
            float angle = 360f / count * i;

            OrbitWeaponObject orbit = GameManager.Pool.GetObject(PoolType.Orbit, context.OwnerTransform)
                .GetComponent<OrbitWeaponObject>();

            orbit.Init(new OrbitWeaponData
            {
                OwnerTransform = context.OwnerTransform,
                StartAngle = angle,
                Range = weaponData.Range,
                Duration = weaponData.Duration,
                Damage = weaponData.BaseDamage,
                RotateSpeed = weaponData.ProjectileSpeed,
                HitInterval = weaponData.RepeatInterval,
            });
        }
    }
}
