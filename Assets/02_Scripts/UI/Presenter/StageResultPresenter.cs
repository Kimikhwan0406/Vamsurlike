using System.Collections.Generic;
using UnityEngine;

public class StageResultPresenter : IPresenter
{
    StageResultModel model;
    StageResultView view;

    SortedList<float, GameObject> slots = new();

    public bool IsOpen => view.IsOpen;

    public void Init(IView _view)
    {
        model = new();
        view = _view as StageResultView;
    }

    void CreateWeaponInfo()
    {
        var weaponList = GameManager.WeaponController.GetWeaponList();
        float endTime = Time.time;

        for(int i = 0; i < weaponList.Count; i++)
        {
            CreateResultWeaponTextSlot(weaponList[i], endTime);
        }

        int index = 0;
        for(int i = slots.Count - 1; i >= 0; i--)
        {
            slots.Values[i].transform.SetSiblingIndex(index);
            index++;
        }
    }

    void CreateResultWeaponTextSlot(WeaponObject weapon, float endTime)
    {
        CombatStat stat = GameManager.CombatRecorder.GetCombatStat(weapon.WeaponId);

        float ownerTime = endTime - weapon.OwnedStartTime;

        var slot = Object.Instantiate(Utils.ResourcesLoad<GameObject>("UI/ResultWeaponTextSlot"));
        if(slot.TryGetComponent(out ResultWeaponTextSlot slotComponent))
        {
            slotComponent.Init(stat, weapon.WeaponLevel, ownerTime);
        }

        float dps = stat.TotalDamage / ownerTime;
        slots.Add(dps, slot);

        view.SetSlotParent(slot);
    }

    public void Open()
    {
        CreateWeaponInfo();
        view.Open();
    }

    public void Close()
    {
        view.Close();

        foreach(var slot in slots.Values)
        {
            Object.Destroy(slot);
        }

        slots.Clear();
    }

    public System.Type GetViewType()
    {
        return typeof(StageResultView);
    }
}
