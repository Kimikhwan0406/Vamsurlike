using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    float angle;
    Vector2 dir;
    Vector3 prePosition;
    List<EnemyBase> hitBuffer = new();
    List<EnemyBase> alreadyHit = new();
    [SerializeField] float projectileRadius = 1f;

    void Awake()
    {
        angle = UnityEngine.Random.Range(0f, math.PI * 2f);
        dir = new Vector2(math.cos(angle), math.sin(angle));
        prePosition = transform.position;
    }
    void Update()
    {
        prePosition = transform.position;

        Move();

        CheckHit();
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
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, projectileRadius);
    }
}
