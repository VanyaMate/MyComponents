using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using VM.TerrainTools;
using Newtonsoft.Json;
using VM.Inventory;
using VM.Player;
using VM.SceneTools;
using VM.Managers;
using VM.Managers.Save;
using VM.UI;

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

    public class UnityVector3
    {
        public Vector3 vector;

        public UnityVector3 (SerVector serVector)
        {
            this.vector = new Vector3(serVector.x, serVector.y, serVector.z);
        }
    }

    public class InventoryItemSaveData
    {
        public float amount;
        public int itemId;
        public SerVector position;
    }

    public class InventoryManagerSaveData
    {
        public int managerId;
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

    public class SaveData
    {
        public string geniralSaveFilePath;
        public string shortInfoSaveFilePath;
        public string screenShotSaveFilePath;
    }

    public class SaveManager : MonoBehaviour
    {
        public static SaveManager Instance;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            if (SceneController.loadFile != "null")
            {
                this.Load(SceneController.loadFile);
            }
        }

        public void Save ()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();

            data.Add("terrain", this._SaveData(TerrainManager.Instance));
            data.Add("player", this._SaveData(PlayerManager.Instance));
            data.Add("storages", this._SaveData(InventoryStoragesManager.Instance));
            data.Add("items", this._SaveData(InventoryItemsManager.Instance));

            string fileName = DateTime.Now.ToString("MM_dd_yyyy-HH_mm_ss");
            string saveDir = DirectoryManager.instance.saveDir;
            string shortDir = DirectoryManager.instance.shortSaveDir;
            string screenDir = DirectoryManager.instance.screenShotsDir;

            Dictionary<string, string> shortInfo = new Dictionary<string, string>();
            shortInfo.Add("date", fileName);
            shortInfo.Add("name", fileName);
            shortInfo.Add("customName", fileName + "!");

            DirectoryManager.instance.WriteFile(
                $"{saveDir}/{fileName}.txt", 
                JsonConvert.SerializeObject(data)
            );

            DirectoryManager.instance.WriteFile(
                $"{shortDir}/{fileName}.txt",
                JsonConvert.SerializeObject(shortInfo)
            );

            DirectoryManager.instance.CreateDir(screenDir);
            MenuController.instance.Hide();
            ScreenShotManager.Make($"{screenDir}/{fileName}.png");
        }

        public void Load(string fileName)
        {
            string filePath = DirectoryManager.instance.saveDir + $"/{fileName}";
            string save = DirectoryManager.instance.ReadFile(filePath);

            if (save != "")
            {
                Dictionary<string, string> data = JsonConvert.DeserializeObject<Dictionary<string, string>>(save);

                InventoryStoragesManager.Instance.FullReset();
                InventoryItemsManager.Instance.FullReset();

                this._LoadData(TerrainManager.Instance, data["terrain"]);
                this._LoadData(PlayerManager.Instance, data["player"]);
                this._LoadData(InventoryStoragesManager.Instance, data["storages"]);
                this._LoadData(InventoryItemsManager.Instance, data["items"]);
            }
        }

        public List<SaveData> GetSaves ()
        {
            List<SaveData> saves = new List<SaveData>();
            List<string> saveNames = DirectoryManager.instance.GetFilesNameFromDir(DirectoryManager.instance.saveDir);

            saveNames.ForEach((save) =>
            {
                string saveName = save.Split(".")[0];
                string geniralPath = DirectoryManager.instance.saveDir + $"{save}";
                string shortFilePath = DirectoryManager.instance.shortSaveDir + $"/{save}";
                string screenPath = DirectoryManager.instance.screenShotsDir + $"/{saveName}.png";

                if (File.Exists(shortFilePath) && File.Exists(screenPath))
                {
                    SaveData data = new SaveData()
                    {
                        geniralSaveFilePath = geniralPath,
                        shortInfoSaveFilePath = shortFilePath,
                        screenShotSaveFilePath = screenPath
                    };

                    saves.Add(data);
                }
            });

            return saves;
        }

        private string _SaveData (ObjectToSave objectToSave)
        {
            return objectToSave.GetSaveData();
        }

        private void _LoadData (ObjectToSave objectToSave, string data)
        {
            objectToSave.LoadSaveData(data);
        }
    }
}