using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponInfoSO", menuName = "ScriptableObjects/Weapons/WeaponInfoSO", order = 0)]

public class WeaponInfoSO : ScriptableObject
{
    public GameObject pfWeapon;
    public WeaponType weaponType;
    public float fireRate;
    public int maxAmmo;
    public int maxAmmoReserve;
    public int damage;
}
