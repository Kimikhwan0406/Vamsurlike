using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] SpriteRenderer weaponImage;
    [SerializeField] float projectileRadius = 1f;
    [SerializeField] Vector3 positionOffset;

    List<EnemyBase> hitBuffer = new();
    List<EnemyBase> alreadyHit = new();

    Vector2 dir;
    Vector3 prePosition;
    Vector3 rotatedOffset;

    DamageContext damageContext;

    float angle;
    int hitCount = 0;
    int projectilePenetration = 1;

    bool initialized = false;


    void Awake()
    {
        initialized = false;


        var randomValue = UnityEngine.Random.Range(-0.5f, 0.5f);
        prePosition = transform.position + new Vector3(0f, randomValue, 0f);
    }

    void Update()
    {
        if (!GameManager.Instance.IsPlaying)
            return;

        prePosition = transform.position;

        Move();

        CheckHit();
    }

    public void Init(RunTimeWeaponlData data, Vector2 direction)
    {
        this.projectilePenetration = data.ProjectilePenetration;

        weaponImage.sprite = Utils.ResourcesLoad<Sprite>($"Sprite/Projectile/{data.WeaponId}");

        dir = direction.normalized;

        var weaponData = GameManager.DataTable.GetWeaponData(data.WeaponId);
        positionOffset = new Vector3(weaponData.ProjectileOffset[0], weaponData.ProjectileOffset[1], 0f);
        float spriteAngle = weaponData.SpriteAngle;
        projectileRadius = weaponData.ProjectileRadius;

        float angle =  Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + spriteAngle);

        damageContext = new DamageContext
        {
            WeaponId = data.WeaponId,
            Damage = data.Damage,
        };

        initialized = true;
    }

    void Move()
    {
        transform.position += (Vector3)dir * Time.deltaTime * 10f;
    }

    void CheckHit()
    {
        if (!initialized) return;

        rotatedOffset = transform.root.rotation * positionOffset;

        GameManager.CombatQuery.QuerySegment(
            prePosition + rotatedOffset, 
            transform.position + rotatedOffset, 
            projectileRadius, 
            hitBuffer
            );

        for(int i = 0; i < hitBuffer.Count; i++)
        {
            if (alreadyHit.Contains(hitBuffer[i]))
                continue;

            alreadyHit.Add(hitBuffer[i]);
            hitBuffer[i].TakeDamage(damageContext);
            hitCount++;

            if (hitCount >= projectilePenetration)
                Release();
        }
    }

    void Release()
    {
        hitBuffer.Clear();
        alreadyHit.Clear();
        initialized = false;
        PoolManager.Instance.DespawnToPool(this.gameObject);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + rotatedOffset, projectileRadius);
    }
}
