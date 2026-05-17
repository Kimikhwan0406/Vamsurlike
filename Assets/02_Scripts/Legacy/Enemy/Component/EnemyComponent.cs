using Unity.Entities;

public struct EnemyComponent : IComponentData
{
    public float Speed;
    public float CurrentHp;
    public float MaxHp;
    public float Damage;
}

public struct EnemyTag : IComponentData { }