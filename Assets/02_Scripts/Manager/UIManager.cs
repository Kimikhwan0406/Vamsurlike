using System;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.Arm;

public class UIManager
{
    Transform UIRoot;
    GameObject lobbyHUD;

    Dictionary<Type, IPresenter> presenterDic = new();

    public void Init(Transform uiRoot)
    {
        UIRoot = uiRoot;
        lobbyHUD = UnityEngine.Object.Instantiate(Utils.ResourcesLoad<GameObject>("UI/LobbyCanvas"), UIRoot);
        lobbyHUD.SetActive(false);
    }

    /// <summary>
    /// </summary>
    /// <typeparam name="T"> : view </typeparam>
    /// <param name="_model"></param>
    /// <param name="_presenter"></param>
    public void OpenUI<T>() where T : IPresenter, new()
    {
        Type presenterType = typeof(T);

        if (presenterDic.ContainsKey(presenterType))
        {
            if (presenterDic[presenterType].IsOpen)
            {
                Debug.Log($"{presenterType} is already open");
                return;
            }
        }
        else
        {
            var newPresenter = new T();
            GameObject newOB = UnityEngine.Object.Instantiate(
                Utils.ResourcesLoad<GameObject>($"UI/{newPresenter.GetViewType()}"), UIRoot);

            if (null == newOB)
            {
                Debug.LogError($"Failed to load {newPresenter.GetViewType()}");
                return;
            }
            presenterDic[presenterType] = newPresenter;
            presenterDic[presenterType].Init(newOB.GetComponent<IView>());
        }

        presenterDic[presenterType].Open();
    }

    public void CloseUI<T>() where T : IPresenter
    {
        if (presenterDic.Count == 0)
        {
            Debug.Log("No UI to close");
            return;
        }

        presenterDic[typeof(T)].Close();
    }

    public T GetPresenter<T>() where T : class, IPresenter
    {
        Type viewType = typeof(T);

        if (presenterDic.TryGetValue(viewType, out var presenter))
        {
            return presenter as T;
        }

        Debug.Log($"{viewType} is not exist");
        return null;
    }

    public void ShowLobbyHUD()
    {
        CloseAllOepnUI();
        lobbyHUD.SetActive(true);
    }

    public void ShowIngameHUD()
    {
        OpenUI<GameHUDPresenter>();
        lobbyHUD.SetActive(false);
    }

    public void CloseAllOepnUI()
    {
        foreach(var presenter in presenterDic.Values)
        {
            if (presenter.IsOpen)
                presenter.Close();
        }
    }
}
