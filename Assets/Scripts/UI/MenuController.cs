using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using VM.SceneTools;
using VM.Save;
using System.IO;
using VM.Managers;
using Newtonsoft.Json;

namespace VM.UI
{
    public class MenuController : MonoBehaviour
    {
        public static bool blockOpenMenu = false;
        public static MenuController instance;

        [Header("Geniral")]
        [SerializeField] private Canvas _canvas;

        [Header("Menus")]
        [SerializeField] private GameObject _geniralMenu;
        [SerializeField] private GameObject _loadSaveMenu;

        [Header("Geniral")]
        [SerializeField] private Button _startNewGameButton;
        [SerializeField] private Button _saveGameButton;
        [SerializeField] private Button _loadSaveMenuButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _exitGeniralMenuButton;
        [SerializeField] private Button _exitGameButton;

        [Header("LoadSaveMenu")]
        [SerializeField] private Transform _saveItemsContainer;
        [SerializeField] private Button _loadSaveButton;
        [SerializeField] private Button _backToMenuButton;

        [Header("Prefabs")]
        [SerializeField] private SaveItem _saveItemPrefab;

        private List<GameObject> _menusList = new List<GameObject>();
        private List<Transform> _savedItems = new List<Transform>();

        private void Awake()
        {
            instance = this;   
        }

        private void Start()
        {
            this._menusList.Add(this._geniralMenu);
            this._menusList.Add(this._loadSaveMenu);

            this._CheckInGameMenu();

            // Geniral Menu
            this._startNewGameButton.onClick.AddListener(this._StartNewGame);
            this._loadSaveMenuButton.onClick.AddListener(this._OpenLoadMenu);
            this._exitGameButton.onClick.AddListener(this._ExitGame);

            // Load Menu
            this._loadSaveButton.onClick.AddListener(this._LoadSaveHandler);
            this._backToMenuButton.onClick.AddListener(this._OpenGeniralMenu);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // static
                if (!blockOpenMenu)
                {
                    this._canvas.enabled = !this._canvas.enabled;
                }
                else
                {
                    blockOpenMenu = false;
                }
            }
        }

        public void Hide ()
        {
            this._canvas.enabled = false;
        }

        public void Show ()
        {
            this._canvas.enabled = true;
        }


        // Check
        private void _CheckInGameMenu ()
        {
            if (SceneController.inGameMenu)
            {
                this._saveGameButton.gameObject.SetActive(true);
                this._exitGeniralMenuButton.gameObject.SetActive(true);
                this._saveGameButton.onClick.AddListener(this._MakeSave);
                this._exitGeniralMenuButton.onClick.AddListener(this._LoadGeniralMenu);
            }
            else
            {
                this._saveGameButton.gameObject.SetActive(false);
                this._exitGeniralMenuButton.gameObject.SetActive(false);
            }
        }


        // Handlers
        private void _StartNewGame()
        {
            SceneController.inGameMenu = true;
            SceneController.loadFile = "null";
            SceneManager.LoadScene("Game");
        }

        private void _OpenLoadMenu ()
        {
            this._savedItems.ForEach(x => Destroy(x.gameObject));
            this._savedItems = new List<Transform>();

            this._HideAllMenus();
            this._loadSaveMenu.SetActive(true);

/*            FileInfo[] files = this._GetListOfSaves();
            for (int i = 0; i < files.Length; i++)
            {
                SaveItem item = Instantiate(this._saveItemPrefab, this._saveItemsContainer);
                this._savedItems.Add(item.transform);
                string fileName = files[i].Name;
                item.Set(fileName, () =>
                {
                    SceneController.loadFile = fileName;
                });
            }*/

            List<SaveData> savedData = SaveManager.Instance.GetSaves();
            savedData.ForEach((save) =>
            {
                SaveItem item = Instantiate(this._saveItemPrefab, this._saveItemsContainer);
                this._savedItems.Add(item.transform);

                string shortInfo = DirectoryManager.instance.ReadFile(save.shortInfoSaveFilePath);
                Dictionary<string, string> saveInfo = JsonConvert.DeserializeObject<Dictionary<string, string>>(shortInfo);
                string saveInfoName = saveInfo["name"];

                item.Set(saveInfo["customName"], () =>
                {
                    SceneController.loadFile = $"{ saveInfoName }.txt";
                });
            });
        }

        private void _OpenGeniralMenu ()
        {
            SceneController.loadFile = "null";
            this._HideAllMenus();
            this._geniralMenu.SetActive(true);
        }

        private void _MakeSave ()
        {
            SaveManager.Instance.Save();
        }

        private void _LoadGeniralMenu ()
        {
            SceneController.loadFile = "null";
            SceneController.inGameMenu = false;
            SceneManager.LoadScene("Main");
        }

        private void _LoadSaveHandler ()
        {
            if (SceneController.loadFile != "null")
            {
                if (SceneController.inGameMenu == true)
                {
                    SaveManager.Instance.Load(SceneController.loadFile);
                    this.Hide();
                }
                else
                {
                    SceneController.inGameMenu = true;
                    SceneManager.LoadScene("Game");
                }
            }
        }

        private void _ExitGame ()
        {
            Application.Quit();
        }


        // Utils
        private void _HideAllMenus ()
        {
            this._menusList.ForEach((x) => x.SetActive(false));
        }

        private FileInfo[] _GetListOfSaves ()
        {
            DirectoryInfo info = new DirectoryInfo(SceneController.saveDir);
            return info.GetFiles();
        }
    }
}