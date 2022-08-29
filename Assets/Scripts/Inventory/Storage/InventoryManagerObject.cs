using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VM.Managers;
using VM.UI;
using VM.UI.Inventory;

namespace VM.Inventory
{
    public class InventoryManagerObject : InteractableItem
    {
        [Header("Startup props")]
        [SerializeField] protected SO_InventoryManager _managerType;

        [Header("Manager")]
        [SerializeField] protected InventoryManager _manager;

        [HideInInspector] public InventoryManager Manager => _manager;

        private void Awake()
        {
            if (this._managerType != null)
            {
                this._manager = new InventoryManager(this._managerType, gameObject);
            }
        }

        public void SetManager (InventoryManager manager)
        {
            this._manager = manager;
        }

        public override void LeftClickAction ()
        {
            this._manager.LeftClickGameObjectHandler();
        }

        public override void RightClickAction()
        {
            this._manager.RightClickGameObjectHandler();
        }
    }
}
