using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VM.SceneTools
{
    public class SceneController
    {
        public static bool inGameMenu = false;
        public static string loadFile = "null";
        public static string saveDir = Application.persistentDataPath + $"/saves/";
    }
}