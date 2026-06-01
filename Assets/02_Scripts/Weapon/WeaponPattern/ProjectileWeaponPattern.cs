using UnityEngine;

public class ProjectileWeaponPattern : IWeaponPattern
{
    public void Excute(WeaponContext context, RunTimeWeaponlData data)
    {
        for (int i = 0; i < data.ProjectileCount; i++)
        {
            var direction = GameManager.Instance.GetPlayer().GetPlayerDir;

            var proejectile = GameManager.Pool.GetObject(PoolType.Projectile, context.OwnerTransform);
            proejectile.GetComponent<Projectile>().Init(data, direction);
        }
    }
}
