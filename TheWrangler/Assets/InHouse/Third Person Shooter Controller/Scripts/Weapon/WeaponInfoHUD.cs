using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponInfoHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ammoInfo;

    private void OnEnable()
    {
        GameEventsManager.Instance.weaponInputEvents.onAmmoUpdate += AmmoUpdate;
    }

    private void OnDisable()
    {
        GameEventsManager.Instance.weaponInputEvents.onAmmoUpdate -= AmmoUpdate;
    }

    private void AmmoUpdate(int currentAmmo, int currentAmmoReserve)
    {
        ammoInfo.text = currentAmmo.ToString() + " / " + currentAmmoReserve.ToString();
    }
}
