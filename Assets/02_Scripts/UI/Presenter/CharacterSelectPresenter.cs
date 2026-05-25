using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectPresenter : IPresenter
{
    public Dictionary<string, CharacterSlot> characterSlots { get; private set; } = new();

    CharacterSelectView view;
    CharacterSelectModel model;

    public bool IsOpen => view.IsOpen;

    public void Init(IModel _model, IView _view)
    {
        model = _model as CharacterSelectModel;
        view = _view as CharacterSelectView;

        SetCharacterSlot();
    }

    public void OnClickCharacterSlot(string characterId)
    {
        model.CharacterId = characterId;
        view.UpdateSelectedCharacterInfo(characterId);
        Debug.Log($"Selected character: {characterId}");
    }

    public void OnClickEnterStage()
    {
        GameManager.Instance.SetCharacterId(model.CharacterId);
        GameManager.UI.CloseUI();
        GameManager.Instance.StageEnter();
    }

    public void Open()
    {
        view.Open();
        view.StartGameButton.onClick.AddListener(OnClickEnterStage);
    }

    public void Close()
    {
        view.Close();
        view.StartGameButton.onClick.RemoveAllListeners();
        view = null;
        model = null;
    }

    void SetCharacterSlot()
    {
        var dataTable = GameManager.DataTable.GetCharacterDataTable();
        foreach (var dataKV in dataTable)
        {
            var data = dataKV.Value;
            if (null == data) continue;

            CreateCharacterSlot(data.Id);
        }
    }

    void CreateCharacterSlot(string characterId)
    {
        GameObject slot = GameObject.Instantiate(view.CharacterSlotPrefab, view.CharacterGroupParent);
        if (null == slot)
        {
            Debug.LogError($"Failed to create character slot for characterId: {characterId}");
            return;
        }

        if (slot.TryGetComponent(out CharacterSlot slotComponent))
        {
            slotComponent.Init(characterId);
            characterSlots.Add(characterId, slotComponent);
        }
        else
        {
            Debug.LogError($"Character slot prefab does not contain CharacterSlot component.");
        }
    }
}
