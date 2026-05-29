
public class OrbitWeaponPattern : IWeaponPattern
{
    public void Excute(WeaponContext context, RunTimeWeaponlData data)
    {
        int count = data.ProjectileCount;
        if (count <= 0) return;

        for (int i = 0; i < count; i++)
        {
            float angle = 360f / count * i;

            OrbitWeaponObject orbit = GameManager.Pool.GetObject(PoolType.Orbit, context.OwnerTransform)
                .GetComponent<OrbitWeaponObject>();

            orbit.Init(data.WeaponId, new OrbitWeaponData
            {
                OwnerTransform = context.OwnerTransform,
                StartAngle = angle,
                Range = data.Range,
                Duration = data.Duration,
                Damage = data.Damage,
                RotateSpeed = data.ProjectileSpeed,
                HitInterval = data.RepeatInterval,
            });
        }
    }
}
