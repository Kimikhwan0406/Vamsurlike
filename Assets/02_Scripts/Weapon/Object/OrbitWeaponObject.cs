using System.Collections.Generic;
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

public class OrbitWeaponObject : MonoBehaviour
{
    List<EnemyBase> queryResults = new(32);
    List<EnemyHitCooldown> hitCooldowns = new(64);

    OrbitWeaponData data;
    DamageContext damageContext;

    float durationTimer;
    float angle;

    [Tooltip("baseDistance는 플레이러 부터의 떨어진 거리, radius는 무기의 반지름")]
    [Header("Distance, Radius")]
    [SerializeField] float baseDistance = 5f;
    [SerializeField] float radius = 1f;
    float totalDistance;

    [Header("Rotate Speed")]
    [SerializeField] float baseSpeed = 10f;
    float totalRotateSpeed;

    public void Init(string weaponId, OrbitWeaponData data)
    {
        queryResults.Clear();
        hitCooldowns.Clear();

        this.data = data;

        angle = data.StartAngle;
        durationTimer = data.Duration;
        totalDistance = baseDistance * data.Range;
        totalRotateSpeed = baseSpeed * data.RotateSpeed;

        transform.SetParent(data.OwnerTransform);
        transform.localPosition = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * totalDistance;

        damageContext = new DamageContext
        {
            WeaponId = weaponId,
            Damage = data.Damage,
        };
    }

    void Update()
    {
        float deltaTime = Time.deltaTime;
        durationTimer -= deltaTime;

        if (durationTimer <= 0f)
        {
            Release();
            return;
        }

        UpdateHitCooldown(deltaTime);
        Roation(deltaTime);
        CheckHit();
    }

    void Roation(float deltaTime)
    {
        angle += deltaTime * totalRotateSpeed; //* data.RotateSpeed;

        float radian = angle * Mathf.Deg2Rad;

        transform.localPosition = new Vector3(Mathf.Cos(radian), Mathf.Sin(radian), 0f) * totalDistance;
    }

    void CheckHit()
    {
        GameManager.CombatQuery.QueryCircle(transform.position, radius, queryResults);

        foreach (var enemy in queryResults)
        {
            if (null == enemy) continue;

            if (IsHitCooldown(enemy)) continue;

            enemy.TakeDamage(damageContext);
            hitCooldowns.Add(new EnemyHitCooldown(enemy, data.HitInterval));
        }
    }

    void UpdateHitCooldown(float deltaTime)
    {
        for(int i = hitCooldowns.Count - 1; i >= 0; i--)
        {
            EnemyHitCooldown cooldown = hitCooldowns[i];
            cooldown.CooldownTimer -= deltaTime;

            if(cooldown.CooldownTimer <= 0f)
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

    void Release()
    {
        GameManager.Pool.ReturnObject(PoolType.Orbit, gameObject);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
