using UnityEngine;

public class SwingWeaponPattern : IWeaponPattern
{
    public void Excute(WeaponContext context, RunTimeWeaponlData data)
    {
        for (int i = 0; i < data.ProjectileCount; i++)
        {
            float angle = (360f / data.ProjectileCount) * i;

            var effect = Object.Instantiate(Utils.ResourcesLoad<GameObject>("Effect/Whip"), context.OwnerTransform);
            if(null == effect)
            {
                Debug.Log("SwingWeaponPattern : Effect/Whip null");
                return;
            }
            effect.GetComponent<EllipseObject>().Init(data, angle);

        }
    }
}
