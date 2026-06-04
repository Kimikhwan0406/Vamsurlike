using UnityEngine;

public struct RunTimeWeaponlData
{
    public string WeaponId;
    public int ProjectileCount;
    public int ProjectileLimits;
    public int ProjectilePenetration;   // 관통 횟수
    public float ProjectileSpeed;
    public float CoolTime;              // 공격 간격
    public float RepeatInterval;        // 한 공격 주기에서 투사체 발사 간격
    public float Damage;
    public float Range;                 // 공격 범위
    public float Knockback;
    public float Duration;
}


public class WeaponObject
{
    IWeaponPattern pattern;
    RunTimeWeaponlData runtimeWeaponData;

    public string WeaponId => weaponId;
    public int WeaponLevel => level;
    public float OwnedStartTime => ownedStartTime;

    string weaponId;
    float coolTime;
    float coolTimeTimer;
    float ownedStartTime;
    int level = 1;

    public WeaponObject(string weaponId)
    {
        this.weaponId = weaponId;
        WeaponData weaponData = GameManager.DataTable.GetWeaponData(weaponId);
        pattern = WeaponFatternFactory.Create(weaponData.PatternType);

        runtimeWeaponData = new RunTimeWeaponlData
        {
            WeaponId = weaponId,
            ProjectileCount = 1,
            ProjectileLimits = weaponData.ProjectileLimits,
            ProjectilePenetration = weaponData.ProjectilePenetration,
            ProjectileSpeed = weaponData.ProjectileSpeed,
            CoolTime = weaponData.CoolTIme,
            RepeatInterval = weaponData.RepeatInterval,
            Damage = weaponData.BaseDamage,
            Range = weaponData.Range,
            Knockback = weaponData.Knockback,
            Duration = weaponData.Duration
        };


        coolTime = runtimeWeaponData.CoolTime + runtimeWeaponData.Duration;
        coolTimeTimer = 0;

        ownedStartTime = Time.time;
    }

    public void Update(float deltaTime, WeaponContext context)
    {
        coolTimeTimer -= deltaTime;

        if (coolTimeTimer > 0f) return;

        pattern.Excute(context, runtimeWeaponData);

        coolTimeTimer = coolTime;
    }

    public void UpgradeWeapon()
    {
        level++;
        
        string weaponLevelId = level.ToString() + "62" + weaponId.Substring(3);
        var data = GameManager.DataTable.GetWeaponLevelData(weaponLevelId);
        if(null == data)
        {
            Debug.Log($"WeaponLevelId : {weaponLevelId} null");
            return;
        }

        for (int i = 0; i < data.EffectName.Count; i++)
        {

            switch (data.EffectName[i])
            {
                case "Damage":
                    runtimeWeaponData.Damage += data.EffectValue[i];
                    break;
                case "ProjectileCount":
                    runtimeWeaponData.ProjectileCount += (int)data.EffectValue[i];
                    break;
                case "CoolTIme":
                    runtimeWeaponData.CoolTime -= data.EffectValue[i];
                    break;
                case "ProjectilePenetration":
                    runtimeWeaponData.ProjectilePenetration += (int)data.EffectValue[i];
                    break;
                case "RepeatInterval":
                    runtimeWeaponData.RepeatInterval -= data.EffectValue[i];
                    break;
                case "Range":
                    runtimeWeaponData.Range += runtimeWeaponData.Range * data.EffectValue[i] / 100;
                    break;
                case "Duration":
                    runtimeWeaponData.Duration += data.EffectValue[i];
                    break;
                case "ProjectileSpeed":
                    runtimeWeaponData.ProjectileSpeed += runtimeWeaponData.ProjectileSpeed * data.EffectValue[i];
                    break;
            }
        }
    }
}
