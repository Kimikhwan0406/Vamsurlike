using System.Data;
using UnityEngine;

public class GameManager : SingletonBehaviour<GameManager>
{
    public static UIManager UI { get { return Instance.ui; } }
    public static DataTable DataTable { get { return Instance.dataTable; } }
    public static UserDataManager UserData { get { return Instance.userData; } }
    public static EnemySystemHandler EnemySystemHandler { get { return Instance.enemySystemHandler; } }
    public static CombatQuerySystem CombatQuery { get { return Instance.combatQuerySystem; } }
    public static PlayerWeaponController WeaponController { get { return Instance.weaponController; } }
    //public static PoolManager Pool { get { return Instance.poolManager; } }
    public static CombatStatRecorder CombatRecorder { get { return Instance.combatStatRecorder; } }

    #region Variables

    [SerializeField] Transform enemyPoolTransform;
    [SerializeField] Transform objectPoolTransform;

    [Header("Managers")]
    UserDataManager userData = new();
    DataTable dataTable = new();
    UIManager ui = new();
    EnemySystemHandler enemySystemHandler = new();
    //PoolManager poolManager = new();
    PlayerWeaponController weaponController = new();
    CombatStatRecorder combatStatRecorder = new();
    CombatQuerySystem combatQuerySystem;

    [Header("Caching")]
    InGameCore inGameCore;
    EnemySystem enemySystem;
    GameObject playMap;

    [Header("InGame")]
    public bool IsPlaying => isPlaying;
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

        //poolManager.Init(objectPoolTransform);
    }

    void Update()
    {
        if (isPlaying)
        {
            inGameCore.Update();
            enemySystemHandler.Update();
            weaponController.Update();
            enemySystem.Update();
        }
    }

    void LateUpdate()
    {
        if (isPlaying)
        {
            enemySystem.LateUpdate();
        }
    }

    public Player GetPlayer() => inGameCore.Player;
    public float GetPlayTime() => inGameCore.GetPlayTime();

    public void GameOver()
    {
        PauseGame();

        UI.OpenUI<StageResultPresenter>();
    }

    public void PauseGame()
    {
        isPlaying = false;
        Debug.Log("Game Paused");
    }

    public void ResumeGame()
    {
        isPlaying = true;
        Debug.Log("Game Resumed");
    }

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

        UI.ShowIngameHUD();

        enemySystem = new();
        enemySystem.Init();
        enemySystemHandler.RegisterEnemySystemHandler(enemySystem);

        combatQuerySystem = new();
        CreateWeaponContext();

        isPlaying = true;

        weaponController.AddWeapon(baseWeapinId);
    }

    public void StageExit()
    {
        isPlaying = false;

        CameraManager.Instance.ClearFollow();

        combatStatRecorder.Release();

        weaponController.Release();
        combatQuerySystem = null;

        PoolManager.Instance.AllDespawnToPool();
        enemySystemHandler.Release();
        enemySystem.Release();
        enemySystem = null;

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
