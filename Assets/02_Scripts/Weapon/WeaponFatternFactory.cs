using UnityEngine;

public static class WeaponFatternFactory
{
    public static IWeaponPattern Create(string type)
    {
        return type switch
        {
            "Projectile" => new ProjectileWeaponPattern(),
            "Swing" => new SwingWeaponPattern(),
            "Area" => new AreaWeaponPattern(),
            //WeaponPatternType.ChainLightning => new ChainLightningWeaponPattern(),
            "Orbit" => new OrbitWeaponPattern(),
            "OrbitN" => new OrbitWeaponPattern(-1),
            _ => throw new System.Exception($"Unknown weapon pattern type: {type}")
        };
    }
}
