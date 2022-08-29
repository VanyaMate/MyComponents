using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace VM.UI.WindowInfo
{
    public class ItemPointData
    {
        public string Name;
        public string Value;
    }

    public class WindowItemInfo : MonoBehaviour
    {
        [Header("Inside Items")]
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private Transform _pointsList;

        [Header("Created Components")]
        [SerializeField] private ItemInfoPoint _itemPoint;

        public void SetData (Sprite icon, string name, List<ItemPointData> points)
        {
            this._icon.sprite = icon;
            this._name.text = name;

            points.ForEach((point) =>
            {
                ItemInfoPoint infoPoint = Instantiate(this._itemPoint, this._pointsList);
                infoPoint.SetData(point.Name, point.Value);
            });
        }
    }
}
