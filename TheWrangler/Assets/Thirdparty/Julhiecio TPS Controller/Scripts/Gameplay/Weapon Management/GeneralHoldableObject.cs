using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JUTPS.ItemSystem;

namespace JUTPS
{

    [AddComponentMenu("JU TPS/Item System/General Holdable Item")]
    public class GeneralHoldableObject : JUGeneralHoldableItem
    {
        public override void UseItem()
        {
            base.UseItem();
            switch(itemSO)
            {
                case ConsumableSO consumableSO:
                    consumableSO.NeedsUpdateMessage(owner, guid);
                    break;
            }
        }
    }

}
