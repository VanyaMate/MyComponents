using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VM.UI;
using VM.UI.Inventory;

namespace VM.Inventory
{
    [Serializable]
    public class InventoryManager
    {
        [SerializeField] private SO_InventoryManager _inventoryType;
        [SerializeField] private GameObject _onScene;

        private Dictionary<int, InventoryItem> _inventory = new Dictionary<int, InventoryItem>();

        public SO_InventoryManager Type => _inventoryType;
        public Dictionary<int, InventoryItem> Inventory => this._inventory;

        public UnityEvent<Dictionary<int, InventoryItem>> OnInventoryChange = 
            new UnityEvent<Dictionary<int, InventoryItem>>();

        public InventoryManager (SO_InventoryManager type, GameObject onScene = null)
        {
            this._inventory = new Dictionary<int, InventoryItem>();
            this._inventoryType = type;
            this._onScene = onScene;

            if (InventoryStoragesManager.Instance != null)
            {
                InventoryStoragesManager.Instance.Storages.Add(this);
            }
            else
            {
                InventoryStoragesManager.OnInit.AddListener((storages) => storages.Add(this));
            }

            for (int i = 0; i < this._inventoryType.Size; i++)
            {
                this._inventory[i] = null;
            }
        }

        public bool Add (InventoryItem item)
        {
            bool addToEmptySlot = this.AddToFirstEmptySlot(item);

            if (!addToEmptySlot)
            {
                bool addToSameSlot = this.AddToSameSlot(item);

                if (!addToEmptySlot)
                {
                    return false;
                }
            }

            item.SetStorage(this);
            return true;
        }

        public bool AddToPosition (int position, InventoryItem item)
        {
            if (this._inventory.TryGetValue(position, out InventoryItem currentItem))
            {
                if (currentItem == null)
                {
                    this._inventory[position] = item;
                    item.SetStorage(this);
                    this.OnInventoryChange.Invoke(this._inventory);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        public InventoryItem GetItemByPosition (int position)
        {
            this._inventory.TryGetValue(position, out InventoryItem item);
            return item;
        }

        public bool AddToFirstEmptySlot (InventoryItem item)
        {
            List<int> emptySlots = this._GetIndexEmptySlots();

            if (emptySlots.Count != 0)
            {
                foreach (int index in emptySlots)
                {
                    this._inventory[index] = item;
                    break;
                }

                this.OnInventoryChange.Invoke(this._inventory);
                return true;
            }

            return false;
        }

        public bool AddToSameSlot (InventoryItem item)
        {
            List<int> sameSlots = this._GetIndexSameItems(item.Type);

            if (sameSlots.Count != 0)
            {
                bool added = false;

                sameSlots.ForEach((index) =>
                {
                    if (this._inventory[index].MergeWith(item, out float amount))
                    {
                        added = true;
                        this.OnInventoryChange.Invoke(this._inventory);
                        return;
                    }
                    else
                    {
                        item.Amount -= amount;
                    }
                });

                this.OnInventoryChange.Invoke(this._inventory);
                return added;
            }

            return false;
        }

        public void AddOnScene (Vector3 position)
        {
            // ���� ������ �� ������ - �������
            if (this._onScene == null)
            {
                this._onScene = MonoBehaviour.Instantiate(
                    this._inventoryType.Prefab,
                    position,
                    Quaternion.identity
                );

                this._onScene.GetComponent<InventoryManagerObject>().SetManager(this);
            }

            // ����������� ������ �� ������� ������
            this._onScene.transform.position = position;
        }

        public InventoryItem Get (int position)
        {
            InventoryItem item = this._inventory[position];
            this._inventory[position] = null;
            item.SetStorage(null);
            this.OnInventoryChange.Invoke(this._inventory);
            return item;
        }

        public InventoryItem Get (InventoryItem item)
        {
            InventoryItem _item = null;
            int position = -1;

            for (int i = 0; i < this._inventory.Count; i++)
            {
                if (this._inventory[i] == item)
                {
                    position = i;
                    _item = item;
                    break;
                }
            }

            if (_item != null)
            {
                this._inventory[position] = null;
                this.OnInventoryChange.Invoke(this._inventory);
                _item.SetStorage(null);
                return _item;
            }
            else
            {
                return null;
            }
        }

        public void LeftClickGameObjectHandler ()
        {
            this._OpenWindow();
        }

        public void RightClickGameObjectHandler ()
        {
            Dictionary<string, UnityAction> context = new Dictionary<string, UnityAction>();

            context.Add("use", () => { Debug.Log("use item: " + this._inventoryType.Name); });
            context.Add("remove", () => { Debug.Log("remove item: " + this._inventoryType.Name); });
            context.Add("open", this._OpenWindow);

            UserInterface.Instance.ContextMenu.Show(context);
        }

        public void LeftClickUIHandler ()
        {

        }

        public void RightClickUIHandler ()
        {

        }

        public void RemoveFromScene ()
        {
            if (this._onScene != null)
            {
                MonoBehaviour.Destroy(this._onScene);
            }
        }

        public void Remove ()
        {
            this.RemoveFromScene();
            InventoryStoragesManager.Instance.Storages.Remove(this);
        }

        private void _OpenWindow()
        {
            Window window = UserInterface.Instance.OpenWindow(this._inventoryType.Name);
            InventoryStorageUI storageUI = MonoBehaviour.Instantiate(
                UserInterface.Instance.InventoryUI,
                window.WindowContainer
            );

            storageUI.SetStorage(this);
        }

        private List<int> _GetIndexEmptySlots ()
        {
            List<int> emptySlots = new List<int>();

            for (int i = 0; i < this._inventory.Count; i++)
            {
                if (this._inventory.TryGetValue(i, out InventoryItem item))
                {
                    if (item == null)
                    {
                        emptySlots.Add(i);
                    }
                }
            }

            return emptySlots;
        }

        private List<int> _GetIndexSameItems (SO_InventoryItem itemType)
        {
            List<int> sameItems = new List<int>();

            for (int i = 0; i < this._inventory.Count; i++)
            {
                if (this._inventory.TryGetValue(i, out InventoryItem item))
                {
                    if (item != null && item.Type == itemType)
                    {
                        sameItems.Add(i);
                    }
                }
            }

            return sameItems;
        }
    }
}
