using UnityEngine;

public class GameHUDPresenter : IPresenter
{
    GameHUDModel model;
    GameHUDView view;

    public bool IsOpen => view.IsOpen;

    public void Init(IModel _model, IView _view)
    {
        model = _model as GameHUDModel;
        view = _view as GameHUDView;

        if(model == null || view == null)
        {
            Debug.LogError("InGameHUDPresenter Init Failed");
            return;
        }

        model.Level = 1;
        model.MaxExp = 5f;
    }

    public void Open()
    {
        view.Open();
    }
    
    public void Close()
    {
        view.Close();
        view = null;
        model = null;
    }

    public float GetPlayTime() => model.Time;


    public void AddExp(float exp)
    {
        model.Exp += exp;
        view.UpdateExp(model.Exp / model.MaxExp);
        
        if(model.Exp >= model.MaxExp)
        {
            model.Exp -= model.MaxExp;
            SetLevel(model.Level + 1);
            view.UpdateExp(model.Exp / model.MaxExp);
        }
    }

    public void AddTime(float time)
    {
        model.Time += time;
        view.UpdateTime(model.Time);
    }

    public void AddGold(int gold)
    {
        model.Gold += gold;
        view.UpdateGold(model.Gold);
    }

    public void AddEnemyCount(int enemyCount)
    {
        model.EnemyCount += enemyCount;
        view.UpdateEnemyCount(model.EnemyCount);
    }


    void SetLevel(int level)
    {
        model.Level = level;
        view.UpdateLevel(model.Level);

        SetMaxExp();
    }

    void SetMaxExp()
    {
        if (model.Level >= 2)
            model.MaxExp += 10;
        else if (model.Level >= 21)
            model.MaxExp += 13;
        else if (model.Level >= 41)
            model.MaxExp += 16;
    }
}
