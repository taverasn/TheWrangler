using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using JUTPS.JUInputSystem;
using JUTPS.InventorySystem;

using JUTPSEditor.JUHeader;
using System.Linq;
using UnityEngine.TextCore.Text;

namespace JUTPS.ItemSystem
{

    [AddComponentMenu("JU TPS/Item System/Item Switch Manager")]
    public class ItemSwitchManager : MonoBehaviour
    {
        [JUHeader("Settings")]
        public bool IsPlayer;
        public bool UseOldInputSystem;
        [SerializeField] private JUCharacterController JuTPSCharacter;
        public ItemSO ItemToEquipOnStart;


        [JUHeader("Next-Previous Item Switch [Q-E]")]
        public bool EnableNextAndPreviousWeaponSwitch;
        [Tooltip("[OLD INPUT SYSTEM ONLY]")]
        public KeyCode CustomNextWeaponKeyCode, CustomPreviousWeaponKeycode;

        [JUHeader("Alpha Numeric Item Switch")]
        public bool EnableAlphaNumericWeaponSwitch;

        [JUHeader("Mouse Scroll Item Switch")]
        public bool EnableMouseScrollWeaponSwitch;
        public float ScrollThreshold = 0.1f;



        protected virtual void Start()
        {

            if (JuTPSCharacter == null)
            {
                JuTPSCharacter = GetComponent<JUCharacterController>();
                if (JuTPSCharacter != null)
                {
                    Invoke("EquipStartItem", 0.2f);
                }
            }
            else
            {
                Invoke(nameof(EquipStartItem), 0.2f);
            }
            IsPlayer = gameObject.tag == "Player";
        }
        protected virtual void Update()
        {
            if (IsPlayer == false)
                return;

            if (JuTPSCharacter.IsMeleeAttacking || JuTPSCharacter.IsRagdolled || JuTPSCharacter.IsDead || JuTPSCharacter.IsRolling) return;

            OldInput_ItemSwitchController();
            NewInput_ItemSwitchController();
        }

        private void EquipStartItem()
        {
            JuTPSCharacter.SwitchToItem(ItemToEquipOnStart ? ItemToEquipOnStart.ID : "");
        }
        protected virtual void OldInput_ItemSwitchController()
        {
            if (UseOldInputSystem == false) return;

            if (EnableNextAndPreviousWeaponSwitch)
            {
                if (CustomNextWeaponKeyCode != KeyCode.None)
                {
                    if (Input.GetKeyDown(CustomNextWeaponKeyCode))
                    {
                        JuTPSCharacter.SwitchToNextItem();
                    }
                }
                else
                {
                    if (JUInput.GetButtonDown(JUInput.Buttons.NextWeaponButton))
                    {
                        Debug.Log("Switch manager tentou trocar para o proximo item");
                        JuTPSCharacter.SwitchToNextItem();
                    }
                }
                if (CustomPreviousWeaponKeycode != KeyCode.None)
                {
                    if (Input.GetKeyDown(CustomPreviousWeaponKeycode))
                    {
                        JuTPSCharacter.SwitchToPreviousItem();
                    }
                }
                else
                {
                    if (JUInput.GetButtonDown(JUInput.Buttons.PreviousWeaponButton))
                    {
                        JuTPSCharacter.SwitchToPreviousItem();
                    }
                }
            }

            if (EnableMouseScrollWeaponSwitch)
            {
                if (Input.GetAxis("Mouse ScrollWheel") >= ScrollThreshold)
                {
                    JuTPSCharacter.SwitchToNextItem();
                }
                if (Input.GetAxis("Mouse ScrollWheel") <= -ScrollThreshold)
                {
                    JuTPSCharacter.SwitchToPreviousItem();
                }
            }

            if (EnableAlphaNumericWeaponSwitch)
            {
                for (int i = 48; i < 58; i++)
                {
                    int InputKey = i;
                    int SwitchID = i - 49;
                    if (Input.GetKeyDown((KeyCode)InputKey))
                    {
                        JuTPSCharacter.SwitchToItem(JuTPSCharacter.Inventory.GetSequentialSlotItemID((HotBarSlot)SwitchID));
                    }
                }
            }
        }
        protected virtual void NewInput_ItemSwitchController()
        {
            if (UseOldInputSystem == true) return;
            if (JUInput.Instance() == null) return;
            if (JUInput.Instance().InputActions == null) return;

            if (EnableNextAndPreviousWeaponSwitch)
            {
                if (JUInput.GetButtonDown(JUInput.Buttons.NextWeaponButton))
                {
                    JuTPSCharacter.SwitchToNextItem(true);
                }

                if (JUInput.GetButtonDown(JUInput.Buttons.PreviousWeaponButton))
                {
                    JuTPSCharacter.SwitchToPreviousItem(true);
                }
            }

            if (EnableMouseScrollWeaponSwitch && Mouse.current != null)
            {
                if (Mouse.current.scroll.ReadValue().y / 360 >= ScrollThreshold)
                {
                    JuTPSCharacter.SwitchToNextItem(true);
                }
                if (Mouse.current.scroll.ReadValue().y / 360 <= -ScrollThreshold)
                {
                    JuTPSCharacter.SwitchToPreviousItem(true);
                }
            }

            if (EnableAlphaNumericWeaponSwitch)
            {
                if (JUInput.Instance().InputActions.Player.Slot1.triggered) SwitchToItemInSequentialSlot(HotBarSlot.first);
                if (JUInput.Instance().InputActions.Player.Slot2.triggered) SwitchToItemInSequentialSlot(HotBarSlot.second);
                if (JUInput.Instance().InputActions.Player.Slot3.triggered) SwitchToItemInSequentialSlot(HotBarSlot.third);
                if (JUInput.Instance().InputActions.Player.Slot4.triggered) SwitchToItemInSequentialSlot(HotBarSlot.fourth);
                if (JUInput.Instance().InputActions.Player.Slot5.triggered) SwitchToItemInSequentialSlot(HotBarSlot.fifth);
                if (JUInput.Instance().InputActions.Player.Slot6.triggered) SwitchToItemInSequentialSlot(HotBarSlot.sixth);
                if (JUInput.Instance().InputActions.Player.Slot7.triggered) SwitchToItemInSequentialSlot(HotBarSlot.seventh);
                if (JUInput.Instance().InputActions.Player.Slot8.triggered) SwitchToItemInSequentialSlot(HotBarSlot.eighth);
                if (JUInput.Instance().InputActions.Player.Slot9.triggered) SwitchToItemInSequentialSlot(HotBarSlot.ninth);
                if (JUInput.Instance().InputActions.Player.Slot10.triggered) SwitchToItemInSequentialSlot(HotBarSlot.tenth);
       }
        }


        /// <summary>
        /// Changes character selected item to the next item in the list
        /// </summary>
        public virtual void NextItem()
        {
            JuTPSCharacter.SwitchToNextItem();
        }

        /// <summary>
        /// Changes character selected item to the previous item in the list
        /// </summary>
        public virtual void PreviousItem()
        {
            JuTPSCharacter.SwitchToPreviousItem();
        }

        /// <summary>
        /// Changes character selected item to a specific one in the list
        /// </summary>
        /// <param name="SwitchID">Item index</param>
        public virtual void SwitchToItem(string SwitchID)
        {
            if (JuTPSCharacter.Inventory.mainHandItems.Any(i => i.info.ID == SwitchID))
            {
                if (JuTPSCharacter.IsItemEquiped == false)
                {
                    JuTPSCharacter.SwitchToItem(SwitchID);
                }
                else
                {
                    if (JuTPSCharacter.HoldableItemInUseRightHand.ItemSwitchID != SwitchID) JuTPSCharacter.SwitchToItem(SwitchID);
                }
            }
            else
            {
                JuTPSCharacter.SwitchToItem("");
            }
        }


        public virtual void SwitchToItemInSequentialSlot(HotBarSlot Slot)
        {
            SwitchToItem(JuTPSCharacter.Inventory.GetSequentialSlotItemID(Slot));
        }


        /// <summary>
        /// Changes character selected item to a specific one in the list
        /// </summary>
        /// <param name="SwitchID">Item index</param>
        public static void SwitchCharacterItem(JUCharacterController character, string SwitchID)
        {
            if (character.Inventory.mainHandItems.Any(i => i.info.ID == SwitchID))
            {
                character.SwitchToItem(SwitchID);
            }
            else
            {
                Debug.LogWarning("Unable to switch to item with ID " + SwitchID + " , this ID is out of bounds for the list");
            }
        }


        /// <summary>
        /// Changes player selected item to a specific one in the list
        /// </summary>
        /// <param name="SwitchID">Item index</param>
        public static void SwitchPlayerItem(string SwitchID)
        {
            if (GameObject.FindGameObjectWithTag("Player") == null)
            {
                Debug.LogError("Could not find a gameobject tagged 'Player'");
                return;
            }

            JUCharacterController player = GameObject.FindGameObjectWithTag("Player").GetComponent<JUCharacterController>();
            if (player.Inventory.mainHandItems.Any(i => i.info.ID == SwitchID))
            {
                player.SwitchToItem(SwitchID);
            }
            else
            {
                Debug.LogWarning("Unable to switch to item with ID " + SwitchID + " , this ID is out of bounds for the list");
            }
        }

        /// <summary>
        /// Changes player selected item to the next item in the list
        /// </summary>
        public static void NextPlayerItem()
        {
            if (GameObject.FindGameObjectWithTag("Player") == null)
            {
                Debug.LogError("Could not find a gameobject tagged 'Player'");
                return;
            }

            JUCharacterController player = GameObject.FindGameObjectWithTag("Player").GetComponent<JUCharacterController>();
            player.SwitchToNextItem();
        }

        /// <summary>
        /// Changes player selected item to the previous item in the list
        /// </summary>
        public static void PreviousPlayerItem()
        {
            if (GameObject.FindGameObjectWithTag("Player") == null)
            {
                Debug.LogError("Could not find a gameobject tagged 'Player'");
                return;
            }

            JUCharacterController player = GameObject.FindGameObjectWithTag("Player").GetComponent<JUCharacterController>();
            player.SwitchToPreviousItem();
        }
    }


}