using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpSlot : MonoBehaviour
{
    string itemId;
    bool isNewItem = false;

    [SerializeField] Image SkilIcon;
    [SerializeField] TextMeshProUGUI skilName;
    [SerializeField] TextMeshProUGUI skilDesc;
    [SerializeField] TextMeshProUGUI skilLevel;

    public void Init(string id, int level)
    {
        itemId = id;

        SkilIcon.sprite = Utils.ResourcesLoad<Sprite>($"Sprite/Weapon/{id}");

        string weaponLevelId = level.ToString() + "62" + id.Substring(3);

        skilName.text = GameManager.DataTable.GetWeaponData(itemId).Name;
        if (level != 1)
            skilDesc.text = GameManager.DataTable.GetWeaponLevelData(weaponLevelId).Description;
        else
            skilDesc.text = "";

        if (level == 1)
        {
            skilLevel.text = "New!";
            skilLevel.color = Color.yellow;
            isNewItem = true;
        }
        else
        {
            skilLevel.text = $"level: {level}";

            Color color;
            // 16진수 색상 코드 입력 (알파값 포함 가능)
            if (UnityEngine.ColorUtility.TryParseHtmlString("#DBCDAC", out color))
            {
                skilLevel.color = color;
            }
        }
    }

    public void OnClickSlot()
    {
        if (!isNewItem)
        {
            GameManager.WeaponController.UpgradeWeapon(itemId);
        }
        else
        {
            GameManager.WeaponController.AddWeapon(itemId);
        }
        
        GameManager.UI.CloseUI();
    }
}
