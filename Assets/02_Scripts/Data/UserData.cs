using System;

public interface IUserData
{
    void SetDefaultData();
    void SaveData();
    void LoadData();
    BaseUserData GetData();
    
}

[Serializable]
public class BaseUserData { }

[Serializable]
public class PowerUpData : BaseUserData
{
    public bool Amount;                 // 투사체 개수 +1
    public bool Growth;                 // 경험치 획득 +3%
    public bool Cooldown;               // 2.5% 감소
    public bool Armor;                  // 1증가 : 데미지 1감소
    public bool Luck;                   // 10% 증가
    public bool Might;                  // 공격력 +5%
    public bool Recovery;               // 회복력: 초당 회복 0.1 증가
    public bool Greed;                  // 골드 획득 +5%
    public bool Area;                   // 사거리 +5%
    public bool ProjectileSpeed;        // 투사체 속도 +10%
    public bool Duration;               // +15%
    public bool MoveSpeed;              // +5%
    public bool Magnet;                 // 흭득 반경 +25%
    public bool MaxHealth;              // 최대 체력 +10%
}
