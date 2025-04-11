using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponRangedData
{
    public int ammoReserve;
    public int currentAmmo;

    public WeaponRangedData(int _currentAmmo, int _ammoReserve)
    {
        ammoReserve = _ammoReserve;
        currentAmmo = _currentAmmo;
    }
}
