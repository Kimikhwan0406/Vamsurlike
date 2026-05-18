using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] Transform playerTransform;

    [SerializeField] float moveSpeed = 3f;
    Vector3 direction;

    bool isLive = true;

    void Update()
    {
        if (!isLive) return;

        Move();
    }

    public void Init(Transform _playerTransform)
    {
        playerTransform = _playerTransform;
    }

    void Move()
    {
        if (null == playerTransform)
        {
            return;
        }

        direction = (playerTransform.position - transform.position).normalized;
        direction.z = 0f;

        transform.position += direction * moveSpeed * Time.deltaTime;
    }
}
