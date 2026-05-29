using UnityEngine;

public class StageResultPresenter : IPresenter
{
    StageResultModel model;
    StageResultView view;

    public bool IsOpen => view.IsOpen;

    public void Init(IModel _model, IView _view)
    {
        model = _model as StageResultModel;
        view = _view as StageResultView;

        ResetModel();
        CreateWeaponInfo();
    }

    void CreateWeaponInfo()
    {
        var weaponList = GameManager.WeaponController.GetWeaponList();
        for(int i = 0; i < weaponList.Count; i++)
        {
            CreateResultWeaponTextSlot(weaponList[i]);
        }
    }

    void CreateResultWeaponTextSlot(WeaponObject weapon)
    {
        CombatStat stat = GameManager.CombatRecorder.GetCombatStat(weapon.WeaponId);

        var slot = Object.Instantiate(Utils.ResourcesLoad<GameObject>("UI/ResultWeaponTextSlot"));
        if(slot.TryGetComponent(out ResultWeaponTextSlot slotComponent))
        {
            slotComponent.Init(stat, weapon.WeaponLevel, Time.time - weapon.OwnedStartTime);
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

    public void ResetModel()
    {
        
    }
}
