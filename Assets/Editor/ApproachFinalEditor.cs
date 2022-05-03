using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ApproachFinal))]
public class ApproachFinalEditor :  Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ApproachFinal approachFinal = (ApproachFinal)target;

        GUILayout.BeginHorizontal();

        if(GUILayout.Button("Solve Cube"))
        {
            approachFinal.Solve();
        }

        if (GUILayout.Button("Instantiate Cube"))
        {
            approachFinal.InstantiateCube();
        }

        GUILayout.EndHorizontal();


    }
}
