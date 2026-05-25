using UnityEngine;

public class CharacterSlot : MonoBehaviour
{
    string characterId;

    public void Init(string characterId)
    {
        this.characterId = characterId;
    }

    public void OnClickCharacterSlot()
    {
        GameManager.UI.GetPresenter<CharacterSelectPresenter, CharacterSelectView>().OnClickCharacterSlot(characterId);
    }
}
