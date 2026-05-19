using System;
using UnityEngine;

public class GameManager : SingletonBehaviour<GameManager>
{
    public static UIManager UI { get { return Instance.ui; } }

    #region Variables
    UIManager ui;
    InGameCore inGameCore;

    bool isPlaying = false;
    #endregion

    protected override void Init()
    {
        base.Init();

        ui = new();
        ui.Init(Instantiate(Resources.Load<GameObject>("UI/UIRoot")).transform);
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
        inGameCore = new InGameCore();
        UI.ShowIngameHUD();
        isPlaying = true;
    }

    public void StageExit()
    {
        isPlaying = false;
        UI.ShowLobbyHUD();
        inGameCore = null;
    }
}
