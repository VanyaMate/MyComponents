using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace VM.UI
{
    public class ContextItem : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _text;

        public void Set (string text, UnityAction action)
        {
            this._text.text = text;
            this._button.onClick.AddListener(() => {
                UserInterface.Instance.ContextMenu.Hide();
                action();
            });
        }

        public void Remove ()
        {
            Destroy(gameObject);
        }
    }
}
