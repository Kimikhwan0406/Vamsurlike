using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpSlot : MonoBehaviour
{
    string itemId;

    [SerializeField] Image SkilIcon;
    [SerializeField] TextMeshProUGUI skilName;
    [SerializeField] TextMeshProUGUI skilDesc;
    [SerializeField] TextMeshProUGUI skilLevel;

    public void Init(string id, int level)
    {
        itemId = id;


        skilName.text = GameManager.DataTable.GetWeaponData(itemId).Name;
        skilDesc.text = GameManager.DataTable.GetWeaponLevelData(id).Description;
        if (level == 1)
        {
            skilLevel.text = "New!";
            skilLevel.color = Color.yellow;
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
}
