using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController
{
    List<WeaponObject> weaponList = new();
    WeaponContext context;
    List<string> maxLevelWeapons = new();

    public void CreateWeaponContext(WeaponContext context)
    {
        this.context = context;
    }


    public void Update()
    {
        float deltaTime = Time.deltaTime;

        for (int i = 0; i < weaponList.Count; i++)
        {
            weaponList[i].Update(deltaTime, context);
        }
    }

    public List<string> GetMaxLevelWeapons() => maxLevelWeapons;

    public bool HasWeapon(string weaponId) => weaponList.Exists(w => w.WeaponId == weaponId);
    public int GetWeaponLevel(string weaponId)
    {
        var weapon = weaponList.Find(w => w.WeaponId == weaponId);
        if (weapon != null)
        {
            return weapon.WeaponLevel;
        }
        else
        {
            Debug.Log($"{GetType()}: 무기가 존재하지 않음");
            return 0;
        }
    }

    public void RegisterWeapon(string weaponId)
    {
        if (weaponList.Exists(w => w.WeaponId == weaponId))
        {
            Debug.Log($"{GetType()}: 무기가 이미 존재함");
            return;
        }

        var weapon = new WeaponObject(weaponId);

        weaponList.Add(weapon);
    }

    public void UnregisterWeapon(string weaponId)
    {
        if (!weaponList.Exists(w => w.WeaponId == weaponId))
        {
            Debug.Log($"{GetType()}: 무기가 존재하지 않음");
            return;
        }

        foreach (var weapon in weaponList)
        {
            if (weapon.WeaponId == weaponId)
            {
                weaponList.Remove(weapon);
                break;
            }
        }
    }

    public void UpgradeWeapon(string weaponId)
    {
        var weapon = weaponList.Find(w => w.WeaponId == weaponId);
        if (weapon == null)
        {
            Debug.LogError($"{GetType()}: 무기가 존재하지 않음");
            return;
        }

        weapon.UpgradeWeapon();


        if (weapon.WeaponLevel == GameManager.DataTable.GetWeaponData(weaponId).MaxLevel)
        {
            maxLevelWeapons.Add(weaponId);
        }
    }

    public void Release()
    {
        context = null;
        weaponList.Clear();
        maxLevelWeapons.Clear();
    }
}
