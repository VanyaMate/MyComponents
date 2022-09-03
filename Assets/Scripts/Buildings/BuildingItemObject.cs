using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VM.Inventory;

namespace VM.Building
{
    public class BuildingItemObject : InventoryItemObject
    {
        [SerializeField] private List<Transform> _magnitPoints;
        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField] private MeshRenderer _meshRenderer;

        public Mesh mesh => this._meshFilter.sharedMesh;
        public Material material => this._meshRenderer.sharedMaterial;
    }
}