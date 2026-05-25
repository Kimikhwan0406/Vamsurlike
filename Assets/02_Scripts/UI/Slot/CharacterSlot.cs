using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSlot : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI characterNameText;
    [SerializeField] Image baseWeaponImage;

    string characterId;

    public void Init(string characterId)
    {
        this.characterId = characterId;
        characterNameText.text = GameManager.DataTable.GetCharacterData(characterId).Name;
        // TODO: 현재 무기 스프라이트가 없어서 캐릭터 스프라이트로 임시 대체
        baseWeaponImage.sprite = Utils.ResourcesLoad<Sprite>($"Sprite/PlayerSprite/{characterId}");
    }

    public void OnClickCharacterSlot()
    {
        GameManager.UI.GetPresenter<CharacterSelectPresenter, CharacterSelectView>().OnClickCharacterSlot(characterId);
    }
}
