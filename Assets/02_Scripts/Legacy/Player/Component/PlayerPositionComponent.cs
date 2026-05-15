using Unity.Entities;
using Unity.Mathematics;

public struct PlayerPositionComponent : IComponentData
{
    public float3 Position;
    public bool IsMoving;
}

public struct PlayerTag : IComponentData { }