using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour
{
    // TODO: 관통 횟수 체크
    // TODO: 일정 거리 이동 시 삭제

    [SerializeField] float hitRadius = 1f;
    [SerializeField] float rotateSpeed = 360f;

    Vector2 velocity;
    Vector2 acceleration;

    CombatQuerySystem combatQuerySystem;
    DamageContext damageContext;

    List<EnemyBase> queryResults = new(32);

    float hitCount;
    bool isInit = false;

    public void Init(Vector2 initialVelocity, float gravity, CombatQuerySystem combatQuerySystem, RunTimeWeaponlData data)
    {
        velocity = initialVelocity;
        acceleration = Vector2.down * gravity;

        this.combatQuerySystem = combatQuerySystem;

        hitCount = data.ProjectilePenetration;

        damageContext = new DamageContext
        {
            WeaponId = data.WeaponId,
            Damage = data.Damage,
        };

        isInit = true;
    }


    void Update()
    {
        if (!isInit || !GameManager.Instance.IsPlaying) return;

        float deltaTime = Time.deltaTime;

        velocity += acceleration * deltaTime;
        transform.position += (Vector3)(velocity * deltaTime);

        transform.Rotate(0f, 0f, rotateSpeed * deltaTime);

        CheckHit();
    }


    void CheckHit()
    {
        combatQuerySystem.QueryCircle(transform.position, hitRadius, queryResults);

        foreach (var enemy in queryResults)
        {
            enemy.TakeDamage(damageContext);
            hitCount--;

            if(hitCount <= 0)
            {
                Release();
                return;
            }
        }
    }
    
    void Release()
    {
        // TODO: 풀링에 반환
        Destroy(gameObject);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, hitRadius);
    }
}
