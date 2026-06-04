using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class EllipseObject : MonoBehaviour
{
    [SerializeField] ParticleSystem particle;

    [Header("타원 설정")]
    [SerializeField] Vector3 offset;
    [SerializeField] float radiusX = 1f;
    [SerializeField] float radiusY = 1f; 
    [Range(10, 100)]
    [SerializeField] int segments = 50;

    List<EnemyBase> hitBuffer = new();

    DamageContext damageContext;

    bool initialized = false;

    public void Init(RunTimeWeaponlData data, float angle)
    {
        transform.localRotation = Quaternion.Euler(0, 0, angle);
        transform.localScale = Vector3.one * (1 + data.Range);
        radiusX += radiusX * data.Range;
        radiusY += radiusY * data.Range;

        if(angle != 0)
        {
            offset *= -1f;
        }

        damageContext = new DamageContext
        {
            WeaponId = data.WeaponId,
            Damage = data.Damage,
        };

        CheckHit();
    }

    void CheckHit()
    {
        GameManager.CombatQuery.QueryEllipse(transform.position + offset, radiusX, radiusY, hitBuffer);

        for (int i = 0; i < hitBuffer.Count; i++)
        {
            hitBuffer[i].TakeDamage(damageContext);
        }

        //if(particle.IsAlive())
        //{
        //    Destroy(gameObject);
        //}
    }

    //void Release()
    //{
    //    GameManager.Pool.ReturnObject(PoolType.Projectile, gameObject);
    //}

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Vector3 center = transform.position + offset;

        Vector3 prevPoint = center + new Vector3(Mathf.Cos(0f) * radiusX, Mathf.Sin(0f) * radiusY, 0f);

        for (int i = 1; i <= segments; i++)
        {
            float angle = (float)i / segments * Mathf.PI * 2f;

            Vector3 nextPoint = center + new Vector3(Mathf.Cos(angle) * radiusX, Mathf.Sin(angle) * radiusY, 0f);

            Gizmos.DrawLine(prevPoint, nextPoint);

            prevPoint = nextPoint;
        }
    }
}
