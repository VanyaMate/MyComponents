using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace VM.Managers
{
    public class Utils : MonoBehaviour
    {
        public static Utils Instance;
        public static RaycastHit MouseWorldPosition => Utils.Instance._raycastHit;
        public static bool MouseOverGameObject => !EventSystem.current.IsPointerOverGameObject();
        public static GameObject MouseSelectedItem => EventSystem.current ? EventSystem.current.currentSelectedGameObject : null;

        private Camera _mainCamera;
        private RaycastHit _raycastHit;

        private void Awake()
        {
            Instance = this;
            this._mainCamera = Camera.main;
        }

        private void Update()
        {
            this._raycastHit = this._UpdateMouseWorldPosition();
        }

        public RaycastHit _UpdateMouseWorldPosition ()
        {
            Ray ray = this._mainCamera.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out RaycastHit hit);
            return hit;
        }
    }
}
