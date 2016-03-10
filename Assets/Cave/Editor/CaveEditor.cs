using UnityEngine;
using System.Collections;
using UnityEditor;
using Flashunity.Cave;
using Flashunity.AtlasUVs;
using System;

[CustomEditor(typeof(BCaveGenerator))]
public class CaveEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var bCave = (BCaveGenerator)target;

        DrawDefaultInspector();

//        bCave.percent = EditorGUILayout.IntField("Percent", bCave.percent);

        if (CubesUVs.cubesUVs != null && CubesUVs.cubesUVs.Length > 0)
        {
            if (GUILayout.Button("Generate"))
            {
                bCave.Generate();
            }

        }

        if (bCave.facesCount > 0 && bCave.fileName.Length > 0)
        {
            if (GUILayout.Button("Save Mesh and Prefab"))
            {
                bCave.Save();
            }
        }

        //          EditorGUILayout.LabelField("Level", myTarget.Level.ToString());

    }
}

