using UnityEngine;

public class WeaponContext
{
    public GameObject Owner;
    public Transform OwnerTransform;
    public CombatQuerySystem CombatQuerySystem;
    //public ProjectilePool ProjectilePool;
    //public VfxPool VfxPool;
}

public interface IWeaponPattern
{
    void Excute(WeaponContext context, RunTimeWeaponlData data);
}
