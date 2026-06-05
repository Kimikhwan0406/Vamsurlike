using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectPresenter : IPresenter
{
    CharacterSelectView view;
    CharacterSelectModel model;

    Dictionary<string, CharacterSlot> characterSlots { get; set; } = new();

    public bool IsOpen => view.IsOpen;

    public void Init(IView _view)
    {
        model = new();
        view = _view as CharacterSelectView;
        SetCharacterSlot();
    }

    public void Open()
    {
        view.Open();
        view.OnStartGame += OnClickEnterStage;
    }

    public void Close()
    {
        view.Close();
        view.OnStartGame -= OnClickEnterStage;
    }

    void OnClickEnterStage()
    {
        GameManager.Instance.SetCharacterId(model.CharacterId, model.BaseWeaponId);
        GameManager.Instance.StageEnter();
    }

    void SetCharacterSlot()
    {
        var dataTable = GameManager.DataTable.GetCharacterDataTable();
        foreach (var dataKV in dataTable)
        {
            var data = dataKV.Value;
            if (null == data) continue;

            CreateCharacterSlot(data.Id, data.DefaultWeapon);
        }
    }

    void CreateCharacterSlot(string characterId, string weaponId)
    {
        GameObject slot = Object.Instantiate(view.CharacterSlotPrefab, view.CharacterSlotGroupParent);
        if (null == slot)
        {
            Debug.LogError($"Failed to create character slot for characterId: {characterId}");
            return;
        }

        if (slot.TryGetComponent(out CharacterSlot slotComponent))
        {
            slotComponent.Init(characterId, weaponId, OnClickCharacterSlot);
            characterSlots.Add(characterId, slotComponent);
        }
        else
        {
            Debug.LogError($"Character slot prefab does not contain CharacterSlot component.");
        }
    }

    void OnClickCharacterSlot(string characterId, string baseWeaponId)
    {
        model.CharacterId = characterId;
        model.BaseWeaponId = baseWeaponId;
        view.UpdateSelectedCharacterInfo(characterId);

        foreach (var slotKV in characterSlots)
        {
            var slot = slotKV.Value;
            bool isSelected = slotKV.Key == characterId;
            slot.ChangeSelectedSlot(isSelected);
        }
    }

    public System.Type GetViewType()
    {
        return typeof(CharacterSelectView);
    }
}
