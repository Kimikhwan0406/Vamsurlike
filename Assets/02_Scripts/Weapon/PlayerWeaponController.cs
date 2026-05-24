using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    List<WeaponObject> weapons;
    WeaponContext context;

    void Awake()
    {
        context = new WeaponContext
        {
            Owner = gameObject,
            OwnerTransform = transform,
            CombatQuerySystem = GameManager.CombatQuery
        };

        weapons = new List<WeaponObject>();

        RegisterWeapon(new WeaponObject("263001"));
    }

    void Update()
    {
        float deltaTime = Time.deltaTime;

        for (int i = 0; i < weapons.Count; i++)
        {
            weapons[i].Update(deltaTime, context);
        }
    }

    public void RegisterWeapon(WeaponObject weapon)
    {
        weapons.Add(weapon);
    }

    public void UpgradeWeapon(WeaponObject weapon)
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            if (weapons[i] == weapon)
            {
                weapons[i].UpgradeWeapon();
            }
        }
    }
}
