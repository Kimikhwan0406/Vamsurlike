using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpPresenter : IPresenter
{
    LevelUpView view;
    LevelUpModel model;

    List<WeaponData> candidateWeapons = new();
    Dictionary<string, int> resultWeapons = new();

    List<LevelUpSlot> levelUpSlots = new();

    public bool IsOpen => view.IsOpen;

    public void Init(IView _view)
    {
        view = _view as LevelUpView;
        model = new();

        SetLevelUpSlot();
    }

    public void Open()
    {
        view.Open();
        GameManager.Instance.PauseGame();
    }

    public void Close()
    {
        view.Close();
        DestorySlot();
        GameManager.Instance.ResumeGame();
    }

    void DestorySlot()
    {
        foreach (var slot in levelUpSlots)
        {
            slot.DestroySlot();
        }
        levelUpSlots.Clear();
    }

    void SetLevelUpSlot()
    {
        BuildCandidateWeapons();

        int total = 0;

        foreach (var data in candidateWeapons)
        {
            total += data.Rarity;
        }

        if (total <= 0)
        {
            Debug.LogError("Total Rarity is 0 or less.");
            return;
        }

        resultWeapons.Clear();
        // O(n^2)인데 최적화 못하나?
        while (resultWeapons.Count < 3)
        {
            var randomValue = UnityEngine.Random.Range(0, total);

            int currentWeight = 0;

            foreach (var data in candidateWeapons)
            {
                currentWeight += data.Rarity;

                if (randomValue < currentWeight)
                {
                    if (resultWeapons.ContainsKey(data.Id)) continue;

                    if (GameManager.WeaponController.HasWeapon(data.Id))
                    {
                        resultWeapons[data.Id] = GameManager.WeaponController.GetWeaponLevel(data.Id) + 1;
                    }
                    else
                    {
                        resultWeapons[data.Id] = 1;
                    }

                    CreateLevelUpSlot(data.Id, resultWeapons[data.Id]);
                    break;
                }
            }
        }
    }

    void BuildCandidateWeapons()
    {
        var weaponTable = GameManager.DataTable.GetWeaponDataTable();

        // TODO foreach와 List의 Contains 때문에 O(n^2)인데 최적화 어케 하지?
        var maxLevellist = GameManager.WeaponController.GetMaxLevelWeapons();
        foreach (var dataKV in weaponTable)
        {
            if (maxLevellist.Contains(dataKV.Key)) continue;

            candidateWeapons.Add(dataKV.Value);
        }
    }

    void CreateLevelUpSlot(string itemId, int level)
    {
        var slot = GameObject.Instantiate(view.LevelUpSlotPrefab, view.LevelUpSlotGroupParent);
        if (null == slot)
        {
            Debug.LogError("Instantiate Error");
            return;
        }

        if (slot.TryGetComponent(out LevelUpSlot slotComponent))
        {
            slotComponent.Init(itemId, level);
            levelUpSlots.Add(slotComponent);
        }
        else
        {
            Debug.LogError("TryGetComponent Error.");
        }
    }

    public Type GetViewType()
    {
        return typeof(LevelUpView);
    }
}
