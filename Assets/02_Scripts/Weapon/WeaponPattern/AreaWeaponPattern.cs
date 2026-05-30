using System.Collections.Generic;
using UnityEngine;

public class AreaWeaponPattern : IWeaponPattern
{
    const float areaRadius = 3f;

    List<EnemyBase> queryResults = new(32);
    List<EnemyHitCooldown> hitCooldowns = new(64);

    DamageContext damageContext;

    bool initialized = false;

    [Header("Debug")]
    public Vector3 drawCenter;

    public void Excute(WeaponContext context, RunTimeWeaponlData data)
    {
        if (!initialized)
        {
            damageContext = new DamageContext
            {
                WeaponId = data.WeaponId,
                Damage = data.Damage,
            };

            drawCenter = context.OwnerTransform.position;

            initialized = true;
        }

        float deltaTime = Time.deltaTime;

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
}
