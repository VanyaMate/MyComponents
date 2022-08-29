using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using VM.TerrainTools;
using Newtonsoft.Json;
using VM.Inventory;
using VM.Player;

namespace VM.Save
{
    public class SerVector
    {
        public float x;
        public float y;
        public float z;
     
        public SerVector(Vector3 vector3)
        {
            this.x = vector3.x;
            this.y = vector3.y;
            this.z = vector3.z;
        }
    }

    public class InventoryItemSaveData
    {
        public float amount;
        public string itemType;
        public SerVector position;
    }

    public class InventoryManagerSaveData
    {
        public string managerType;
        public Dictionary<int, InventoryItemSaveData> inventory;
        public SerVector position;
    }

    public class PlayerSaveData
    {
        public SerVector position;
        public InventoryManagerSaveData pockets;
    }

    public class TerrainSaveData
    {
        public float[,] heights;
        public float[,,] colors;
        public Dictionary<int, int[,]> details;
    }

    public class SaveManager : MonoBehaviour
    {
        public static SaveManager Instance;

        [SerializeField] private string _saveDir;

        private string _dir;

        private void Awake()
        {
            Instance = this;
            this._dir = Application.persistentDataPath + $"/{ this._saveDir }";
        }

        public void Save ()
        {
            DateTime time = new DateTime();
            string fileName = time.Millisecond.ToString();

            this._SaveTerrainData("0");
            this._SavePlayerData("0");
        }

        public void Load()
        {
            this._LoadTerrainData("0");
        }

        private void _SaveTerrainData (string fileName)
        {
            TerrainSaveData terrainData = TerrainManager.Instance.GetTerrainSaveData();
            string filePath = this._dir + $"/{fileName}-terrainData.txt";

            using (StreamWriter sw = new StreamWriter(filePath))
            {
                sw.WriteLine(JsonConvert.SerializeObject(terrainData));
            }
        }

        private void _LoadTerrainData (string fileName)
        {
            string filePath = this._dir + $"/{fileName}-terrainData.txt";
            string saveData = File.ReadAllText(filePath);
            TerrainSaveData terrainData = JsonConvert.DeserializeObject<TerrainSaveData>(saveData);

            TerrainManager.Instance.LoadTerrainSaveData(terrainData);
        }

        private void _SavePlayerData (string fileName)
        {
            PlayerSaveData playerData = PlayerManager.Instance.GetPlayerSaveData();
            string filePath = this._dir + $"/{fileName}-playerData.txt";

            Debug.Log(filePath);
            
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                sw.WriteLine(JsonConvert.SerializeObject(playerData));
            }
        }

        private void _LoadPlayerData (string fileName)
        {

        }

        private void _SaveInventoryData (string fileName)
        {

        }

        private void _LoadInventoryData (string fileName)
        {

        }
    }
}