using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

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
    CancellationToken token;

    bool initializedParent = false;

    public void Init(RunTimeWeaponlData data, float angle)
    {
        transform.localRotation = Quaternion.identity;
        transform.localRotation = Quaternion.Euler(0, 0, angle);

        transform.localScale = Vector3.one * (1 + data.Range);

        radiusX += radiusX * data.Range;
        radiusY += radiusY * data.Range;

        if(!initializedParent)
        {
            gameObject.transform.SetParent(GameManager.Instance.GetPlayer().transform);

            initializedParent = true;
        }

        if (angle != 0)
        {
            offset *= -1f;
        }

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
        GameManager.CombatQuery.QueryEllipse(transform.position + offset, radiusX, radiusY, hitBuffer);

        for (int i = 0; i < hitBuffer.Count; i++)
        {
            hitBuffer[i].TakeDamage(damageContext);
        }

        ReleaseObject(token).Forget();
    }

    async UniTask ReleaseObject(CancellationToken token)
    {
        await UniTask.WaitUntil(() => !particle.IsAlive(), cancellationToken: token);
        Debug.Log("파티클 릴리즈");
        Release();
    }

    void Release()
    {
        PoolManager.Instance.DespawnToPool(this.gameObject);
    }

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
