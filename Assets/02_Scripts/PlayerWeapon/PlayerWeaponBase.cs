using UnityEngine;

public class PlayerWeaponBase : MonoBehaviour
{
    float radius = 5f;
    float damage = 5;

    void Awake()
    {
        foreach (var enemy in GameManager.CombatQuery.QueryCircle(transform.position, radius))
        {
            enemy.TakeDamage(damage);
        }

        Destroy(gameObject, 1f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
