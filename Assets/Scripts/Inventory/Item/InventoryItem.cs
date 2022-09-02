using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VM.UI;
using VM.UI.WindowInfo;

namespace VM.Inventory
{
    [Serializable]
    public class InventoryItem
    {
        [SerializeField] private SO_InventoryItem _itemType;
        [SerializeField] private GameObject _onScene;
        [SerializeField] private float _amount;

        private InventoryManager _storage;

        public UnityEvent<float> OnAmountChange = new UnityEvent<float>();
        public UnityEvent OnDelete = new UnityEvent();

        public SO_InventoryItem Type => _itemType;
        public float Amount
        {
            get => this._amount;
            set {
                this._amount = value;
                this.OnAmountChange.Invoke(this._amount);
            }
        }
        public InventoryManager Storage => _storage;
        public GameObject OnScene => _onScene;

        public InventoryItem (SO_InventoryItem type, float amount, GameObject onScene = null)
        {
            this._itemType = type;
            this._amount = amount;
            this._onScene = onScene;

            if (this._onScene != null)
            {
                this._AddToCommonManager();
            }

            this.OnAmountChange.AddListener((float amount) => this._OnAmountChange());
        }

        public bool MergeWith (InventoryItem item, out float amount)
        {
            float sum = this._amount + item.Amount;

            if (sum > this._itemType.MaxAmount)
            {
                amount = sum - this._itemType.MaxAmount;
                this.Amount = this._itemType.MaxAmount;
                return false;
            }
            else
            {
                amount = 0;
                this.Amount += item.Amount;
                return true;
            }
        }

        public bool Get (float amount)
        {
            if (amount > this._amount)
            {
                return false;
            }
            else
            {
                this.Amount -= amount;
                return true;
            }
        }
        
        public void SetStorage (InventoryManager storage)
        {
            this._storage = storage;
        }

        public void AddOnScene (Vector3 position)
        {
            // если объект не создан - создать
            if (this._onScene == null)
            {
                this._onScene = MonoBehaviour.Instantiate(
                    this._itemType.Prefab.gameObject, 
                    position, 
                    Quaternion.identity
                );

                this._storage = null;
                this._onScene.GetComponent<InventoryItemObject>().SetManager(this);
                this._onScene.transform.parent = InventoryItemsManager.Instance.Container;
                this._AddToCommonManager();
            }

            // переместить объект на позицию спауна
            this._onScene.transform.position = position;
        }

        public void RemoveFromScene(bool deleteFromCommonManager = true)
        {
            if (this._onScene != null)
            {
                MonoBehaviour.Destroy(this._onScene);
                this._onScene = null;

                if (deleteFromCommonManager)
                {
                    this._RemoveFromCommonManager();
                }
            }
        }

        public void LeftClickGameObjectHandler ()
        {
            if (InventoryPlayerPockets.Instance.Manager.Add(this))
            {
                this.RemoveFromScene();
            }
        }

        public void RightClickGameObjectHandler ()
        {
            Dictionary<string, UnityAction> context = new Dictionary<string, UnityAction>();

            context.Add("info", this._OpenInfo);
            context.Add("add to pockets", () => {
                if (InventoryPlayerPockets.Instance.Manager.Add(this))
                {
                    this.RemoveFromScene();
                }
            });

            UserInterface.Instance.ContextMenu.Show(context);
        }

        public void LeftClickUIHandler (InventoryManager manager)
        {
            Debug.Log("Used");
        }

        public void RightClickUIHandler (InventoryManager manager)
        {
            Dictionary<string, UnityAction> context = new Dictionary<string, UnityAction>();

            context.Add("info", this._OpenInfo);

            UserInterface.Instance.ContextMenu.Show(context);
        }

        private void _OpenInfo ()
        {
            Window window = UserInterface.Instance.OpenWindow(this._itemType.Name);
            WindowItemInfo itemInfo = MonoBehaviour.Instantiate(
                UserInterface.Instance.ItemInfo,
                window.WindowContainer
            );

            itemInfo.SetData(this._itemType.Icon, this._itemType.Name, new List<ItemPointData>()
                {
                    new ItemPointData() { Name = "Количество", Value = this._amount.ToString() },
                    new ItemPointData() { Name = "Максимальное", Value = this._itemType.MaxAmount.ToString() },
                });
        }

        private void _OnAmountChange ()
        {
            if (this._amount == 0)
            {
                this.RemoveFromScene();
                InventoryItemsManager.Instance.Items.Remove(this);
                this.OnDelete.Invoke();
            }
        }

        private void _AddToCommonManager()
        {
            if (InventoryItemsManager.Instance != null)
            {
                InventoryItemsManager.Instance.Items.Add(this);
            }
            else
            {
                InventoryItemsManager.OnInit.AddListener((items) => items.Add(this));
            }
        }

        private void _RemoveFromCommonManager ()
        {
            if (InventoryItemsManager.Instance != null)
            {
                InventoryItemsManager.Instance.Items.Remove(this);
            }
            else
            {
                InventoryItemsManager.OnInit.AddListener((items) => items.Remove(this));
            }
        }
    }
}
