using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace VM.UI
{
    public class ContextMenu : MonoBehaviour
    {
        [SerializeField] private ContextItem _itemPrefab;

        private List<ContextItem> _items = new List<ContextItem>();

        public void Show (Dictionary<string, UnityAction> items)
        {
            // Reset
            this.Hide();

            int i = 0;
            foreach (KeyValuePair<string, UnityAction> pair in items)
            {
                ContextItem item = Instantiate(this._itemPrefab, transform);
                item.Set(pair.Key, pair.Value);
                this._items.Add(item);

                i += 1;
            }

            ((RectTransform)transform).sizeDelta = new Vector2(100, i * (25 + 5));
            transform.position = this._GetContextPosition(i);
            gameObject.SetActive(true);
        }

        public void Hide ()
        {
            this._items.ForEach((item) => item.Remove());
            this._items = new List<ContextItem>();
            gameObject.SetActive(false);
        }

        private Vector3 _GetContextPosition(int itemsAmount)
        {
            Vector3 position = Input.mousePosition;
            int contextMenuHeight = itemsAmount * 25;
            float deltaY = position.y - contextMenuHeight;
            float deltaX = Screen.width - position.x - 100;

            if (deltaY < 0)
            {
                position.y -= deltaY - 30;
            }

            if (deltaX < 0)
            {
                position.x += deltaX - 20;
            }

            return position;
        }
    }
}
