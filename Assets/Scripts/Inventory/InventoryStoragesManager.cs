using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VM.Managers.Save;
using VM.Save;

namespace VM.Inventory
{
    public class InventoryStoragesManager : ObjectToSave
    {
        public static InventoryStoragesManager Instance = null;
        public static UnityEvent<List<InventoryManager>> OnInit = new UnityEvent<List<InventoryManager>>();

        [SerializeField] private Transform _container;

        private List<InventoryManager> _storages = new List<InventoryManager>();

        public List<InventoryManager> Storages => _storages;
        public Transform Container => _container;

        private void Awake()
        {
            Instance = this;
            OnInit.Invoke(this._storages);
        }

        public void FullReset ()
        {
            this._storages.ForEach((storage) =>
            {
                storage.FullReset();
                storage.RemoveFromScene();
            });

            this._storages = new List<InventoryManager>();
        }

        public InventoryManager Add (SO_InventoryManager inventoryType)
        {
            InventoryManager manager = new InventoryManager(inventoryType);
            this._storages.Add(manager);
            return manager;
        }

        public override string GetSaveData()
        {
            List<InventoryManagerSaveData> data = new List<InventoryManagerSaveData>();

            Debug.Log("InventoryStorageCount: " + this._storages.Count);

            this._storages.ForEach((storage) =>
            {
                Debug.Log("stroage saved: " + storage.OnScene.name);
                InventoryManagerSaveData saveData = new InventoryManagerSaveData()
                {
                    managerId = storage.Type.Id,
                    inventory = new Dictionary<int, InventoryItemSaveData>(),
                    position = new SerVector(storage.OnScene.transform.position)
                };

                for (int i = 0; i < storage.Inventory.Count; i++)
                {
                    InventoryItem item = storage.Inventory[i];

                    if (item != null)
                    {
                        saveData.inventory.Add(i, new InventoryItemSaveData()
                        {
                            amount = item.Amount,
                            itemId = item.Type.Id
                        });
                    }
                }

                data.Add(saveData);
            });

            return JsonConvert.SerializeObject(data);
        }

        public override void LoadSaveData(string data)
        {
            List<InventoryManagerSaveData> storagesData = JsonConvert.DeserializeObject<List<InventoryManagerSaveData>>(data);
            
            storagesData.ForEach((storage) =>
            {
                SO_InventoryManager storageType = InventoryListOfTypes.Instance.GetStorageById(storage.managerId);
                InventoryManager manager = new InventoryManager(storageType);
                manager.AddOnScene(new UnityVector3(storage.position).vector);

                Dictionary<int, InventoryItemSaveData> inventory = storage.inventory;

                foreach (KeyValuePair<int, InventoryItemSaveData> pair in inventory)
                {
                    InventoryItemSaveData itemData = pair.Value;
                    SO_InventoryItem itemType = InventoryListOfTypes.Instance.GetItemById(itemData.itemId);
                    InventoryItem item = new InventoryItem(itemType, itemData.amount);
                    manager.AddToPosition(pair.Key, item);
                }
            });
        }
    }
}