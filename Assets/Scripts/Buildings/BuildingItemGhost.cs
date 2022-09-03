using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VM.Building
{
    public class BuildingItemGhost : MonoBehaviour
    {
        [Header("Mesh components")]
        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField] private MeshRenderer _meshRenderer;

        [Header("Material status")]
        [SerializeField] private Material _successStatus;
        [SerializeField] private Material _deniedStatus;

        [Header("Props")]
        public Vector3 position;
        public bool status;

        public void SetPosition(Vector3 position)
        {
            transform.position = this.position = position;
        }

        public void SetMesh (Mesh mesh = null)
        {
            this._meshFilter.mesh = mesh ? mesh : this._meshFilter.mesh;
        }

        public void SetStatus (bool status)
        {
            this.status = status;
            this._meshRenderer.material = status ? this._successStatus : this._deniedStatus;
        }
    }
}