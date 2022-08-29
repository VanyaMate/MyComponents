using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace VM.UI
{
    public class Window : InteractableItem, IDragHandler
    {
        [Header("Window elements")]
        [SerializeField] private TMP_Text _windowName;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Transform _windowContainer;

        private Vector2 _beginDragPosition;

        public Transform WindowContainer => _windowContainer;

        private void Awake()
        {
            this._closeButton.onClick.AddListener(() => Destroy(gameObject));
        }

        public void SetName (string name)
        {
            this._windowName.text = name;
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = transform.position + (Vector3)eventData.delta;
        }

        public override void LeftClickAction()
        {
            Debug.Log("LCA");
        }

        public override void RightClickAction()
        {
        }
    }
}
