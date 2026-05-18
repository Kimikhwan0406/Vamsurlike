using Unity.Mathematics;
using UnityEngine;

public class EnemySpawnManager
{
    Transform playerTransform;
    [SerializeField] float spawnMinRadius = 10f;
    [SerializeField] float spawnMaxRadius = 50f;
    [SerializeField] float spawnCount = 10_000f;

    public EnemySpawnManager(Transform _playerTransform)
    {
        playerTransform = _playerTransform;

        if (null == playerTransform)
        {
            Debug.LogError($"{GetType()}: playerTransform is null");
            return;
        }

        TestSpawnEnemy();
    }

    void TestSpawnEnemy()
    {
        for(int i = 0; i < spawnCount; i++)
        {
            float angle = UnityEngine.Random.Range(0f, math.PI * 2f);
            float distance = UnityEngine.Random.Range(spawnMinRadius, spawnMaxRadius);
            var spawnPosition = new Vector3(Mathf.Cos(angle) * distance, Mathf.Sin(angle) * distance, i * 0.001f);

            if(Object.Instantiate(Resources.Load<GameObject>("TestEnemy"), spawnPosition, Quaternion.identity)
                .TryGetComponent<EnemyBase>(out var enemyBase))
            {
                if(null == enemyBase)
                {
                    Debug.LogError("Failed to get EnemyBase component from instantiated enemy.");
                    continue;
                }
                enemyBase.Init(playerTransform);
            }
        }
    }
}
