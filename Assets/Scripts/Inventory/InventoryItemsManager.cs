using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VM.Managers.Save;
using VM.Save;

namespace VM.Inventory
{
    public class InventoryItemsManager : ObjectToSave
    {
        public static InventoryItemsManager Instance = null;
        public static UnityEvent<List<InventoryItem>> OnInit = new UnityEvent<List<InventoryItem>>();

        [SerializeField] private Transform _container;
        [SerializeField] private List<InventoryItem> _items = new List<InventoryItem>();

        public List<InventoryItem> Items => _items;
        public Transform Container => _container;

        private void Awake()
        {
            Instance = this;
            OnInit.Invoke(this._items);
        }

        public void FullReset()
        {
            this._items.ForEach((item) =>
            {
                item.RemoveFromScene(deleteFromCommonManager: false);
            });

            this._items.RemoveAll(x => true);
            this._items = new List<InventoryItem>();
        }

        public InventoryItem Add(SO_InventoryItem itemType, float amount)
        {
            InventoryItem item = new InventoryItem(itemType, amount);
            this._items.Add(item);
            return item;
        }

        public override string GetSaveData()
        {
            List<InventoryItemSaveData> data = new List<InventoryItemSaveData>();

            this._items.ForEach((item) =>
            {
                InventoryItemSaveData saveData = new InventoryItemSaveData()
                {
                    amount = item.Amount,
                    itemId = item.Type.Id,
                    position = new SerVector(item.OnScene.transform.position)
                };

                data.Add(saveData);
            });

            return JsonConvert.SerializeObject(data);
        }

        public override void LoadSaveData(string data)
        {
            List<InventoryItemSaveData> items = JsonConvert.DeserializeObject<List<InventoryItemSaveData>>(data);
            
            items.ForEach((item) =>
            {
                SO_InventoryItem itemType = InventoryListOfTypes.Instance.GetItemById(item.itemId);

                InventoryItemObject onScene = Instantiate(
                    itemType.Prefab,
                    Vector3.zero,
                    Quaternion.identity
                );

                onScene.SetItemType(itemType, item.amount);
                onScene.Manager.AddOnScene(new UnityVector3(item.position).vector);
            });
        }
    }
}
