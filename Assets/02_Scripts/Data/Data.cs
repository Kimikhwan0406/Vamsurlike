using System;
using System.Collections.Generic;

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
    public List<float> PositionOffset;
    public float Radius;
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
    public int ProjectilePenetration;   // 관통 횟수
    public float ProjectileSpeed;
    public int Rarity;
    public float CoolTIme;              // 공격 간격
    public float RepeatInterval;        // 한 공격 주기에서 투사체 발사 간격
    public float BaseDamage;
    public float Range;                 // 공격 범위
    public int Knockback;
    public float Duration;
    public string Evolution;

    public string PatternType;
    //public TargetingType TargetingType;
    //public int PenetrationCount;
}

[Serializable]
public class WeaponLevelData : BaseData
{
    public string WeaponId;
    public int Level;
    public List<string> EffectName;
    public List<float> EffectValue;
    public string Description;
}