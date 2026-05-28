using System.Collections.Generic;

public struct CombatStat
{
    public string WeaponId;
    public float TotalDamage;
}

public struct DamageContext
{
    public string WeaponId;
    public float Damage;
}

public class CombatStatRecorder
{
    Dictionary<string, CombatStat> combatStats = new();

    public void RegisterCombatStat(string weaponId)
    {
        if (!combatStats.ContainsKey(weaponId))
        {
            combatStats.Add(weaponId, new CombatStat
            {
                WeaponId = weaponId,
                TotalDamage = 0
            });
        }
    }

    public void AddDamage(string weaponId, float damage)
    {
        if (combatStats.TryGetValue(weaponId, out var stat))
        {
            stat.TotalDamage += damage;
        }
    }

    public CombatStat GetCombatStat(string weaponId)
    {
        if (combatStats.TryGetValue(weaponId, out var stat))
        {
            return stat;
        }
        return default;
    }

    public void Release()
    {
        combatStats.Clear();
    }
}

// GameMgr에 등록 필요, 플로우는? 일단 무기가 생성된 PlayerWeaponController 이후일 듯.