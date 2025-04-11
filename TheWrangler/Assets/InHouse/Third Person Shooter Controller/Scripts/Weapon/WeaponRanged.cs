using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRanged
{
    public WeaponInfoSO weaponInfo;
    public int currentAmmo { get; private set; }
    public int currentAmmoReserve { get; private set; }
    public int damage { get; private set; }
    
    // Contructor: First Time Using Ranged Weapon
    public WeaponRanged(WeaponInfoSO _weaponInfo)
    {
        weaponInfo = _weaponInfo;
        currentAmmo = _weaponInfo.maxAmmo;
        currentAmmoReserve = _weaponInfo.maxAmmoReserve;
        damage = _weaponInfo.damage;
    }

    // Constructor: Ranged Weapon has saved data
    public WeaponRanged(WeaponInfoSO _weaponInfo, int _currentAmmo, int _currentAmmoReserve)
    {
        weaponInfo = _weaponInfo;
        currentAmmo = _currentAmmo;
        currentAmmoReserve = _currentAmmoReserve;
    }

    public void Reload()
    {
        if(currentAmmoReserve > weaponInfo.maxAmmo)
        {
            int ammoSubtraction = weaponInfo.maxAmmo - currentAmmo;
            currentAmmoReserve -= ammoSubtraction;
            currentAmmo = weaponInfo.maxAmmo;
        }
        else
        {
            currentAmmo = currentAmmoReserve;
            currentAmmoReserve = 0;
        }
    }

    public void AmmoPickup(int pickupAmount)
    {
        currentAmmoReserve += pickupAmount;
        if(currentAmmoReserve > weaponInfo.maxAmmoReserve)
        {
            currentAmmoReserve = weaponInfo.maxAmmoReserve;
        }
    }

    public WeaponRangedData GetWeaponRangedData()
    {
        return new WeaponRangedData(currentAmmo, currentAmmoReserve);
    }

    public void Shoot()
    {
        if(currentAmmo > 0)
        {
            currentAmmo--;
        }
    }

    public bool CanShoot()
    {
        return currentAmmo > 0;
    }

    public bool CanReload()
    {
        return currentAmmoReserve > 0 && currentAmmo < weaponInfo.maxAmmo;
    }
}
