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
