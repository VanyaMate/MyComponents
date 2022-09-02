using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VM.Managers
{
    public class ScreenShotManager : MonoBehaviour
    {
        public static void Make (string fileName)
        {
            ScreenCapture.CaptureScreenshot(fileName);
        }

        public static void Get (string fileName)
        {
            
        }
    }
}