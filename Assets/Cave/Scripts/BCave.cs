using UnityEngine;
using System.Collections;


namespace Flashunity.Cave
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshCollider))]

    public class BCave : MonoBehaviour
    {
        void Awake()
        {
            GetComponent<MeshCollider>().sharedMesh = GetComponent<MeshFilter>().mesh;
        }
    }
}
