using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.EventSystems.EventTrigger;

public class EnemySpawnPoolManager// : SingletonBehaviour<EnemySpawnPoolManager>
{
    [Header("Enemy Pool")]
    [SerializeField] GameObject enemyPrefab;
    Transform poolTransform;
    Dictionary<string, Queue<EnemyBase>> enemyPool = new();
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

    public void Init(Transform _poolTransform)
    {
        poolTransform = _poolTransform;

        //for (int i = 0; i < 1000; i++)
        //{
        //    var newOB = GameObject.Instantiate(Utils.ResourcesLoad<GameObject>($"Enemy/Test"));
        //    float angle = UnityEngine.Random.Range(0f, math.PI * 2f);
        //    float distance = UnityEngine.Random.Range(spawnMinRadius, spawnMaxRadius);
        //    var spawnPosition = new Vector3(Mathf.Cos(angle) * distance, Mathf.Sin(angle) * distance, 0f);
        //    newOB.transform.position = spawnPosition;
        //}
    }

    public void RegisterEnemyManager(EnemyManager _enemyManager)
    {
        enemyManager = _enemyManager;
        stop = false;
    }

    public void ReleaseEnemyManager()
    {
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

        enemyManager.UnregisterEnemy(removeIndex);

        activatedEnemys[removeIndex] = lastEnemy;
        activatedEnemys.RemoveAt(lastIndex);

        enemy.SetManagerIndex(-1);

        if (lastEnemy != enemy)
        {
            lastEnemy.SetManagerIndex(removeIndex);
        }

        enemy.gameObject.SetActive(false);
        enemy.transform.SetParent(poolTransform);
        enemyPool[enemy.EnemyId].Enqueue(enemy);
    }

    public void AllDespawnEnemy()
    {
        for (int i = activatedEnemys.Count - 1; i >= 0; i--)
        {
            DespawnEnemy(activatedEnemys[i]);
        }
    }

    EnemyBase CreateNewEnemy(string createEnemyId, bool isEnqueue = false)
    {
        EnemyBase newEnemy = GameObject.Instantiate(
            Utils.ResourcesLoad<GameObject>($"Enemy/{createEnemyId}")
            , poolTransform).GetComponent<EnemyBase>();

        newEnemy.Init(createEnemyId);
        newEnemy.gameObject.SetActive(false);

        if (isEnqueue)
        {
            if (!enemyPool.ContainsKey(createEnemyId))
            {
                enemyPool.Add(createEnemyId, new Queue<EnemyBase>());
            }
            enemyPool[createEnemyId].Enqueue(newEnemy);
        }

        return newEnemy;
    }

    void PoolInit(string initEnemyId)
    {
        for (int i = 0; i < 60; i++)
        {
            CreateNewEnemy(initEnemyId, true);
        }
    }

    EnemyBase SpawnEnemy(List<string> enemyIds)
    {
        EnemyBase enemyObj = null;

        for (int i = 0; i < enemyIds.Count; i++)
        {
            if (!enemyPool.ContainsKey(enemyIds[i]))
            {
                PoolInit(enemyIds[i]);
            }

            if (enemyPool[enemyIds[i]].Count > 0)
            {
                enemyObj = enemyPool[enemyIds[i]].Dequeue();
            }
            else
            {
                enemyObj = CreateNewEnemy(enemyIds[i]);
            }

            SetPosition(enemyObj);
            enemyObj.transform.SetParent(null);
            enemyObj.gameObject.SetActive(true);

            activatedEnemys.Add(enemyObj);
            // 위에 활성 적 목록에 추가 후, 아래에서 Job을 위해 등록이 필요
            enemyObj.SetManagerIndex(activatedEnemys.Count - 1);
            enemyManager.RegisterEnemy(enemyObj);

        }

        return enemyObj;
    }

    void SetPosition(EnemyBase enemy)
    {
        float angle = UnityEngine.Random.Range(0f, math.PI * 2f);
        float distance = UnityEngine.Random.Range(spawnMinRadius, spawnMaxRadius);
        var spawnPosition = new Vector3(Mathf.Cos(angle) * distance, Mathf.Sin(angle) * distance, 0f) + GameManager.Instance.GetPlayer().transform.position;
        enemy.transform.position = spawnPosition;
    }
}
