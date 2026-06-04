using UnityEngine;

public class ProjectileWeaponPattern : IWeaponPattern
{
    public void Excute(WeaponContext context, RunTimeWeaponlData data)
    {
        for (int i = 0; i < data.ProjectileCount; i++)
        {
            var direction = GameManager.Instance.GetPlayer().GetPlayerDir;

            var proejectile = PoolManager.Instance.SpawnFromPool<Projectile>("Projectile", context.OwnerTransform.position);
            proejectile.Init(data, direction);
        }
    }
}
