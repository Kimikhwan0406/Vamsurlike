using System;
using System.Collections.Generic;
using System.Xml.Linq;
using static Unity.Collections.AllocatorManager;

[Serializable]
public class BaseData
{
    public string Id;
}

[Serializable]
public class CharacterData : BaseData
{
    public string Name;
    public int MaxHealth;
    public string DefaultWeapon;
    public string Ability;
    public List<string> AbilityCondition;
    public List<string> AbilityMargin;
    public List<string> AbilityValue;
    public List<string> PN;
    public string UnlockCondition;
    public int UnlockValue;
    public string Description;
}

[Serializable]
public class EnemyData : BaseData
{
    public string Name;
    public float MaxHealth;
    public float Power;
    public float MoveSpeed;
    public float XP;
}

[Serializable]
public class StageData : BaseData
{
    public int TimeElapsed;
    public List<string> Enemies;
    public int EnemyMinimum;
    public float SpawnInterval;
    public List<string> Boss;
    public string MapEvent;
}

[Serializable]
public class WeaponData : BaseData
{
    public string Name;
    public int MaxLevel;
    public int ProjectileLimits;
    public int ProjectileCount;
    public string ProjectileSpeed;
    public int Rarity;
    public int CoolTIme;        // 공격 간격
    public int RepeatInterval;  // 한 공격 주기에서 투사체 발사 간격
    public int BaseDamage;
    public int Range;
    public int Knockback;
    public int Duration;
    public string Evolution;

    public WeaponPatternType PatternType;
    //public TargetingType TargetingType;
    //public int PenetrationCount;
}

[Serializable]
public class WeaponLevelData : BaseData
{
    public int Level;
    public List<string> EffectName;
    public List<string> EffectValue;
}