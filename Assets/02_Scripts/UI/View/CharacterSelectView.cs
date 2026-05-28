using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectView : MonoBehaviour, IView
{
    public Transform CharacterSlotGroupParent;
    public GameObject CharacterSlotPrefab;
    public Button StartGameButton;

    [SerializeField] TextMeshProUGUI selectedCharacterName;
    [SerializeField] Image selectedCharacterIcon;
    [SerializeField] Image baseWeaponIcon;
    [SerializeField] TextMeshProUGUI selectedCharacterDescription;

    bool notSelectedYet = true;


    public bool IsOpen => this.gameObject.activeSelf;

    void OnEnable()
    {
        Init();
        StartGameButton.gameObject.SetActive(false);
        selectedCharacterName.gameObject.SetActive(false);
        selectedCharacterIcon.gameObject.SetActive(false);
        baseWeaponIcon.gameObject.SetActive(false);
        selectedCharacterDescription.gameObject.SetActive(false);
    }

    public void UpdateSelectedCharacterInfo(string characterId)
    {
        var data = GameManager.DataTable.GetCharacterData(characterId);

        if (notSelectedYet)
        {
            StartGameButton.gameObject.SetActive(true);
            selectedCharacterName.gameObject.SetActive(true);
            selectedCharacterIcon.gameObject.SetActive(true);
            baseWeaponIcon.gameObject.SetActive(true);
            selectedCharacterDescription.gameObject.SetActive(true);
            notSelectedYet = false;
        }

        selectedCharacterName.text = data.Name;
        selectedCharacterIcon.sprite = Utils.ResourcesLoad<Sprite>($"Sprite/PlayerSprite/{characterId}");
        baseWeaponIcon.sprite = Utils.ResourcesLoad<Sprite>($"Sprite/Weapon/{data.DefaultWeapon}");
        selectedCharacterDescription.text = data.Description;
    }

    public void Init()
    {
        selectedCharacterName.text = string.Empty;
        selectedCharacterIcon.sprite = null;
        selectedCharacterDescription.text = string.Empty;
    }

    public void Open()
    {
        this.gameObject.SetActive(true);
    }

    public void Close()
    {
        this.gameObject.SetActive(false);
    }
}
