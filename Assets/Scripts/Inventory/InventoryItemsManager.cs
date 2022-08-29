using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace VM.Inventory
{
    public class InventoryItemsManager : MonoBehaviour
    {
        public static InventoryItemsManager Instance = null;
        public static UnityEvent<List<InventoryItem>> OnInit = new UnityEvent<List<InventoryItem>>();

        private List<InventoryItem> _items = new List<InventoryItem>();

        public List<InventoryItem> Items => _items;

        private void Awake()
        {
            Instance = this;
            OnInit.Invoke(this._items);
        }
    }
}
