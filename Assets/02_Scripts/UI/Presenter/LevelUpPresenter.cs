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
    }

    public void Open()
    {
        SetLevelUpSlot();

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


        while (resultWeapons.Count < 3)
        {
            if (GameManager.WeaponController.GetWeaponCount() >= 6)
                break;

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

        if(resultWeapons.Count < 3)
        {
            // 후보 무기가 3개 미만인 경우, 남은 슬롯을 어떤 슬롯으로 채워하는데
            // 현재 임시로 그냥 생성 X
        }
    }

    void BuildCandidateWeapons()
    {
        candidateWeapons.Clear();

        var weaponTable = GameManager.DataTable.GetWeaponDataTable();


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
