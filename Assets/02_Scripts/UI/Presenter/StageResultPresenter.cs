using UnityEngine;

public class StageResultPresenter : IPresenter
{
    StageResultModel model;
    StageResultView view;

    public bool IsOpen => view.IsOpen;

    public void Init(IView _view)
    {
        model = new();
        view = _view as StageResultView;

        CreateWeaponInfo();
    }

    void CreateWeaponInfo()
    {
        var weaponList = GameManager.WeaponController.GetWeaponList();
        float endTime = Time.time;

        for(int i = 0; i < weaponList.Count; i++)
        {
            CreateResultWeaponTextSlot(weaponList[i], endTime);
        }
    }

    void CreateResultWeaponTextSlot(WeaponObject weapon, float endTime)
    {
        CombatStat stat = GameManager.CombatRecorder.GetCombatStat(weapon.WeaponId);

        var slot = Object.Instantiate(Utils.ResourcesLoad<GameObject>("UI/ResultWeaponTextSlot"));
        if(slot.TryGetComponent(out ResultWeaponTextSlot slotComponent))
        {
            slotComponent.Init(stat, weapon.WeaponLevel, endTime - weapon.OwnedStartTime);
        }

        view.SetSlotParent(slot);
    }

    public void Open()
    {
        view.Open();
    }

    public void Close()
    {
        view.Close();
    }

    public System.Type GetViewType()
    {
        return typeof(StageResultView);
    }
}
