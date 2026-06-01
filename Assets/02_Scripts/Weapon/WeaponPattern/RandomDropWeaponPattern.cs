using System.Collections.Generic;
using UnityEngine;

public class RandomDropWeaponPattern : IWeaponPattern
{
    List<EnemyBase> queryResults = new(32);
    List<EnemyBase> attackResults = new(32);
    List<Transform> randomTransform = new(32);

    DamageContext damageContext;

    float searchRadius = 10f;
    bool initialized = false;

    public void Excute(WeaponContext context, RunTimeWeaponlData data)
    {
        if (!initialized)
        {
            damageContext = new DamageContext
            {
                WeaponId = data.WeaponId,
                Damage = data.Damage,
            };

            initialized = true;
        }

        CheckHit(context, data);
    }

    void CheckHit(WeaponContext context, RunTimeWeaponlData data)
    {
        context.CombatQuerySystem.QueryCircle(context.OwnerTransform.position, searchRadius, queryResults);
        randomTransform.Clear();

        int totalWieght = queryResults.Count;

        if (queryResults.Count <= 0) return;

        while (randomTransform.Count < data.ProjectileCount)
        {
            int randomValue = Random.Range(0, totalWieght) + 1;

            int currentWeight = 1;

            if (currentWeight <= randomValue)
            {
                if (null == queryResults[currentWeight - 1]) continue;

                randomTransform.Add(queryResults[currentWeight - 1].transform);
            }
        }

        // O(n^2) 개선 필요
        for (int i = 0; i < randomTransform.Count; i++)
        {
            attackResults.Clear();

            context.CombatQuerySystem.QueryCircle(randomTransform[i].position, 1f * data.Range, attackResults);

            // TODO: 추후에는 weaponID에 맞는 이펙트 실행 및 Pooling 사용
            Object.Instantiate(Utils.ResourcesLoad<GameObject>("Effect/Lightning"), randomTransform[i].position, Quaternion.identity);

            foreach (var enemy in attackResults)
            {
                if (null == enemy) continue;
                enemy.TakeDamage(damageContext);
            }
        }
    }
}
