using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using VM.UI;

namespace VM.Inventory
{
    public class InventoryItemObject : InteractableItem
    {
        [Header("Startup props")]
        [SerializeField] private SO_InventoryItem _itemType;
        [SerializeField] private float _amount;

        [Header("Manager")]
        [SerializeField] private InventoryItem _manager;

        [HideInInspector] public InventoryItem Manager => _manager;

        private void Awake()
        {
            if (this._itemType != null)
            {
                this._manager = new InventoryItem(this._itemType, this._amount, gameObject);
            }
        }

        public void SetManager (InventoryItem manager)
        {
            this._manager = manager;
        }

        public override void LeftClickAction()
        {
            this._manager.LeftClickGameObjectHandler();
        }

        public override void RightClickAction()
        {
            this._manager.RightClickGameObjectHandler();
        }
    }
}
