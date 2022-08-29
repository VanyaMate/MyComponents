using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VM.Managers;

namespace VM.Grid
{
    public class GridManager : MonoBehaviour
    {
        public static GridManager Instance;

        private void Awake()
        {
            Instance = this;
        }

        public Vector3 GetGridPoint (Vector3 position)
        {
            return new Vector3(Mathf.Ceil(position.x), 0, Mathf.Ceil(position.z));
        }
    }
}