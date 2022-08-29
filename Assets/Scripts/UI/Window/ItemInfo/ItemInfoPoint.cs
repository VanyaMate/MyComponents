using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace VM.UI.WindowInfo
{
    public class ItemInfoPoint : MonoBehaviour
    {
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _value;

        public void SetData (string name, string value)
        {
            this._name.text = name;
            this._value.text = value;
        }

        public void AddListener<T> (UnityEvent<T> listener)
        {
            listener.AddListener((x) => this._value.text = x.ToString());
        }
    }
}