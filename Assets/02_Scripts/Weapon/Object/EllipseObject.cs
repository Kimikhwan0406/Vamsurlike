using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
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

    Vector3 positionOffset;

    List<EnemyBase> hitBuffer = new();

    DamageContext damageContext;
    CancellationToken token;

    bool initializedParent = false;

    public void Init(RunTimeWeaponlData data, int facingDir)
    {
        transform.localScale = Vector3.one * (1 + data.Range);
        transform.localScale = new Vector3(transform.localScale.x * facingDir, transform.localScale.y, transform.localScale.z);

        radiusX += radiusX * data.Range;
        radiusY += radiusY * data.Range;

        if (!initializedParent)
        {
            gameObject.transform.SetParent(GameManager.Instance.GetPlayer().transform);

            initializedParent = true;
        }

        if (facingDir == -1)
            positionOffset = offset * -1f;
        else
            positionOffset = offset;

        damageContext = new DamageContext
        {
            WeaponId = data.WeaponId,
            Damage = data.BaseDamage,
        };

        token = transform.GetCancellationTokenOnDestroy();

        CheckHit();
    }

    void CheckHit()
    {
        GameManager.CombatQuery.QueryEllipse(transform.position + positionOffset, radiusX, radiusY, hitBuffer);

        for (int i = 0; i < hitBuffer.Count; i++)
        {
            hitBuffer[i].TakeDamage(damageContext);
        }

        ReleaseObject(token).Forget();
    }

    async UniTask ReleaseObject(CancellationToken token)
    {
        await UniTask.WaitUntil(() => !particle.IsAlive(), cancellationToken: token);
        Release();
    }

    void Release()
    {
        PoolManager.Instance.DespawnToPool(this.gameObject);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        positionOffset = offset;
        Vector3 center = transform.position + positionOffset;

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
