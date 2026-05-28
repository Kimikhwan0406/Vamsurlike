using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSlot : MonoBehaviour
{
    [SerializeField] Image frameImage;
    [SerializeField] TextMeshProUGUI characterNameText;
    [SerializeField] Image characterImage;
    [SerializeField] Image baseWeaponImage;
    [SerializeField] Button selectedButton;

    event Action<string> onClickCharacterSlot;

    string characterId;

    public void Init(string characterId, string weaponId, Action<string> onClickCallback)
    {
        this.characterId = characterId;

        characterNameText.text = GameManager.DataTable.GetCharacterData(characterId).Name;
        characterImage.sprite = Utils.ResourcesLoad<Sprite>($"Sprite/PlayerSprite/{characterId}");
        baseWeaponImage.sprite = Utils.ResourcesLoad<Sprite>($"Sprite/Weapon/{weaponId}");

        onClickCharacterSlot = onClickCallback;
    }

    public void OnClickCharacterSlot()
    {
        onClickCharacterSlot?.Invoke(characterId);
    }

    public void ChangeSelectedSlot(bool isSelected)
    {
        frameImage.color = isSelected ? Color.yellow : Color.white;
    }
}
