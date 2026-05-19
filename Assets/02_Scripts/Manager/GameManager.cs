using UnityEngine;

public class GameManager : SingletonBehaviour<GameManager>
{
    public static UIManager UI { get { return Instance.ui; } }

    #region Variables
    [Header("Managers")]
    UIManager ui;
    InGameCore inGameCore;

    [Header("InGame")]
    GameObject playMap;
    bool isPlaying = false;
    #endregion

    protected override void Init()
    {
        base.Init();

        ui = new(); 
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
