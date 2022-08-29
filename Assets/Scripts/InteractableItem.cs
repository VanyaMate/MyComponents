using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VM
{
    public abstract class InteractableItem : MonoBehaviour
    {
        public abstract void LeftClickAction();
        public abstract void RightClickAction();
    }
}