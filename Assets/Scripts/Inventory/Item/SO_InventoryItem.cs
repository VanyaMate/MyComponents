using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VM.Inventory
{
    [CreateAssetMenu(fileName = "so_inventoryItem_", menuName = "game/create/inventory/inventoryItem/create", order = 51)]
    public class SO_InventoryItem : ScriptableObject
    {
        [SerializeField] private string _type;
        [SerializeField] private int _id;
        [SerializeField] private string _name;
        [SerializeField] private Sprite _icon;
        [SerializeField] private float _maxAmount;
        [SerializeField] private InventoryItemObject _prefab;

        public string Type => _type;
        public int Id => _id;
        public string Name => _name;
        public Sprite Icon => _icon;
        public float MaxAmount => _maxAmount;
        public InventoryItemObject Prefab => _prefab;
    }
}
