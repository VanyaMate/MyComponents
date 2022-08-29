using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VM.TerrainGenerator
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
    public class _LIST_TerrainManager : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private int _xSize;
        [SerializeField] private int _zSize;
        [SerializeField] private Sprite _texture;

        private Mesh _mesh;
        private Material _material;
        private MeshFilter _filter;
        private MeshRenderer _renderer;
        private MeshCollider _collider;

        private List<Vector3> _vertices;
        private List<int> _triangles;
        private List<Color> _colors;
        private List<Vector2> _uvs;
        private List<Vector3> _normals;
        private List<Vector4> _tangents;

        private void Awake()
        {
            this._filter = GetComponent<MeshFilter>();
            this._renderer = GetComponent<MeshRenderer>();
            this._mesh = this._filter.mesh;
            this._material = this._renderer.material;
            this._collider = GetComponent<MeshCollider>();

            this._RenderMesh();
        }

        private void _RenderTexture ()
        {

        }

        private void _RenderMesh ()
        {
            this._vertices = new List<Vector3>();
            this._triangles = new List<int>();
            this._colors = new List<Color>();
            this._uvs = new List<Vector2>();
            this._normals = new List<Vector3>();
            this._tangents = new List<Vector4>();

            transform.position = new Vector3(-this._xSize / 2, 0, -this._zSize / 2);

            // Col
            for (int i = 0, z = 0; z <= this._zSize; z++)
            {
                // Row
                for (int x = 0; x <= this._xSize; x++, i++)
                {
                    float y = 0;
                    this._vertices.Add(new Vector3(x, y, z));
                    this._colors.Add(x % 2 == 0 ? Color.white : Color.black);
                    this._uvs.Add(new Vector2(z, x));
                    this._normals.Add(Vector3.up);
                    this._tangents.Add(new Vector4(1, 0, 0, -1));
                }
            }

            int vert = 0;

            // Col
            for (int z = 0; z < this._zSize; z++)
            {
                // Row
                for (int x = 0; x < this._xSize; x++)
                {
                    this._triangles.Add(vert);
                    this._triangles.Add(vert + this._xSize + 1);
                    this._triangles.Add(vert + 1);
                    this._triangles.Add(vert + 1);
                    this._triangles.Add(vert + this._xSize + 1);
                    this._triangles.Add(vert + this._xSize + 2);

                    vert += 1;
                }

                vert += 1;
            }

            this._mesh.Clear();
            this._mesh.SetVertices(this._vertices);
            this._mesh.SetTriangles(this._triangles, 0);
            this._mesh.SetColors(this._colors);
            this._mesh.SetNormals(this._normals);
            this._mesh.SetTangents(this._tangents);
            this._mesh.SetUVs(0, this._uvs);
            this._collider.sharedMesh = this._mesh;
        }

        private void OnDrawGizmos()
        {
            if (this._vertices != null)
            {
                for (int i = 0; i < this._vertices.Count; i++)
                {
                    Gizmos.DrawSphere(this._vertices[i] - new Vector3(this._xSize / 2, 0, this._zSize / 2), .1f);
                }
            }
        }
    }
}
