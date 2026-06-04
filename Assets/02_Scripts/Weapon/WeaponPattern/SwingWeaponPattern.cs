using UnityEngine;

public class SwingWeaponPattern : IWeaponPattern
{
    public void Excute(WeaponContext context, RunTimeWeaponlData data)
    {
        // 투사체 개수 만큼 연사 간격 마다 휘두르기
        for (int i = 0; i < data.ProjectileCount; i++)
        {
            // 바라보는 방향으로 휘들러야 함. -> context.OwnerTransform.right;
            // 파티클 오브젝트를 VFX풀 에서 가져와서 실행하면 끝. -> 우선 풀 없이 그냥 해보자.
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
