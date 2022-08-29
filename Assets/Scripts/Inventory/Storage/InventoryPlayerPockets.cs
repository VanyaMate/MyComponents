using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VM.Inventory
{
    public class InventoryPlayerPockets : InventoryManagerObject
    {
        public static InventoryPlayerPockets Instance;

        private void Awake()
        {
            Instance = this;

            if (this._managerType != null)
            {
                this._manager = new InventoryManager(this._managerType, gameObject);
            }
        }
    }
}