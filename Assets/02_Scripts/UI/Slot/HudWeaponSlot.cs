using UnityEngine;
using UnityEngine.UI;

public class HudWeaponSlot : MonoBehaviour
{
    [SerializeField] Image weaponIcon;

    public void Init(string weaponId)
    {
        weaponIcon.sprite = Utils.ResourcesLoad<Sprite>($"Sprite/Weapon/{weaponId}");
    }
}
