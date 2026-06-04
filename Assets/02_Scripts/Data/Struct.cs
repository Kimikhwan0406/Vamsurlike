using UnityEngine;

public struct OrbitWeaponData
{
    public Transform OwnerTransform;
    public float StartAngle;
    public float Range;

    public float Duration;
    public float Damage;
    public float RotateSpeed;
    public float HitInterval;
}

public struct EnemyHitCooldown
{
    public EnemyBase Enemy;
    public float CooldownTimer;

    public EnemyHitCooldown(EnemyBase enemy, float cooldown)
    {
        Enemy = enemy;
        CooldownTimer = cooldown;
    }
}

public struct RunTimeWeaponlData
{
    public string WeaponId;
    public int ProjectileCount;
    public int ProjectileLimits;
    public int ProjectilePenetration;   // 관통 횟수
    public float ProjectileSpeed;
    public float CoolTime;              // 공격 간격
    public float RepeatInterval;        // 한 공격 주기에서 투사체 발사 간격
    public float Damage;
    public float Range;                 // 공격 범위
    public float Knockback;
    public float Duration;
}
