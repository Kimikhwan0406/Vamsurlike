using System.Collections.Generic;
using UnityEngine;

public class RandomDropWeaponPattern : IWeaponPattern
{
    List<EnemyBase> queryResults = new(32);
    List<EnemyBase> attackResults = new(32);
    List<Vector3> randomDropPosition = new(32);

    DamageContext damageContext;
    WeaponData weaponData;

    const float searchRadius = 15f;
    bool initialized = false;

    public void Excute(WeaponContext context, RunTimeWeaponlData data)
    {
        if (!initialized)
        {
            damageContext = new DamageContext
            {
                WeaponId = data.WeaponId,
                Damage = data.BaseDamage,
            };

            weaponData = GameManager.DataTable.GetWeaponData(data.WeaponId);

            initialized = true;
        }

        CheckHit(context, data);
    }

    void CheckHit(WeaponContext context, RunTimeWeaponlData data)
    {
        context.CombatQuerySystem.QueryCircle(context.OwnerTransform.position, searchRadius, queryResults);
        randomDropPosition.Clear();

        if (queryResults.Count <= 0) return;

        int selectCount = Mathf.Min(data.ProjectileCount, queryResults.Count);

        // O(n)으로 개선 완료
        for(int i = 0; i < selectCount; i++)
        {
            int randomIndex = Random.Range(i, queryResults.Count);

            (queryResults[i], queryResults[randomIndex]) = (queryResults[randomIndex], queryResults[i]);

            EnemyBase enemy = queryResults[i];

            if (null == enemy) continue;

            randomDropPosition.Add(enemy.HitPosition);
        }

        // O(n^2) 개선 필요
        for (int i = 0; i < randomDropPosition.Count; i++)
        {
            Vector3 target = randomDropPosition[i];

            if(null == target) continue;


            context.CombatQuerySystem.QueryCircle(randomDropPosition[i] + 
                new Vector3(weaponData.ProjectileOffset[0], weaponData.ProjectileOffset[1], 0), 
                weaponData.ProjectileRadius * data.Range, 
                attackResults);

            PoolManager.Instance.SpawnFromPool(data.WeaponId, randomDropPosition[i]);

            foreach (var enemy in attackResults)
            {
                if (null == enemy) continue;

                enemy.TakeDamage(damageContext);
            }
        }
    }
}
