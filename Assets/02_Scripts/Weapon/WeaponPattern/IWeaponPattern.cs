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
    // 무기마다 패턴이 다름, 스위치로 처리하면 힘듦. 인터페이스로 나눔
    // 투사체, 체이닝, 장막공격 등 무기 패턴에 따라 클래스를 만들어 상속하여 각각에 맞는 패턴을 구현
    void Excute(WeaponContext context, RunTimeWeaponlData data);
}
