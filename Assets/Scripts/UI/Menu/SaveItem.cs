using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace VM.UI
{
    public class SaveItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text _title;
        [SerializeField] private Button _button;

        public void Set (string title, UnityAction action)
        {
            this._title.text = title;
            this._button.onClick.AddListener(action);
        }
    }
}