using Unity.Mathematics;
using UnityEngine;

public class ProjectileWeaponPattern : IWeaponPattern
{
    public void Excute(WeaponContext context, WeaponData levelData)
    {
        GameManager.Pool.GetObject(PoolType.Projectile, context.OwnerTransform);

        //GameObject.Instantiate(
        //    Utils.ResourcesLoad<GameObject>("PoolObject/Projectile")
        //    , context.OwnerTransform.position
        //    , Quaternion.identity);
    }
}
