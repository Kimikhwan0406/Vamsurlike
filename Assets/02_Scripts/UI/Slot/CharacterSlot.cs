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

    event Action<string, string> onClickCharacterSlot;

    string characterId;
    string baseWeaponId;

    public void Init(string characterId, string weaponId, Action<string, string> onClickCallback)
    {
        this.characterId = characterId;
        baseWeaponId = weaponId;

        characterNameText.text = GameManager.DataTable.GetCharacterData(characterId).Name;
        characterImage.sprite = Utils.ResourcesLoad<Sprite>($"Sprite/PlayerSprite/{characterId}");
        baseWeaponImage.sprite = Utils.ResourcesLoad<Sprite>($"Sprite/Weapon/{weaponId}");

        onClickCharacterSlot = onClickCallback;
    }

    public void OnClickCharacterSlot()
    {
        onClickCharacterSlot?.Invoke(characterId, baseWeaponId);
    }

    public void ChangeSelectedSlot(bool isSelected)
    {
        frameImage.color = isSelected ? Color.yellow : Color.white;
    }
}
