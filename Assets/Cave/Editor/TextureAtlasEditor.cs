using UnityEngine;
using System.Collections;
using UnityEditor;
using Flashunity.AtlasUVs;

[CustomEditor(typeof(BCubesUVsFromJSON))]
public class TextureAtlasEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var bCubesUVsFromJSON = (BCubesUVsFromJSON)target;

        if (bCubesUVsFromJSON.text != null)
        {
            if (GUILayout.Button("Update Dictionary"))
            {
                bCubesUVsFromJSON.UpdateDic();
            }
        }

    }

    void OnEnable()
    {
        var bCubesUVsFromJSON = (BCubesUVsFromJSON)target;

        if (bCubesUVsFromJSON.text != null && CubesUVs.cubesUVs == null || CubesUVs.cubesUVs.Length == 0)
        {
            bCubesUVsFromJSON.UpdateDic();
        }
    }

}
