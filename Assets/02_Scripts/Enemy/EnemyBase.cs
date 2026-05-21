using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public float MoveSpeed => moveSpeed;
    public int ManagerIndex => managerIndex;
    public string EnemyId => enemyId;


    float moveSpeed;
    string enemyId;
    int managerIndex = -1;


    public void SetManagerIndex(int _managerIndex)
    {
        managerIndex = _managerIndex;
    }

    public void Init(string _enemyId)
    {
        enemyId = _enemyId;
        moveSpeed = GameManager.DataTable.GetEnemyData(enemyId).MoveSpeed * 0.025f;
    }
}
