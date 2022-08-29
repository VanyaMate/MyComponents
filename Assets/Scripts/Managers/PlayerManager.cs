using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VM.Inventory;
using VM.Save;

namespace VM.Player
{
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager Instance;

        private void Awake()
        {
            Instance = this;
        }

        public PlayerSaveData GetPlayerSaveData ()
        {
            PlayerSaveData playerSaveData = new PlayerSaveData();
            InventoryManager playerInventory = InventoryPlayerPockets.Instance.Manager;

            playerSaveData.position = new SerVector(transform.position);
            playerSaveData.pockets = new InventoryManagerSaveData()
            {
                managerType = playerInventory.Type.Name,
                inventory = new Dictionary<int, InventoryItemSaveData>(),
                position = new SerVector(Vector3.zero)
            };

            for (int i = 0; i < playerInventory.Inventory.Count; i++)
            {
                InventoryItem item = playerInventory.Inventory[i];

                if (item != null)
                {
                    InventoryItemSaveData itemSaveData = new InventoryItemSaveData()
                    {
                        amount = item.Amount,
                        itemType = item.Type.Name,
                        position = new SerVector(Vector3.zero)
                    };

                    playerSaveData.pockets.inventory.Add(i, itemSaveData);
                }
            }

            return playerSaveData;
        }
    }
}