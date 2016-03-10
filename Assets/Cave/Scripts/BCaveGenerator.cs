using UnityEngine;
using System.Collections;
using Flashunity.AtlasUVs;
using UnityEditor;
using System;

namespace Flashunity.Cave
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshCollider))]

    public class BCaveGenerator : MonoBehaviour
    {
        public const int SIZE_MIN = 3;
        public const int SIZE_MAX = 5416;

        [Tooltip("Greed columns number")]
        [Range(SIZE_MIN, SIZE_MAX)]
        public int columns = 64;

        [Tooltip("Greed rows number")]
        [Range(SIZE_MIN, SIZE_MAX)]
        public int rows = 64;

        [Tooltip("Initial random greed fill percents")]
        [Range(1, 55)]
        public int percent = 45;

        public int tileWidth = 1;
        public int tileHeight = 1;

        public string fileName = "cave_";

        public bool useFileNamePostfix = true;
        public uint fileNamePostfix = 0;

        [HideInInspector]
        public int facesCount = 0;

        Mesh mesh;
        MeshCollider meshCollider;

        int selectedTextureIndex = -1;

        public void Generate()
        {
            if (mesh == null)
                mesh = GetComponent<MeshFilter>().mesh;

            if (meshCollider == null)
                meshCollider = GetComponent<MeshCollider>(); 
            
            if (columns * rows > SIZE_MAX * SIZE_MIN)
                columns = rows = (int)Mathf.Floor(Mathf.Sqrt(SIZE_MAX * SIZE_MIN));

            MapGenerator.Generate(columns - 2, rows - 2, percent);

            UpdateMesh(MapGenerator.borderedMap);
        }


        void UpdateMesh(int[,] map)
        {
            mesh.Clear();

            facesCount = MapGenerator.GetFacesCount();

            if (facesCount > 0)
            {
                var vertices = new Vector3[facesCount * 4];
                var normals = new Vector3[facesCount * 4];
                var triangles = new int[facesCount * 6];
                var uv = new Vector2[facesCount * 4];

                int indexVertices = 0;
                int indexTriangles = 0;
                int indexUv = 0;

                for (int y = 0; y < map.GetLength(1); y++)
                    for (int x = 0; x < map.GetLength(0); x++)
                    {
                        if (map [x, y] == 1)
                        {
                            if (UnityEngine.Random.value < 0.01 || selectedTextureIndex == -1)
                                selectedTextureIndex = UnityEngine.Random.Range(0, 3);
                        
                            AddFace(selectedTextureIndex, x, y, vertices, normals, triangles, uv, ref indexVertices, ref indexTriangles, ref indexUv);
                        }
                    }

                mesh.vertices = vertices;
                mesh.normals = normals;
                mesh.triangles = triangles;
                mesh.uv = uv;

                meshCollider.sharedMesh = mesh;
            }
        }

        void AddFace(int textureIndex, int x, int y, Vector3[] vertices, Vector3[] normals, int[] triangles, Vector2[] uv, ref int indexVertices, ref int indexTriangles, ref int indexUv)
        {
            var faceVertices = new Vector3[]
            {
                new Vector3(0.0f, 0.0f, 0.0f),
                new Vector3(0.0f, tileHeight, 0.0f),
                new Vector3(tileWidth, tileHeight, 0.0f),
                new Vector3(tileWidth, 0.0f, 0.0f)
            };

            var faceNormals = new Vector3[]
            {
                Vector3.back,
                Vector3.back,
                Vector3.back,
                Vector3.back
            };

            var faceTriangles = new int[]{ 0, 1, 2, 0, 2, 3 };

            var faceUV = CubesUVs.cubesUVs [textureIndex].side;

            int indexVerticesBegin = indexVertices;

            var v = new Vector3(x * tileWidth, y * tileHeight, 0);

            for (int i = 0; i < faceVertices.Length; i++)
            {
                vertices [indexVertices] = faceVertices [i] + v;
                normals [indexVertices] = faceNormals [i];

                indexVertices++;
            }

            for (int i = 0; i < faceTriangles.Length; i++)
            {
                triangles [indexTriangles++] = indexVerticesBegin + faceTriangles [i];
            }

            for (int i = 0; i < faceUV.Length; i++)
            {
                uv [indexUv++] = faceUV [i];
            }
        }


        public void Save()
        {
            if (this.fileName.Length > 0)
            {
                string fileName = this.fileName;

                if (useFileNamePostfix)
                    fileName += fileNamePostfix.ToString();

                string meshPath = "Assets/Cave/Meshes/" + fileName + ".mesh";
                string prefabPath = "Assets/Cave/Prefabs/" + fileName + ".prefab";

                GameObject clone = Instantiate(gameObject);

                AssetDatabase.DeleteAsset(meshPath);
                AssetDatabase.DeleteAsset(prefabPath);

                DestroyImmediate(clone.GetComponent<BCaveGenerator>());
                DestroyImmediate(clone.GetComponent<BCubesUVsFromJSON>());

                AssetDatabase.CreateAsset(clone.GetComponent<MeshFilter>().mesh, meshPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                PrefabUtility.CreatePrefab(prefabPath, clone);

                DestroyImmediate(clone);

                if (useFileNamePostfix)
                {
                    if (fileNamePostfix < uint.MaxValue)
                        fileNamePostfix++;
                    else
                    {
                        fileNamePostfix = 0;
                        this.fileName = "";
                    }
                }
            }
        }
    }
}