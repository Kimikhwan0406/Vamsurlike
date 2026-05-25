using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelUpPresenter : IPresenter
{
    LevelUpView view;
    LevelUpModel model;

    List<WeaponData> candidateWeapons = new();
    Dictionary<string, int> resultWeapons = new();

    List<LevelUpSlot> levelUpSlots = new();

    public bool IsOpen => view.IsOpen;

    public void Init(IModel _model, IView _view)
    {
        view = _view as LevelUpView;
        model = _model as LevelUpModel;

        SetLevelUpSlot();
    }

    public void Close()
    {
        view.Close();
        DestorySlot();
    }

    public void Open()
    {
        view.Open();
    }

    void DestorySlot()
    {
        foreach (var slot in levelUpSlots)
        {
            Object.Destroy(slot.gameObject);
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
            var randomValue = Random.Range(0, total);

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

        // 후보군은? 
        // 플레이어가 가진 무기 + 플레이어가 갖지 않은 무기

        // 1) 갖지 않은 무기는 간단하게 1레벨 무기 지급

        // 2) 가지고 있다면 현재 레벨을 체크
        // 2-1) 만렙이라면 제외
        // 2-2) 그게 아니면 레벨업에 해단되는 무기 지급

        // TODO 사실 여기도 foreach와 List의 Contains 때문에 O(n^2)인데 최적화 어케 하지?
        var maxLevellist = GameManager.WeaponController.GetMaxLevelWeapons();
        foreach (var dataKV in weaponTable)
        {
            if (maxLevellist.Contains(dataKV.Key)) continue;

            candidateWeapons.Add(dataKV.Value);
        }
    }

    void CreateLevelUpSlot(string itemId, int level)
    {
        var slot = Object.Instantiate(view.LevelUpSlotPrefab, view.LevelUpSlotGroupParent);
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
}
