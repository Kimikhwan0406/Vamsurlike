using System;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.Arm;

public class UIManager
{
    Transform UIRoot;
    GameObject lobbyHUD;

    Dictionary<Type, IPresenter> presenters = new();
    Stack<Type> viewStack = new();

    public void Init(Transform uiRoot)
    {
        UIRoot = uiRoot;
        lobbyHUD = UnityEngine.Object.Instantiate(Utils.ResourcesLoad<GameObject>("UI/LobbyCanvas"), UIRoot);
        lobbyHUD.SetActive(false);
    }

    public void OpenUI<T>(IModel _model, IPresenter _presenter) where T : IView
    {
        Type viewType = typeof(T);

        if (null == viewType)
        {
            Debug.LogError($"{viewType} is null");
            return;
        }

        if (presenters.ContainsKey(viewType))
        {
            if (presenters[viewType].IsOpen)
            {
                Debug.Log($"{viewType} is already open");
                return;
            }
        }
        else
        {
            GameObject newOB = UnityEngine.Object.Instantiate(Utils.ResourcesLoad<GameObject>($"UI/{viewType}"), UIRoot);
            if (null == newOB)
            {
                Debug.LogError($"Failed to load {viewType}");
                return;
            }
            presenters[viewType] = _presenter;
            presenters[viewType].Init(_model, newOB.GetComponent<IView>());

        }

        presenters[viewType].Open();
        viewStack.Push(viewType);
    }

    public void CloseUI()
    {
        if (viewStack.Count == 0)
        {
            Debug.Log("No UI to close");
            return;
        }

        presenters[viewStack.Pop()].Close();
    }

    public P GetPresenter<P, V>() where P : class, IPresenter where V : IView
    {
        Type viewType = typeof(V);

        if (presenters.ContainsKey(viewType))
        {
            return presenters[viewType] as P;
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
        OpenUI<GameHUDView>(new GameHUDModel(), new GameHUDPresenter());
        lobbyHUD.SetActive(false);
    }

    public bool ExistOpenUI()
    {
        if (viewStack.Count <= 0) Debug.Log("No open UI exists.");

        return viewStack.Count > 0;
    }

    public Type GetCurrentFrontUIType() => viewStack.Count > 0 ? viewStack.Peek() : null;

    public void CloseAllOepnUI()
    {
        while (viewStack.Count > 0)
        {
            CloseUI();
        }
    }
}
