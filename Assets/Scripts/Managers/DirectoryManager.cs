using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace VM.Managers
{
    public class DirectoryManager : MonoBehaviour
    {
        public static DirectoryManager instance;

        [Header("Paths")]
        public string dir;
        public string saveDir;
        public string shortSaveDir;
        public string screenShotsDir;

        private void Awake()
        {
            instance = this;

            this.dir = Application.persistentDataPath;
            this.saveDir = $"{ this.dir }/saves";
            this.shortSaveDir = $"{ this.dir }/shortInfo";
            this.screenShotsDir = $"{ this.dir }/screenShots";
        }

        public List<string> GetFilesNameFromDir (string dirPath)
        {
            List<string> filesName = new List<string>();

            if (Directory.Exists(dirPath))
            {
                DirectoryInfo info = new DirectoryInfo(dirPath);
                FileInfo[] files = info.GetFiles();
                
                for (int i = 0; i < files.Length; i++)
                {
                    filesName.Add(files[i].Name);
                }
            }

            return filesName;
        }

        public void CreateDir (string dir, bool fileEnd = false)
        {
            string[] path = dir.Split('/');
            string checkPath = "";

            for (int i = 0; i < path.Length - (fileEnd ? 1 : 0); i++)
            {
                checkPath += path[i];

                if (!Directory.Exists(checkPath))
                {
                    Directory.CreateDirectory(checkPath);
                }

                checkPath += "/";
            }
        }

        public void RemoveFile (string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public void WriteFile (string filePath, string value)
        {
            this.CreateDir(filePath, true);

            using (StreamWriter sw = new StreamWriter(filePath))
            {
                sw.WriteLine(value);
            }
        }

        public string ReadFile (string filePath)
        {
            return File.Exists(filePath) ? File.ReadAllText(filePath) : "";
        }
    }
}