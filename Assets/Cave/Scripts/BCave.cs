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
    public class BCave : MonoBehaviour
    {
        CaveGenerator caveGenerator;

        [Range(1, 127)]
        public int width = 64;
        [Range(1, 127)]
        public int height = 64;
        [Range(1, 100)]
        public int percent = 40;

        public int tileWidth = 1;
        public int tileHeight = 1;

        public string fileName = "cave";

        [HideInInspector]
        public int facesCount = 0;

        Mesh mesh;
        MeshCollider meshCollider;

        int selectedTextureIndex = -1;

        void Awake()
        {
            if (mesh == null)
                mesh = GetComponent<MeshFilter>().mesh;

            if (mesh == null)
                mesh = GetComponent<MeshFilter>().sharedMesh;

            if (meshCollider == null)
                meshCollider = GetComponent<MeshCollider>();

            meshCollider.sharedMesh = mesh;
        }

        public void Generate()
        {
            CaveGenerator.Generate(width, height, percent);

            UpdateMesh(CaveGenerator.map);
        }


        void UpdateMesh(int[,] map)
        {
            Awake();

            mesh.Clear();

            facesCount = CaveGenerator.GetFacesCount();

            var vertices = new Vector3[facesCount * 4];
            var normals = new Vector3[facesCount * 4];
            var triangles = new int[facesCount * 6];
            var uv = new Vector2[facesCount * 4];

            int indexVertices = 0;
            int indexTriangles = 0;
            int indexUv = 0;

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
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

        protected void AddFace(int textureIndex, int x, int y, Vector3[] vertices, Vector3[] normals, int[] triangles, Vector2[] uv, ref int indexVertices, ref int indexTriangles, ref int indexUv)
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
            string meshPath = "Assets/Cave/Meshes/" + fileName + ".mesh";
            string prefabPath = "Assets/Cave/Prefabs/" + fileName + ".prefab";

          
            GameObject clone = Instantiate(gameObject);

            AssetDatabase.DeleteAsset(meshPath);
            AssetDatabase.DeleteAsset(prefabPath);

            DestroyImmediate(clone.GetComponent<BCave>());
            DestroyImmediate(clone.GetComponent<BCubesUVsFromJSON>());

            AssetDatabase.CreateAsset(clone.GetComponent<MeshFilter>().mesh, meshPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            PrefabUtility.CreatePrefab(prefabPath, clone);

            DestroyImmediate(clone);
        }
    }
}