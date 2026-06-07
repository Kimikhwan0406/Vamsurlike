using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class EnemyBase : MonoBehaviour//, IPoolObject
{
    public float MoveSpeed => moveSpeed;
    public int ManagerIndex => managerIndex;
    public string EnemyId => enemyId;
    public float Power => power;
    public Vector3 HitPosition => transform.position + hitPositionOffset;
    public float HitRadius => hitRadius;
    public bool IsDead => isDead;

    [SerializeField] Transform enemyModel;
    SpriteRenderer[] spriteRenderers;
    CancellationToken token;

    string enemyId;
    float health;
    float power;
    float xp;
    float moveSpeed;
    int managerIndex = -1;
    bool isDead = false;
    [SerializeField] Vector3 hitPositionOffset;
    [SerializeField] float hitRadius;

    int facingX = 1;
    [SerializeField] bool defaultFacesRight = false;

    void Awake()
    {
        token = this.GetCancellationTokenOnDestroy();
        spriteRenderers = enemyModel.GetComponentsInChildren<SpriteRenderer>();
    }

    public void SetManagerIndex(int _managerIndex)
    {
        managerIndex = _managerIndex;
    }

    public void Init(string _enemyId)
    {
        enemyId = _enemyId;

        var enemyData = GameManager.DataTable.GetEnemyData(enemyId);

        moveSpeed = enemyData.MoveSpeed * 0.025f;
        health = enemyData.MaxHealth;
        power = enemyData.Power;
        xp = enemyData.XP;

        isDead = false;
    }

    public void SetFacingX(int dirX)
    {
        if (dirX == 0)
            return;

        dirX = defaultFacesRight ? dirX : -dirX;

        if (facingX == dirX)
            return;

        facingX = dirX;

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * facingX;
        transform.localScale = scale;

        hitPositionOffset.x = Mathf.Abs(hitPositionOffset.x) * facingX;
    }

    public void TakeDamage(DamageContext context)
    {
        if(isDead)
        {
            return;
        }

        float befoeHp = health;

        health -= context.Damage;

        foreach (var spriteRenderer in spriteRenderers)
        {
            spriteRenderer.color = Color.red;
        }
        SetSpriteColor(token).Forget();

        float takeDamage = Mathf.Min(befoeHp, context.Damage);
        GameManager.CombatRecorder.AddDamage(context.WeaponId, takeDamage);

        if (health <= 0)
        {
            Die();
        }
    }

    async UniTask SetSpriteColor(CancellationToken token)
    {
        await UniTask.Delay(System.TimeSpan.FromSeconds(0.2), cancellationToken: token);
        foreach (var spriteRenderer in spriteRenderers)
        {
            spriteRenderer.color = Color.white;
        }
    }


    void Die()
    {
        GameManager.UI.GetPresenter<GameHUDPresenter>().AddEnemyCount(1);

        var e = PoolManager.Instance.SpawnFromPool<FieldObject>("FieldObject", gameObject.transform.position);
        e.Init(xp);

        isDead = true;
        Release();
    }

    void Release()
    {
        GameManager.EnemySystemHandler.DespawnEnemy(this);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = isDead ? Color.blue : Color.red;
        Gizmos.DrawWireSphere(HitPosition, hitRadius);
    }
}
