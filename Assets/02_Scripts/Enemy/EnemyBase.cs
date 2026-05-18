using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public float MoveSpeed => moveSpeed;
    [SerializeField] float moveSpeed = 3f;
}
