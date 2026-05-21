using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public float MoveSpeed => moveSpeed;
    public int ManagerIndex => managerIndex;
    public string EnemyId => enemyId;

    string enemyId;
    float health;
    float power;
    float xp;
    float moveSpeed;
    int managerIndex = -1;


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
    }
}
