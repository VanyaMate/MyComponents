using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VM.Inventory
{
    public class InventoryListOfTypes : MonoBehaviour
    {
        public static InventoryListOfTypes Instance;

        [SerializeField] private List<SO_InventoryItem> _items = new List<SO_InventoryItem>();
        [SerializeField] private List<SO_InventoryManager> _storages = new List<SO_InventoryManager>();

        private void Awake()
        {
            Instance = this;
        }

        public SO_InventoryItem GetItemById (int id)
        {
            return this._items.Find((x) => x.Id == id);
        }

        public SO_InventoryManager GetStorageById (int id)
        {
            return this._storages.Find((x) => x.Id == id);
        }
    }
}