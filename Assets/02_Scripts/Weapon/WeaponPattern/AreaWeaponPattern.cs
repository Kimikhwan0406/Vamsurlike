using System.Collections.Generic;
using UnityEngine;

public class AreaWeaponPattern : IWeaponPattern
{
    const float areaRadius = 5f;

    List<EnemyBase> queryResults = new(32);
    List<EnemyHitCooldown> hitCooldowns = new(64);

    DamageContext damageContext;

    float preRange;
    bool initialized = false;

    [Header("추후 풀링 사용해서 삭제")]
    GameObject effect;

    public void Excute(WeaponContext context, RunTimeWeaponlData data)
    {
        if (!initialized)
        {
            damageContext = new DamageContext
            {
                WeaponId = data.WeaponId,
                Damage = data.Damage,
            };

            preRange = data.Range;

            effect = Object.Instantiate(Utils.ResourcesLoad<GameObject>("Effect/Garlic"), context.OwnerTransform);
            if (effect.TryGetComponent<AreaEffect>(out var areaEffect))
            {
                areaEffect.SetRange(areaRadius * data.Range);
            }

            initialized = true;
        }

        float deltaTime = Time.deltaTime;

        UpdateEffect(context.OwnerTransform, preRange, data.Range);
        UpdateHitCooldown(deltaTime);
        CheckHit(context, data);
    }

    void CheckHit(WeaponContext context, RunTimeWeaponlData data)
    {
        context.CombatQuerySystem.QueryCircle(context.OwnerTransform.position, areaRadius * data.Range, queryResults);

        foreach (var enemy in queryResults)
        {
            if (null == enemy) continue;

            if (IsHitCooldown(enemy)) continue;

            enemy.TakeDamage(damageContext);
            hitCooldowns.Add(new EnemyHitCooldown(enemy, data.RepeatInterval));
        }
    }

    void UpdateHitCooldown(float deltaTime)
    {
        for (int i = hitCooldowns.Count - 1; i >= 0; i--)
        {
            EnemyHitCooldown cooldown = hitCooldowns[i];
            cooldown.CooldownTimer -= deltaTime;

            if (cooldown.CooldownTimer <= 0f)
            {
                hitCooldowns.RemoveAt(i);
            }
            else
            {
                hitCooldowns[i] = cooldown;
            }
        }
    }

    bool IsHitCooldown(EnemyBase enemy)
    {
        foreach (var hitCooldown in hitCooldowns)
        {
            if (enemy == hitCooldown.Enemy)
                return true;
        }

        return false;
    }

    void UpdateEffect(Transform playerPosition, float preRange, float currentRange)
    {
        // TODO 추후 Pooling 사용
        if (preRange != currentRange)
        {
            Object.Destroy(effect);
            effect = Object.Instantiate(Utils.ResourcesLoad<GameObject>("Effect/Garlic"), playerPosition);
            if(effect.TryGetComponent<AreaEffect>(out var areaEffect))
            {
                areaEffect.SetRange(areaRadius * currentRange);
            }
        }
    }
}
