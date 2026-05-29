using System.Collections.Generic;
using UnityEngine;

public class GameHUDPresenter : IPresenter
{
    GameHUDModel model;
    GameHUDView view;

    public Dictionary<string, GameObject> HudWeaponSlots { get; set; } = new();

    public bool IsOpen => view.IsOpen;

    public void Init(IView _view)
    {
        model = new();
        view = _view as GameHUDView;

        if (model == null || view == null)
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
    public int GetGold() => model.Gold;
    public int GetLevel() => model.Level;
    public int GetEnemyCount() => model.EnemyCount;


    public void AddExp(float exp)
    {
        model.Exp += exp;
        view.UpdateExp(model.Exp / model.MaxExp);

        if (model.Exp >= model.MaxExp)
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

    public void AddHUDWeaponSlot(string weaponId)
    {
        var slot = CreateHUDWeaponSlot(weaponId);

        HudWeaponSlots.Add(weaponId, slot);
        view.AddHUDWeaponSlot(slot);
    }

    public void RemoveHUDWeaponSlot(string weaponId)
    {
        if (HudWeaponSlots.TryGetValue(weaponId, out var slot))
        {
            view.RemoveHUDWeaponSlot(slot);
            HudWeaponSlots.Remove(weaponId);
        }
    }

    GameObject CreateHUDWeaponSlot(string weaponId)
    {
        GameObject slot = Object.Instantiate(Utils.ResourcesLoad<GameObject>("UI/HudWeaponSlot"));

        if (slot.TryGetComponent(out HudWeaponSlot slotComponent))
        {
            slotComponent.Init(weaponId);
        }
        else
        {
            Debug.LogError("HudWeaponSlot component not found.");
        }

        return slot;
    }

    void SetLevel(int level)
    {
        model.Level = level;
        view.UpdateLevel(model.Level);

        SetMaxExp();

        GameManager.UI.OpenUI<LevelUpPresenter>();

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

    public System.Type GetViewType()
    {
        return typeof(GameHUDView);
    }
}
