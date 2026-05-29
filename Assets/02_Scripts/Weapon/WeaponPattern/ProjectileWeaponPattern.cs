using UnityEngine;

public class ProjectileWeaponPattern : IWeaponPattern
{
    public void Excute(WeaponContext context, RunTimeWeaponlData data)
    {
        for (int i = 0; i < data.ProjectileCount; i++)
        {
            var proejectile = GameManager.Pool.GetObject(PoolType.Projectile, context.OwnerTransform);
            proejectile.GetComponent<Projectile>().Init(data);

            float t = 0;
            while (true)
            {
                if (t >= data.RepeatInterval)
                {
                    t = 0;
                    break;
                }
                else
                {
                    t += Time.deltaTime;
                }
            }

        }
    }
}
