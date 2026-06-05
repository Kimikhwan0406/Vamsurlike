using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class StageResultPresenter : IPresenter
{
    StageResultModel model;
    StageResultView view;

    List<KeyValuePair<float, GameObject>> slots = new();

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

        slots.Sort((a, b) => b.Key.CompareTo(a.Key));

        for(int i = 0; i < slots.Count; i++)
        {
            slots[i].Value.transform.SetSiblingIndex(i);
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
        slots.Add(new (dps, slot));

        view.SetSlotParent(slot);
    }

    public void Open()
    {
        CreateWeaponInfo();
        view.Open();

        view.OnClose += Close;
    }

    public void Close()
    {
        view.Close();

        foreach(var slotKV in slots)
        {
            Object.Destroy(slotKV.Value.gameObject);
        }

        slots.Clear();

        view.OnClose -= Close;
    }

    public System.Type GetViewType()
    {
        return typeof(StageResultView);
    }
}
