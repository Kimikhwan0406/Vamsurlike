using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletonBehaviour<UIManager>
{
    [SerializeField] GameObject lobbyHUD;

    Dictionary<Type, IPresenter> presenters = new();
    Stack<Type> viewStack = new();

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
            GameObject newOB = Instantiate(Resources.Load<GameObject>($"UI/{viewType}"), this.transform);
            if (null == newOB)
            {
                Debug.LogError($"Failed to load {viewType}");
                return;
            }
            _presenter.Init(_model, newOB.GetComponent<IView>());
            presenters[viewType] = _presenter;
        }

        _presenter.Open();
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

    public IPresenter GetPresenter<T>(out bool closed) where T : IView
    {
        Type viewType = typeof(T);
        closed = false;

        if (presenters.ContainsKey(viewType))
        {
            closed = !presenters[viewType].IsOpen;
            return presenters[viewType];
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
