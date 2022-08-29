using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VM.Inventory;

namespace VM.UI.Inventory
{
    public class InventoryStorageUI : MonoBehaviour
    {
        [SerializeField] private InventoryItemUI _itemPrefab;

        private InventoryManager _manager;
        private Dictionary<int, InventoryItemUI> _inventoryUI = new Dictionary<int, InventoryItemUI>();
        private UnityAction<Dictionary<int, InventoryItem>> _onChangeAction;

        public InventoryManager Manager => _manager;

        private void Awake()
        {
            this._onChangeAction = (inventory) => this._UpdateData(inventory);
        }

        private void OnDestroy()
        {
            if (this._manager != null)
            {
                this._manager.OnInventoryChange.RemoveListener(this._onChangeAction);
            }
        }

        public void SetStorage(InventoryManager manager)
        {
            if (this._manager != null)
            {
                this._manager.OnInventoryChange.RemoveListener(this._onChangeAction);
            }

            this._manager = manager;
            int inventorySize = this._manager.Inventory.Count;

            for (int i = 0; i < inventorySize; i++)
            {
                this._inventoryUI.Add(i, Instantiate(this._itemPrefab, transform));
                this._inventoryUI[i].SetData(this._manager.Inventory[i], i);
                this._inventoryUI[i].SetStorage(this._manager);
            }

            this._manager.OnInventoryChange.AddListener(this._onChangeAction);
        }

        private void _UpdateData(Dictionary<int, InventoryItem> inventory)
        {
            int inventorySize = inventory.Count;

            for (int i = 0; i < inventorySize; i++)
            {
                this._inventoryUI[i].ResetData();
                this._inventoryUI[i].SetData(inventory[i], i);
            }
        }
    }
}