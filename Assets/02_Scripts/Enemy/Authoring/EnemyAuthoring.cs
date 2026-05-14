using Unity.Entities;
using UnityEngine;

class EnemyAuthoring : MonoBehaviour
{
    public float speed = 3f;
    public float currentHp = 100f;
    public float maxHp = 100f;
    public float damage = 0.2f;
}

class EnemyAuthoringBaker : Baker<EnemyAuthoring>
{
    public override void Bake(EnemyAuthoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.Dynamic);

        AddComponent(entity, new EnemyComponent
        {
            Speed = authoring.speed,
            CurrentHp = authoring.currentHp,
            MaxHp = authoring.maxHp,
            Damage = authoring.damage
        });

        AddComponent<EnemyTag>(entity);
    }
}
