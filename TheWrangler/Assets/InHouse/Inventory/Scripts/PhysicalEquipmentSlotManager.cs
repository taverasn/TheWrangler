using System;
using System.Linq;
using JUTPS;
using JUTPS.CharacterBrain;
using Sirenix.OdinInspector;
using UnityEngine;

public class PhysicalEquipmentSlotManager : MonoBehaviour
{
    [field:SerializeField] public PhysicalEquipmentSlotDictionary slots { get; private set; } = new PhysicalEquipmentSlotDictionary();
    private JUCharacterBrain jUCharacterBrain;
    private PlayerInventory inventory;

    private void Awake()
    {
        inventory = GetComponent<PlayerInventory>();
        jUCharacterBrain = GetComponent<JUCharacterBrain>();
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
        if (item == null || item.info.equipmentSlot == EquipmentSlot.NONE) return;

        if (slots[item.info.equipmentSlot].childCount > 0)
        {
            Destroy(slots[item.info.equipmentSlot].GetChild(0).gameObject);
        }

        if (equipped)
        {
            Transform equipmentTransform = Instantiate(item.info.prefab).transform;
            equipmentTransform.SetParent(slots[item.info.equipmentSlot], false);

            PhysicalItem physicalItem = equipmentTransform.gameObject.GetComponent<PhysicalItem>();
            physicalItem.owner = jUCharacterBrain.owner;

            Damager damager = physicalItem.GetComponentInChildren<Damager>();
            if (damager != null)
            {
                damager.owner = jUCharacterBrain.owner;
            }

            inventory.PhysicalItemEquipped(physicalItem, item.info.equipmentSlot);
        }
    }

}

[Serializable] public class PhysicalEquipmentSlotDictionary : SerializableDictionary<EquipmentSlot, Transform> { };
