using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VM.Inventory;

namespace VM.UI.Inventory
{
    public class PlayerPocketsUI : MonoBehaviour
    {
        [SerializeField] private InventoryPlayerPockets _storage;
        [SerializeField] private InventoryItemUI _itemPrefab;

        private Dictionary<int, InventoryItemUI> _inventoryUI = new Dictionary<int, InventoryItemUI>();
        private UnityAction<Dictionary<int, InventoryItem>> _onChangeAction;

        private void Awake()
        {
             this._onChangeAction = (inventory) => this._UpdateData(inventory);
        }

        private void Start()
        {
            int inventorySize = this._storage.Manager.Inventory.Count;

            for (int i = 0; i < inventorySize; i++)
            {
                this._inventoryUI.Add(i, Instantiate(this._itemPrefab, transform));
                this._inventoryUI[i].transform.name = "item:" + i;
                this._inventoryUI[i].SetData(this._storage.Manager.Inventory[i], i);
                this._inventoryUI[i].SetStorage(this._storage.Manager);
            }

            ((RectTransform)transform).sizeDelta = new Vector2(inventorySize * 60 ,60);
            this._storage.Manager.OnInventoryChange.AddListener(this._onChangeAction);
        }

        private void _UpdateData (Dictionary<int, InventoryItem> inventory)
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
