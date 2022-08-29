using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using VM.Inventory;
using VM.UI;

namespace VM.Managers.Mouse
{
    public class InteractManager : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && Utils.MouseOverGameObject)
            {
                UserInterface.Instance.ContextMenu.Hide();
                RaycastHit hit = Utils.Instance._UpdateMouseWorldPosition();

                if (hit.transform && hit.transform.TryGetComponent<InteractableItem>(out InteractableItem item))
                {
                    item.LeftClickAction();
                }
            }
            else if (Input.GetMouseButtonDown(1) && Utils.MouseOverGameObject)
            {
                UserInterface.Instance.ContextMenu.Hide();
                RaycastHit hit = Utils.MouseWorldPosition;

                if (hit.transform && hit.transform.TryGetComponent<InteractableItem>(out InteractableItem item))
                {
                    item.RightClickAction();
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UserInterface.Instance.ContextMenu.Hide();
            }
        }
    }
}
