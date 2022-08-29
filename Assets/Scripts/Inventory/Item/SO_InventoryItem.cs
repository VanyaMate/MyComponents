using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VM.Inventory
{
    [CreateAssetMenu(fileName = "so_inventoryItem_", menuName = "game/create/inventory/inventoryItem/create", order = 51)]
    public class SO_InventoryItem : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private Sprite _icon;
        [SerializeField] private float _maxAmount;
        [SerializeField] private GameObject _prefab;

        public string Name => _name;
        public Sprite Icon => _icon;
        public float MaxAmount => _maxAmount;
        public GameObject Prefab => _prefab;
    }
}
