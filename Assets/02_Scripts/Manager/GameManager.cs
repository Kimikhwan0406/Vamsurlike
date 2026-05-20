using System.Data;
using UnityEngine;

public class GameManager : SingletonBehaviour<GameManager>
{
    public static UIManager UI { get { return Instance.ui; } }
    public static DataTable DataTable { get { return Instance.dataTable; } }
    public static UserDataManager UserData { get { return Instance.userData; } }

    #region Variables

    [Header("Managers")]
    UserDataManager userData = new();
    DataTable dataTable = new();
    UIManager ui = new();
    InGameCore inGameCore;

    [Header("InGame")]
    GameObject playMap;
    bool isPlaying = false;

    #endregion

    protected override void Init()
    {
        base.Init();


        userData.LoadAllData();
        dataTable.LoadAllData();
        ui.Init(Instantiate(Utils.ResourcesLoad<GameObject>("UI/UIRoot")).transform);
    }

    void Update()
    {
        if(isPlaying)
        {
            inGameCore.Update();
        }
    }

    public GameObject GetPlayer() => inGameCore.Player;

    public void StageEnter()
    {
        // TODO : 맵 추가시 로직 변경
        playMap = Instantiate(Utils.ResourcesLoad<GameObject>("BasicMap"));

        inGameCore = new InGameCore();
        UI.ShowIngameHUD();
        isPlaying = true;
    }

    public void StageExit()
    {
        isPlaying = false;
        UI.ShowLobbyHUD();
        inGameCore.Release();
        inGameCore = null;

        Destroy(playMap);
        playMap = null;
    }
}
