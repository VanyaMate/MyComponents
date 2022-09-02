using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using VM.Inventory;
using VM.SceneTools;
using VM.UI.Inventory;
using VM.UI.WindowInfo;

namespace VM.UI
{
    public class UserInterface : MonoBehaviour
    {
        public static UserInterface Instance;

        [Header("Canvas")]
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Transform _windowContainer;

        [Header("Elements")]
        [SerializeField] private ContextMenu _contextMenu;
        [SerializeField] private Window _window;
        [SerializeField] private InventoryItemUI _itemGhost;

        [Header("Window Elements")]
        [SerializeField] private InventoryStorageUI _storageUI;
        [SerializeField] private WindowItemInfo _itemInfo;

        public ContextMenu ContextMenu => _contextMenu;
        public InventoryStorageUI InventoryUI => _storageUI;
        public InventoryItemUI ItemGhost => _itemGhost;
        public WindowItemInfo ItemInfo => _itemInfo;

        private void Awake()
        {
            Instance = this;
            SceneController.inGameMenu = true;
            this._contextMenu.Hide();
        }

        public Window OpenWindow (string windowName)
        {
            Window window = Instantiate(this._window, this._windowContainer);
            window.SetName(windowName);
            return window;
        }
    }
}
