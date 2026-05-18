using UnityEngine;

public class InGameCore : MonoBehaviour
{
    GameObject player;
    EnemySpawnManager enemySpawnManager;

    void Awake()
    {
        // TODO: 플레이어 소환 및 캐싱
        // TODO: 몬스터 스포너 동작?
        Init();
    }

    void Update()
    {
        GameManager.UI.GetPresenter<GameHUDPresenter, GameHUDView>().AddTime(Time.deltaTime);
    }

    void Init()
    {
        PlayerSpawn();
        EnemySpawn();
    }

    void PlayerSpawn()
    {
        player = Instantiate(Resources.Load<GameObject>("Player"));
    }

    void EnemySpawn()
    {
        if(null == player)
        {
            Debug.LogError("Player is not spawned. Enemy spawn failed.");
            return;
        }

        enemySpawnManager = new(player.transform);
    }
}
