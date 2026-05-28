using System.Data;
using UnityEngine;

public class GameManager : SingletonBehaviour<GameManager>
{
    public static UIManager UI { get { return Instance.ui; } }
    public static DataTable DataTable { get { return Instance.dataTable; } }
    public static UserDataManager UserData { get { return Instance.userData; } }
    public static EnemySpawnPoolManager EnemySpawnPool { get { return Instance.spawnPool; } }
    public static CombatQuerySystem CombatQuery { get { return Instance.combatQuerySystem; } }
    public static PlayerWeaponController WeaponController { get { return Instance.weaponController; } }
    public static PoolManager Pool { get { return Instance.poolManager; } }
    public static CombatStatRecorder CombatRecorder { get { return Instance.combatStatRecorder; } }

    #region Variables
    [SerializeField] Transform enemyPoolTransform;
    [SerializeField] Transform objectPoolTransform;


    [Header("Managers")]
    UserDataManager userData = new();
    DataTable dataTable = new();
    UIManager ui = new();
    EnemySpawnPoolManager spawnPool = new();
    PoolManager poolManager = new();
    PlayerWeaponController weaponController = new();
    CombatStatRecorder combatStatRecorder = new();
    CombatQuerySystem combatQuerySystem;

    [Header("Caching")]
    InGameCore inGameCore;
    GameObject enemyManager;

    [Header("InGame")]
    public bool IsPlaying => isPlaying;
    GameObject playMap;
    bool isPlaying = false;
    string selectedCharacterId;
    string baseWeapinId;
    #endregion

    protected override void Init()
    {
        base.Init();

        userData.LoadAllData();
        dataTable.LoadAllData();
        ui.Init(Instantiate(Utils.ResourcesLoad<GameObject>("UI/UIRoot")).transform);

        spawnPool.Init(enemyPoolTransform);
        poolManager.Init(objectPoolTransform);
    }

    void Update()
    {
        if (isPlaying)
        {
            inGameCore.Update();
            spawnPool.Update();
            weaponController.Update();
        }
    }

    public Player GetPlayer() => inGameCore.Player;
    public float GetPlayTime() => inGameCore.GetPlayTime();

    public void SetCharacterId(string characterId, string baseWeaponId)
    {
        selectedCharacterId = characterId;
        this.baseWeapinId = baseWeaponId;
    }

    public void StageEnter()
    {
        // TODO : 맵 추가시 로직 변경
        playMap = Instantiate(Utils.ResourcesLoad<GameObject>("BasicMap"));

        inGameCore = new InGameCore(selectedCharacterId);

        CreateWeaponContext();
        UI.ShowIngameHUD();

        enemyManager = Instantiate(Utils.ResourcesLoad<GameObject>("Object/EnemyManager"));
        spawnPool.RegisterEnemyManager(enemyManager.GetComponent<EnemyManager>());
        combatQuerySystem = new();


        isPlaying = true;




        weaponController.AddWeapon(baseWeapinId);


        // TEST
        //weaponController.AddWeapon("263080");
        //weaponController.RegisterWeapon("263079");
    }

    public void StageExit()
    {
        isPlaying = false;

        CameraManager.Instance.ClearFollow();

        combatStatRecorder.Release();
        weaponController.Release();
        combatQuerySystem = null;

        spawnPool.ReleaseEnemyManager();
        Destroy(enemyManager);
        enemyManager = null;

        UI.ShowLobbyHUD();
        inGameCore.Release();
        inGameCore = null;

        Destroy(playMap);
        playMap = null;
    }

    void CreateWeaponContext()
    {
        weaponController.CreateWeaponContext(
            new WeaponContext
            {
                Owner = GetPlayer().gameObject,
                OwnerTransform = GetPlayer().transform,
                CombatQuerySystem = combatQuerySystem
            });
    }
}
