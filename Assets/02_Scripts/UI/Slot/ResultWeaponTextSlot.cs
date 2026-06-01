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
        weaponOwnedTimeText.text = (Mathf.Floor(ownedTime / 60)).ToString()
            + ":" + (ownedTime % 60).ToString("F0");



        weaponTotalDamageText.text = NumberFormat(stat.TotalDamage);
        dpsText.text = NumberFormat(stat.TotalDamage / ownedTime);
    }


    string NumberFormat(float value)
    {
        float absValue = Mathf.Abs(value);

        string[] suffixes = { "", "K", "M", "B", "P" };
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