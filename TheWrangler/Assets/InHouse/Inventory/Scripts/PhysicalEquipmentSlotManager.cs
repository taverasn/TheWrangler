using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class PhysicalEquipmentSlotManager : MonoBehaviour
{
    [ShowInInspector] public PhysicalEquipmentSlotDictionary slots { get; private set; } = new PhysicalEquipmentSlotDictionary();
    [SerializeField] private Transform HeadSlot;
    [SerializeField] private Transform WeaponSlot;

    private Inventory inventory;

    private void Awake()
    {
        inventory = GetComponent<Inventory>();
        slots[EquipmentType.HEAD] = HeadSlot;
        slots[EquipmentType.WEAPON] = WeaponSlot;
    }

    private void OnEnable()
    {
        inventory.onItemEquipped += OnItemEquipped;
    }

    private void OnDisable()
    {
        inventory.onItemEquipped -= OnItemEquipped;
    }

    private void OnItemEquipped(Item item, bool equipped)
    {
        if (item == null) return;

        if (slots[item.info.equipmentType].childCount > 0)
        {
            Destroy(slots[item.info.equipmentType].GetChild(0).gameObject);
        }

        if (equipped)
        {
            Transform equipmentTransform = Instantiate(item.info.prefab).transform;
            equipmentTransform.SetParent(slots[item.info.equipmentType], false);
        }
    }

}

[Serializable] public class PhysicalEquipmentSlotDictionary : SerializableDictionary<EquipmentType, Transform> { };
