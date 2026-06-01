using UnityEngine;

public class SwingWeaponPattern : IWeaponPattern
{
    public void Excute(WeaponContext context, RunTimeWeaponlData data)
    {
        // 투사체 개수 만큼 연사 간격 마다 휘두르기

        /// 1. 게임 오브젝트 생성 - 오브젝트와 닿이면 데미지 => 마땅한 이미지가 없음 -> 휘두르는 걸 표현 못함
        /// 2. 이펙트 생성 - 이펙트 생성과 동시에 데미지 => 편함 대신 휘두르기 이펙트가 닿지 않았는데 데미지가 들어갈 수 있음
        /// 3. 이펙트 생성 - 이펙트 생성과 데미지를 분리 => 위 단점을 보완, 어떻게 이펙트가 닿았는지 판별하는가?

        /// 0. 근데 휘두르기 영역만큼 어떻게 몬스터를 검사하지?

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
