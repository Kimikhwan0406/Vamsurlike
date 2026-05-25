using UnityEngine;

public class WeaponObject
{
    IWeaponPattern pattern;
    WeaponData weaponData;

    public string WeaponId => weaponId;
    public int WeaponLevel => level;

    string weaponId;
    float coolTimeTimer;
    float coolTime = 1f;
    int level = 1;

    public WeaponObject(string weaponId)
    {
        weaponData = GameManager.DataTable.GetWeaponData(weaponId);

        this.weaponId = weaponId;
        pattern = WeaponFatternFactory.Create(weaponData.PatternType);
        coolTime = weaponData.CoolTIme;
        coolTimeTimer = coolTime;
    }

    public void Update(float deltaTime, WeaponContext context)
    {
        coolTimeTimer -= deltaTime;

        if (coolTimeTimer > 0f) return;

        pattern.Excute(context, weaponData);

        coolTimeTimer = coolTime;
    }

    public void UpgradeWeapon()
    {
        level++;
        //GameManager.DataTable.GetWeaponLevelData(weaponId);
        // TODO 레벨업 효과에 따라 효과 적용하기
    }
    
}
