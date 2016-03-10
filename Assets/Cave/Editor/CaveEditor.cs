using UnityEngine;
using System.Collections;
using UnityEditor;
using Flashunity.Cave;
using Flashunity.AtlasUVs;

[CustomEditor(typeof(BCave))]
public class CaveEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var bCave = (BCave)target;

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
            if (GUILayout.Button("Save"))
            {
                bCave.Save();
            }
        }


        //          EditorGUILayout.LabelField("Level", myTarget.Level.ToString());

    }
}

