using UnityEngine;

public static class WeaponFatternFactory
{
    public static IWeaponPattern Create(string type)
    {
        return type switch
        {
            "Projectile" => new ProjectileWeaponPattern(),
            //WeaponPatternType.AreaPulse => new AreaPulseWeaponPattern(),
            //WeaponPatternType.ChainLightning => new ChainLightningWeaponPattern(),
            "Orbit" => new OrbitWeaponPattern(),
            _ => throw new System.Exception($"Unknown weapon pattern type: {type}")
        };
    }
}
