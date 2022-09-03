using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VM.Inventory;
using VM.Managers;

namespace VM.UI.Inventory
{
    public class InventoryItemUI : 
        MonoBehaviour, 
        IBeginDragHandler, 
        IDragHandler, 
        IEndDragHandler, 
        IPointerEnterHandler,
        IPointerExitHandler,
        IPointerClickHandler
    {
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _amount;

        private UnityAction<float> _onChangeAction;
        private InventoryItem _item;
        private InventoryItemUI _ghost;
        private InventoryManager _storage;
        public int _position;

        public InventoryItem Item => _item;
        public InventoryManager Storage => _storage;
        public int Position => _position;

        private void Awake()
        {
            this._onChangeAction = (float amount) => this._SetAmount(amount);
        }

        private void Start()
        {
            this._ghost = UserInterface.Instance.ItemGhost;
        }

        public void SetData (InventoryItem item, int position)
        {
            this._position = position;

            if (item != null)
            {
                this._item = item;
                this._image.sprite = item.Type.Icon;
                this._image.color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
                this._SetAmount(item.Amount);

                item.OnAmountChange.AddListener(this._onChangeAction);
            }
        }

        public void SetStorage (InventoryManager manager)
        {
            this._storage = manager;
        }

        public void ResetData ()
        {
            this._image.sprite = null;
            this._image.color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 10f / 255f);
            this._amount.text = "";

            if (this._item != null)
            {
                this._item.OnAmountChange.RemoveListener(this._onChangeAction);
                this._item = null;
            }
        }

        public void SetGhostData (InventoryItem item)
        {
            if (item != null)
            {
                this._item = item;
                this._image.sprite = item.Type.Icon;
                this._image.color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 150f / 255f);
                this._SetAmount(item.Amount);

                item.OnAmountChange.AddListener(this._onChangeAction);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (this._item != null)
            {
                this._ghost.gameObject.SetActive(true);
                this._ghost.SetGhostData(this._item);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (this._item != null)
            {
                this._ghost.transform.position = eventData.position + new Vector2(.5f, .5f);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (this._item != null)
            {
                this._ghost.gameObject.SetActive(false);

                GameObject hoveredUI = eventData.hovered.Find((x) => x.GetComponent<InventoryItemUI>() != null);

                if (hoveredUI)
                {
                    this._UIManipulation(hoveredUI);
                }
                else
                {
                    this._GameWorldManipulation(Utils.MouseWorldPosition);
                }
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {

        }

        public void OnPointerExit(PointerEventData eventData)
        {

        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (this._item != null)
            {
                if (eventData.button == PointerEventData.InputButton.Left)
                {
                    this._item.LeftClickUIHandler(this._storage);
                }
                else if (eventData.button == PointerEventData.InputButton.Right)
                {
                    this._item.RightClickUIHandler(this._storage);
                }
            }
        }
        
        private void _SetAmount (float amount)
        {
            if (this._item != null)
            {
                if (amount > 0)
                {
                    this._amount.text = amount.ToString();
                }
                else
                {
                    this._item.OnAmountChange.RemoveListener(this._onChangeAction);
                    this.ResetData();
                }
            }
        }

        private void _UIManipulation (GameObject hoveredUI)
        {
            hoveredUI.TryGetComponent<InventoryItemUI>(out InventoryItemUI itemUI);

            if (itemUI != null && itemUI != this && itemUI != this._ghost)
            {
                InventoryItem item = this._storage.Get(this._position);
                
                if (itemUI.Storage.AddToPosition(itemUI.Position, item))
                {
                    Debug.Log("Added");
                }
                else
                {
                    InventoryItem invItem = itemUI.Storage.Inventory[itemUI.Position];
                    if (invItem != null)
                    {
                        if (invItem.Type == item.Type)
                        {
                            if (invItem.MergeWith(item, out float amount))
                            {
                                this._storage.Get<InventoryItem>(item);
                            }
                            else
                            {
                                item.Amount = amount;
                            }
                        }
                        else
                        {
                            // Swap
                            Debug.Log("Swap");
                            InventoryItem itemFrom = this._storage.Get<InventoryItem>(item);
                            InventoryItem itemTo = itemUI.Storage.Get<InventoryItem>(itemUI.Item);

                            Debug.Log("Item: " + item);
                            Debug.Log("InvItem: " + invItem);
                            Debug.Log("ItemFrom: " + itemFrom);
                            Debug.Log("ItemTo: " + itemTo);

                            itemUI.Storage.AddToPosition(itemUI.Position, item);
                            this._storage.AddToPosition(this.Position, itemTo);
                        }
                    }
                    else
                    {
                        this._storage.AddToPosition(this._position, item);
                    }
                }
            }
        }

        private void _GameWorldManipulation (RaycastHit hit)
        {
            if (hit.transform)
            {
                InventoryItem item = this._storage.Get<InventoryItem>(this._item);
                
                if (hit.transform.TryGetComponent<InventoryManagerObject>(out InventoryManagerObject manager))
                {
                    if (!manager.Manager.Add(item))
                    {
                        this._storage.AddToPosition(this._position, item);
                    }
                }
                else
                {
                    item.AddOnScene(hit.point);
                }
            }
            else
            {
                Debug.Log("EmptyZone");
            }
        }
    }
}
