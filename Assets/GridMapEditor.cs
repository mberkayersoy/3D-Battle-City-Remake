using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GridMap))]
public class GridMapEditor : Editor
{

    private void OnEnable()
    {
        GridMap gridMap = (GridMap)target;

        gridMap.ClearMap();
        gridMap.ConstructGround(12, 12);
        gridMap.ConstructMap();
    }

    // Event Handling
    private void OnSceneGUI()
    {
        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Space)
        {
            Debug.Log("Space was pressed");
        }
        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            Debug.Log("The left mouse button has been clicked");
        }
    }

    // DrawGUI
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.Space();
        if (GUILayout.Button("Refresh Map"))
        {
            RefreshMap();
        }
    }

    private void RefreshMap()
    {
        GridMap gridMap = (GridMap)target;

        gridMap.ClearMap();
        gridMap.ConstructGround(12, 12);
        gridMap.ConstructMap();
    }
}
