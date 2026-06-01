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
            "RandomDrop" => new RandomDropWeaponPattern(),
            "Orbit" => new OrbitWeaponPattern(),
            "OrbitN" => new OrbitWeaponPattern(-1),
            "Axe" => new AxeWeaponPattern(),
            "FireWand" => new FireWandWeaponPattern(),
            _ => throw new System.Exception($"Unknown weapon pattern type: {type}")
        };
    }
}
