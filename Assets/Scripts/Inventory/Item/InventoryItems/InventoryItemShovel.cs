using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VM.Inventory;
using VM.TerrainTools;

namespace VM.Inventory.Items
{
    public class InventoryItemShovel : InventoryItem
    {
        private bool _active;

        public InventoryItemShovel(SO_InventoryItem type, float amount, GameObject onScene = null) : base(type, amount, onScene)
        {
            this._active = false;
        }

        public override void LeftClickUIHandler(InventoryManager manager)
        {
            Debug.Log("IsShovel");

            if (!this._active)
            {
                TerrainManager.Instance.Enable();
            }
            else
            {
                TerrainManager.Instance.Disable();
            }

            this._active = !this._active;
        }
    }
}