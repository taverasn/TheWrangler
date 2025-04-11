using System;
using UnityEngine;

public class WeaponInputEvents
{
    public event Action<int, int> onAmmoUpdate;
    public void AmmoUpdate(int currentAmmo, int currentAmmoReserve)
    {
        if (onAmmoUpdate != null)
        {
            onAmmoUpdate(currentAmmo, currentAmmoReserve);
        }
    }
}
