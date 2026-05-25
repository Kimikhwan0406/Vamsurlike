using Unity.Mathematics;
using UnityEngine;

public class ProjectileWeaponPattern : IWeaponPattern
{
    public void Excute(WeaponContext context, WeaponData levelData)
    {
        GameObject.Instantiate(
            Utils.ResourcesLoad<GameObject>("TestWeapon")
            , context.OwnerTransform.position
            , Quaternion.identity);
    }
}
