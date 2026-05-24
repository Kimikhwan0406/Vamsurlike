using UnityEngine;

public enum WeaponPatternType
{
    Projectile,
    AreaPulse,
    ChainLightning,
    Orbit,
    RandomArea,
    ForwardSlash
}

public static class WeaponFatternFactory
{
    public static IWeaponPattern Create(WeaponPatternType type)
    {
        return type switch
        {
            WeaponPatternType.Projectile => new ProjectileWeaponPattern(),
            //WeaponPatternType.AreaPulse => new AreaPulseWeaponPattern(),
            //WeaponPatternType.ChainLightning => new ChainLightningWeaponPattern(),
            //WeaponPatternType.Orbit => new OrbitWeaponPattern(),
            _ => throw new System.Exception($"Unknown weapon pattern type: {type}")
        };
    }
}
