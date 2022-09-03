using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VM.Building;
using VM.Inventory;
using VM.UI;

namespace VM.Inventory.Items
{
    public class InventoryItemBuilding : InventoryItem
    {
        protected new SO_InventoryBuildingItem _itemType;
        private bool _active;
        private bool _placed;

        public InventoryItemBuilding(SO_InventoryBuildingItem type, float amount, GameObject onScene = null) : base(type, amount, onScene)
        {
            this._itemType = type;
            this._active = false;
            this._placed = false;
        }

        public override void LeftClickGameObjectHandler()
        {
            if (this._placed)
            {
                Debug.Log("click");
            }
            else
            {
                base.LeftClickGameObjectHandler();
            }
        }

        public override void RightClickGameObjectHandler()
        {
            if (this._placed)
            {
                Dictionary<string, UnityAction> context = new Dictionary<string, UnityAction>();

                context.Add("info", this._OpenInfo);
                context.Add("destroy", () => {
                    if (InventoryPlayerPockets.Instance.Manager.Add(this))
                    {
                        this.RemoveBuildFromScene();
                    }
                });

                UserInterface.Instance.ContextMenu.Show(context);
            }
            else
            {
                base.RightClickGameObjectHandler();
            }
        }

        public override void LeftClickUIHandler(InventoryManager manager)
        {
            if (!this._active)
            {
                BuildingManager.instance.ShowGhost(this);
            }
            else
            {
                BuildingManager.instance.HideGhost();
            }

            this._active = !this._active;
        }

        public void PlaceOnScene (Vector3 position)
        {
            this._placed = true;

            // если объект не создан - создать
            if (this._onScene == null)
            {
                this._onScene = MonoBehaviour.Instantiate(
                    ((SO_InventoryBuildingItem)this._itemType).BuildingPrefab.gameObject,
                    position,
                    Quaternion.identity
                );

                this._storage = null;
                this._onScene.GetComponent<BuildingItemObject>().SetManager(this);
                this._AddToBuildingManager();
            }

            // переместить объект на позицию спауна
            this._onScene.transform.parent = InventoryItemsManager.Instance.Container;
            this._onScene.transform.position = position;
        }

        public void RemoveBuildFromScene(bool deleteFromCommonManager = true)
        {
            this._placed = false;

            if (this._onScene != null)
            {
                MonoBehaviour.Destroy(this._onScene);
                this._onScene = null;

                if (deleteFromCommonManager)
                {
                    this._RemoveFromBuildingManager();
                }
            }
        }

        private void _AddToBuildingManager()
        {
            if (BuildingManager.instance != null)
            {
                BuildingManager.instance.Items.Add(this);
            }
            else
            {
                BuildingManager.OnInit.AddListener((items) => items.Add(this));
            }
        }

        private void _RemoveFromBuildingManager()
        {
            if (BuildingManager.instance != null)
            {
                BuildingManager.instance.Items.Remove(this);
            }
            else
            {
                BuildingManager.OnInit.AddListener((items) => items.Remove(this));
            }
        }
    }
}