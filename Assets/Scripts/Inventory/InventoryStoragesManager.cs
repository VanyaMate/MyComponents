using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace VM.Inventory
{
    public class InventoryStoragesManager : MonoBehaviour
    {
        public static InventoryStoragesManager Instance = null;
        public static UnityEvent<List<InventoryManager>> OnInit = new UnityEvent<List<InventoryManager>>();

        private List<InventoryManager> _storages = new List<InventoryManager>();

        public List<InventoryManager> Storages => _storages;

        private void Awake()
        {
            Instance = this;
            OnInit.Invoke(this._storages);
        }
    }
}