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
    public int UnlockConditionGold;
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