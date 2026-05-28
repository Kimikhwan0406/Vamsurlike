using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultWeaponTextSlot : MonoBehaviour
{
    [SerializeField] Image weaponImage;

    [SerializeField] TextMeshProUGUI weaponNameText;
    [SerializeField] TextMeshProUGUI weaponLevelText;
    [SerializeField] TextMeshProUGUI weaponTotalDamageText;
    [SerializeField] TextMeshProUGUI weaponOwnedTimeText;
    [SerializeField] TextMeshProUGUI dpsText;

    public void Init(CombatStat stat, int weaponLevel, float ownedTime)
    {
        weaponImage.sprite = Utils.ResourcesLoad<Sprite>($"Sprite/Weapon/{stat.WeaponId}");

        weaponNameText.text = GameManager.DataTable.GetWeaponData(stat.WeaponId).Name;
        weaponLevelText.text = weaponLevel.ToString();
        weaponOwnedTimeText.text = ownedTime.ToString();

        // 데미지랑 DPS는 ###.# 로 표현하며 네 자리수가 넘어간다면 K, M, B로 표시
        weaponTotalDamageText.text = NumberFormat(stat.TotalDamage);
        dpsText.text = NumberFormat(stat.TotalDamage / ownedTime);
    }


    string NumberFormat(float value)
    {
        float absValue = Mathf.Abs(value);

        string[] suffixes = { "", "K", "M", "B", "T" };
        float[] thresholds =
        {
        1f,
        1_000f,
        1_000_000f,
        1_000_000_000f,
        1_000_000_000_000f
        };

        int index = 0;

        for (int i = thresholds.Length - 1; i >= 0; i--)
        {
            if (absValue >= thresholds[i])
            {
                index = i;
                break;
            }
        }

        float scaledValue = value / thresholds[index];

        return scaledValue.ToString("0.#") + suffixes[index];
    }
}