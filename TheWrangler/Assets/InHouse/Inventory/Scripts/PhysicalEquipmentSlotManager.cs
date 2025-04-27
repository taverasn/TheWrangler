using System;
using System.Linq;
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
        slots[EquipmentSlot.HEAD] = HeadSlot;
        slots[EquipmentSlot.MAIN_HAND] = WeaponSlot;
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

        if (slots[item.info.equipmentSlot].childCount > 0)
        {
            Destroy(slots[item.info.equipmentSlot].GetChild(0).gameObject);
        }

        if (equipped)
        {
            Transform equipmentTransform = Instantiate(item.info.prefab).transform;
            equipmentTransform.SetParent(slots[item.info.equipmentSlot], false);

            inventory.PhysicalItemEquipped(equipmentTransform.gameObject.GetComponent<PhysicalItem>(), item.info.equipmentSlot);
        }
    }

}

[Serializable] public class PhysicalEquipmentSlotDictionary : SerializableDictionary<EquipmentSlot, Transform> { };
