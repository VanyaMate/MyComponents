using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VM.Inventory
{
    [CreateAssetMenu(fileName = "so_inventoryManager_", menuName = "game/create/inventory/inventoryManager/create", order = 51)]
    public class SO_InventoryManager : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private Sprite _icon;
        [SerializeField] private int _size;
        [SerializeField] private GameObject _prefab;

        public string Name => _name;
        public Sprite Icon => _icon;
        public int Size => _size;
        public GameObject Prefab => _prefab;
    }
}
