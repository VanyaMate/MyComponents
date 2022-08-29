using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VM.TerrainGenerator
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
    public class _SAVED_TerrainManager : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private int _xSize;
        [SerializeField] private int _zSize;

        private Mesh _mesh;
        private MeshCollider _collider;

        private Vector3[] _vertices;
        private int[] _triangles;

        private void Awake()
        {
            this._mesh = GetComponent<MeshFilter>().mesh;
            this._collider = GetComponent<MeshCollider>();
            this._vertices = new Vector3[(this._xSize + 1) * (this._zSize + 1)];
            this._triangles = new int[this._xSize * this._zSize * 6];

            transform.position = new Vector3(-this._xSize / 2, 0, -this._zSize / 2);

            // Col
            for (int i = 0, z = 0; z <= this._zSize; z++)
            {
                // Row
                for (int x = 0; x <= this._xSize; x++, i++)
                {
                    float y = 0;
                    this._vertices[i] = new Vector3(x, y, z);
                }
            }

            int triangle = 0;
            int vert = 0;

            // Col
            for (int z = 0; z < this._zSize; z++)
            {
                // Row
                for (int x = 0; x < this._xSize; x++)
                {
                    this._triangles[triangle + 0] = vert;
                    this._triangles[triangle + 1] = vert + this._xSize + 1;
                    this._triangles[triangle + 2] = vert + 1;
                    this._triangles[triangle + 3] = vert + 1;
                    this._triangles[triangle + 4] = vert + this._xSize + 1;
                    this._triangles[triangle + 5] = vert + this._xSize + 2;

                    triangle += 6;
                    vert += 1;
                }

                vert += 1;
            }

            this._mesh.SetVertices(this._vertices);
            this._mesh.SetTriangles(this._triangles, 0);
            this._collider.sharedMesh = this._mesh;
        }

        private void OnDrawGizmos()
        {
            if (this._vertices != null)
            {
                for (int i = 0; i < this._vertices.Length; i++)
                {
                    Gizmos.DrawSphere(this._vertices[i] - new Vector3(this._xSize / 2, 0, this._zSize / 2), .1f);
                }
            }
        }
    }
}
