using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VM.Building;
using VM.Inventory;

[CreateAssetMenu(fileName = "so_inventoryItem_", menuName = "game/create/inventory/buildingItem/create", order = 51)]
public class SO_InventoryBuildingItem : SO_InventoryItem
{
    [SerializeField] private BuildingItemObject _buildingPrefab;

    public BuildingItemObject BuildingPrefab => _buildingPrefab;
}
