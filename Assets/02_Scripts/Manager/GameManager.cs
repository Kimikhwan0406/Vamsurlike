using System.Data;
using UnityEngine;

public class GameManager : SingletonBehaviour<GameManager>
{
    public static UIManager UI { get { return Instance.ui; } }
    public static DataTable DataTable { get { return Instance.dataTable; } }
    public static UserDataManager UserData { get { return Instance.userData; } }
    public static EnemySpawnPoolManager EnemySpawnPool { get { return Instance.spawnPool; } }
    public static CombatQuerySystem CombatQuery { get { return Instance.combatQuerySystem; } }

    #region Variables
    [SerializeField] Transform poolTransform;

    [Header("Managers")]
    UserDataManager userData = new();
    DataTable dataTable = new();
    UIManager ui = new();
    EnemySpawnPoolManager spawnPool = new();
    CombatQuerySystem combatQuerySystem;

    [Header("Caching")]
    InGameCore inGameCore;
    GameObject enemyManager;

    [Header("InGame")]
    public bool IsPlaying => isPlaying;
    GameObject playMap;
    bool isPlaying = false;
    string selectedCharacterId;
    #endregion

    protected override void Init()
    {
        base.Init();

        userData.LoadAllData();
        dataTable.LoadAllData();
        ui.Init(Instantiate(Utils.ResourcesLoad<GameObject>("UI/UIRoot")).transform);

        spawnPool.Init(poolTransform);
    }

    void Update()
    {
        if (isPlaying)
        {
            inGameCore.Update();
            spawnPool.Update();
        }
    }

    public Player GetPlayer() => inGameCore.Player;
    public float GetPlayTime() => inGameCore.GetPlayTime();

    public void SetCharacterId(string characterId)
    {
        selectedCharacterId = characterId;
    }

    public void StageEnter()
    {
        // TODO : 맵 추가시 로직 변경
        playMap = Instantiate(Utils.ResourcesLoad<GameObject>("BasicMap"));

        inGameCore = new InGameCore(selectedCharacterId);
        UI.ShowIngameHUD();
        isPlaying = true;

        enemyManager = Instantiate(Utils.ResourcesLoad<GameObject>("Object/EnemyManager"));
        spawnPool.RegisterEnemyManager(enemyManager.GetComponent<EnemyManager>());
        combatQuerySystem = new();
    }

    public void StageExit()
    {
        combatQuerySystem = null;

        spawnPool.ReleaseEnemyManager();
        Destroy(enemyManager);
        enemyManager = null;

        isPlaying = false;
        UI.ShowLobbyHUD();
        inGameCore.Release();
        inGameCore = null;

        Destroy(playMap);
        playMap = null;
    }
}
