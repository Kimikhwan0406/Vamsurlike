using UnityEngine;

public class SwingWeaponPattern : IWeaponPattern
{
    public void Excute(WeaponContext context, RunTimeWeaponlData data)
    {
        for (int i = 0; i < data.ProjectileCount; i++)
        {
            float angle = 180f * (i + 1);

            var effect = PoolManager.Instance.SpawnFromPool<EllipseObject>(data.WeaponId, context.OwnerTransform.position);
            effect.GetComponent<EllipseObject>().Init(data, angle);
        }
    }
}
