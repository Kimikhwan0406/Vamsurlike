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

    #region Handler

    public void AddExp(float exp)
    {
        model.Exp += exp;
        view.UpdateExp(model.Exp);
        // TODO: 레벨업 체크, 레벨업시 SetLevel호출
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
    }

    #endregion
}
