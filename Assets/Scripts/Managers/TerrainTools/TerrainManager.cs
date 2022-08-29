using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VM.Managers;
using VM.Save;

namespace VM.TerrainTools
{
    public class TerrainManager : MonoBehaviour
    {
        public static TerrainManager Instance;

        [Header("Terrain Tools")]
        [SerializeField] private TerrainRedactor _redactor;

        [Header("Terrain Elements")]
        [SerializeField] private Terrain _terrain;
        [SerializeField] private Transform _terrainChangeArea;

        private Vector3 _startPosition;

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            if (Utils.MouseWorldPosition.transform == this._terrain.transform)
            {
                int radius = 2;
                this._ShowGhost(radius);

                if (this._GetGroundInfoAboutArea(Utils.MouseWorldPosition.point, 1f) == 1)
                {
                    this._ChangeGhostColor(new Color(0, 1, 0, .3f));
                }
                else
                {
                    this._ChangeGhostColor(new Color(1, 0, 0, .3f));
                }

                if (Input.GetMouseButton(0))
                {
                    this._redactor.ChangeHeights(Utils.MouseWorldPosition.point, radius, .001f);
                    this._redactor.SetDetails(Utils.MouseWorldPosition.point, radius * 2, 0, 0);
                    this._redactor.SetColor(Utils.MouseWorldPosition.point, radius + .3f, 1, 1);
                }
                else if (Input.GetMouseButton(1))
                {
                    this._redactor.ChangeHeights(Utils.MouseWorldPosition.point, radius, -.001f);
                    this._redactor.SetDetails(Utils.MouseWorldPosition.point, radius * 2, 0, 0);
                    this._redactor.SetColor(Utils.MouseWorldPosition.point, radius + .3f, 1, 0);
                }
                /*else if (Input.GetMouseButtonDown(1))
                {
                    this._startPosition = Utils.MouseWorldPosition.point;
                }
                else if (Input.GetMouseButtonUp(1))
                {
                    this.GetHeightsInfoAboutArea(this._startPosition, Utils.MouseWorldPosition.point);
                }

                if (Input.GetMouseButtonDown(1))
                {
                    this.GetGroundInfoAboutArea(Utils.MouseWorldPosition.point, 1f);
                }*/
            }
        }

        public TerrainSaveData GetTerrainSaveData ()
        {
            TerrainSaveData saveData = new TerrainSaveData();
            saveData.details = new Dictionary<int, int[,]>();

            TerrainData terrainData = this._terrain.terrainData;

            saveData.heights = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);
            saveData.colors = terrainData.GetAlphamaps(0, 0, terrainData.alphamapWidth, terrainData.alphamapHeight);

            for (int i = 0; i < terrainData.terrainLayers.Length; i++)
            {
                saveData.details.Add(i, terrainData.GetDetailLayer(0, 0, terrainData.detailWidth, terrainData.detailHeight, i));
            }

            return saveData;
        }

        public void LoadTerrainSaveData (TerrainSaveData terrainData)
        {
            TerrainData terrain = this._terrain.terrainData;

            terrain.SetHeights(0, 0, terrainData.heights);
            terrain.SetAlphamaps(0, 0, terrainData.colors);

            for (int i = 0; i < terrain.terrainLayers.Length; i++)
            {
                terrain.SetDetailLayer(0, 0, i, terrainData.details[i]);
            }
        }

        private int _GetGroundInfoAboutArea (Vector3 position, float radius)
        {
            float[,,] colors = this._redactor.GetColorsFrom(position, radius);
            int color = -1;
            bool equals = true;

            for (int i = 0; i < colors.GetLength(0); i++)
            {
                if (equals)
                {
                    for (int j = 0; j < colors.GetLength(1); j++)
                    {
                        if (equals)
                        {
                            for (int l = 0; l < colors.GetLength(2); l++)
                            {
                                if (color == -1 && colors[i, j, l] == 1)
                                {
                                    color = l;
                                }

                                if (color != -1 && (l != color || colors[i, j, l] != 1))
                                {
                                    if (colors[i, j, l] == 0) continue;

                                    color = -1;
                                    equals = false;
                                    break;
                                }
                            }
                        }
                        else break;
                    }
                } 
                else break;
            }

            return color;
        }

        // TODO:
        private void _GetHeightsInfoAboutArea (Vector3 from, Vector3 to)
        {
            float[,] heights = this._redactor.GetHeighsFrom(from, to);
            float startValue = -2;
            bool equals = true;

            for (int i = 0; i < heights.GetLength(0); i++)
            {
                if (equals)
                {
                    for (int j = 0; j < heights.GetLength(1); j++)
                    {
                        if (startValue == -2)
                        {
                            startValue = heights[i, j];
                        }
                        else
                        {
                            if (startValue != heights[i, j])
                            {
                                equals = false;
                                break;
                            }
                        }
                    }
                }
                else break;
            }

            Debug.Log("Heights: " + equals);
        }

        private void _ShowGhost (float radius)
        {
            this._terrainChangeArea.gameObject.SetActive(true);
            this._terrainChangeArea.transform.localScale = new Vector3(radius + .3f, .5f, radius + .3f);
            this._terrainChangeArea.transform.position = Utils.MouseWorldPosition.point + new Vector3(0, 0, 0);
        }

        private void _HideGhost ()
        {
            this._terrainChangeArea.gameObject.SetActive(false);
        }
            
        private void _ChangeGhostColor (Color color)
        {
            this._terrainChangeArea.GetComponent<MeshRenderer>().material.color = color;
        }
    }
}