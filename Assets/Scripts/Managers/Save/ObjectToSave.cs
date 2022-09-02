using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VM.Managers.Save
{
    public abstract class ObjectToSave : MonoBehaviour
    {
        public abstract string GetSaveData();
        public abstract void LoadSaveData(string data);
    }
}
