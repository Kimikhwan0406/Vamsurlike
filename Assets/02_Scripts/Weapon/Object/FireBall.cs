using System.Collections.Generic;
using UnityEngine;

public class FireBall : AutoDespawnObject
{
    [SerializeField] float hitRadius = 1f;
    [SerializeField] Vector3 offset;

    List<EnemyBase> queryResults = new();

    CombatQuerySystem combatQuerySystem;
    DamageContext damageContext;

    Vector3 direction;

    float hitCount;
    bool isInit = false;

    public void Init(CombatQuerySystem combatQuerySystem, RunTimeWeaponlData data, Vector3 direction)
    {
        this.combatQuerySystem = combatQuerySystem;

        this.direction = direction;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 90f);

        hitCount = data.ProjectilePenetration;

        damageContext = new DamageContext
        {
            WeaponId = data.WeaponId,
            Damage = data.Damage,
        };

        isInit = true;
    }


    protected override void Update()
    {
        base.Update();

        if (!isInit || !GameManager.Instance.IsPlaying) return;

        CheckHit();
        Move();
    }

    void Move()
    {
        transform.position += direction * Time.deltaTime * 5f;
    }

    void CheckHit()
    {
        combatQuerySystem.QueryCircle(transform.position, hitRadius, queryResults);

        foreach (var enemy in queryResults)
        {
            enemy.TakeDamage(damageContext);
            hitCount--;

            if (hitCount <= 0)
            {
                Release();
                return;
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position + offset, hitRadius);
    }
}
