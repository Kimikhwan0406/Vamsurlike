using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float projectileRadius = 1f;

    List<EnemyBase> hitBuffer = new();
    List<EnemyBase> alreadyHit = new();

    Vector2 dir;
    Vector3 prePosition;

    float angle;
    int hitCount = 0;
    int projectilePenetration = 1;


    void Awake()
    {
        angle = UnityEngine.Random.Range(0f, math.PI * 2f);
        dir = new Vector2(math.cos(angle), math.sin(angle));

        var randomValue = UnityEngine.Random.Range(-0.5f, 0.5f);

        prePosition = transform.position + new Vector3(0f, randomValue, 0f);
    }

    void Update()
    {
        prePosition = transform.position;

        Move();

        CheckHit();
    }

    public void Init(int projectilePenetration)
    {
        this.projectilePenetration = projectilePenetration;
    }

    void Move()
    {
        transform.Translate(dir * Time.deltaTime * 5f);
    }

    void CheckHit()
    {
        GameManager.CombatQuery.QuerySegment(prePosition, transform.position, projectileRadius, hitBuffer);

        for(int i = 0; i < hitBuffer.Count; i++)
        {
            if (alreadyHit.Contains(hitBuffer[i]))
                continue;

            alreadyHit.Add(hitBuffer[i]);
            hitBuffer[i].TakeDamage(10f);
            hitCount++;

            if (hitCount >= projectilePenetration)
                Release();
        }
    }

    void Release()
    {
        GameManager.Pool.ReturnObject(PoolType.Projectile, gameObject);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, projectileRadius);
    }
}
