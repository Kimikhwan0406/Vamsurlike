using System.Collections.Generic;
using UnityEngine;

public class EnemySystemHandler
{
    // Job에 넘겨주기 위해 활성화된 적을 관리
    List<EnemyBase> activatedEnemys = new();
    public List<EnemyBase> ActivatedEnemys => activatedEnemys;

    [Header("Spawn Settings")]
    [SerializeField] float spawnMinRadius = 10f;
    [SerializeField] float spawnMaxRadius = 40f;
    float elapsedTime = 0f;
    const int stageIdOffset = 269001;
    bool stop = false;

    [Header("Caching")]
    EnemyManager enemyManager;

    public void RegisterEnemySystemHandler(EnemyManager _enemyManager)
    {
        enemyManager = _enemyManager;
        stop = false;
    }

    public void ReleaseEnemySystemHandler()
    {
        enemyManager.Release();
        activatedEnemys.Clear();

        stop = true;
        enemyManager = null;
    }

    public void Update()
    {
        if (stop) return;

        var min = GameManager.Instance.GetPlayTime() / 60;
        string stageId = ((int)min + stageIdOffset).ToString();

        if (elapsedTime >= GameManager.DataTable.GetStageData(stageId).SpawnInterval)
        {
            SpawnEnemy(GameManager.DataTable.GetStageData(stageId).Enemies);
            elapsedTime = 0f;
        }
        else
        {
            elapsedTime += Time.deltaTime;
        }
    }

    void SpawnEnemy(List<string> enemyIds)
    {
        for (int i = 0; i < enemyIds.Count; i++)
        {
            EnemyBase enemyObj = PoolManager.Instance.SpawnFromPool<EnemyBase>(enemyIds[i], GetRandomPosition());
            enemyObj.Init(enemyIds[i]);

            activatedEnemys.Add(enemyObj);

            // 위에 활성 적 목록에 추가 후, 아래에서 Job을 위해 등록이 필요
            enemyObj.SetManagerIndex(activatedEnemys.Count - 1);
            enemyManager.RegisterEnemy(enemyObj);
        }
    }

    public void DespawnEnemy(EnemyBase enemy)
    {
        int removeIndex = enemy.ManagerIndex;
        int lastIndex = activatedEnemys.Count - 1;

        if (removeIndex < 0 || removeIndex > lastIndex)
        {
            Debug.LogError("인덱스 범위 벗어남.");
            return;
        }

        EnemyBase lastEnemy = activatedEnemys[lastIndex];

        if (!enemyManager.UnregisterEnemy(removeIndex))
        {
            return;
        }

        activatedEnemys[removeIndex] = lastEnemy;
        activatedEnemys.RemoveAt(lastIndex);

        enemy.SetManagerIndex(-1);

        if (lastEnemy != enemy)
        {
            lastEnemy.SetManagerIndex(removeIndex);
        }

        PoolManager.Instance.DespawnToPool(enemy.gameObject);
    }

    Vector3 GetRandomPosition()
    {
        float angle = Random.Range(0f, Mathf.PI * 2f);
        float distance = Random.Range(spawnMinRadius, spawnMaxRadius);
        var spawnPosition = new Vector3(Mathf.Cos(angle) * distance, Mathf.Sin(angle) * distance, 0f) 
            + GameManager.Instance.GetPlayer().transform.position;
        
        return spawnPosition;
    }
}