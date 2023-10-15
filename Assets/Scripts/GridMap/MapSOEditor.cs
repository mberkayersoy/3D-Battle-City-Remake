using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(MapSO))]
public class MapSOEditor : Editor
{

    private SerializedObject mapSO;

    SerializedProperty widthProperty;
    SerializedProperty heightProperty;
    SerializedProperty wallMapProperty;

    public  void OnEnable()
    {
        mapSO = new SerializedObject((MapSO)target);
        widthProperty = mapSO.FindProperty("width");
        heightProperty = mapSO.FindProperty("height");
        wallMapProperty = mapSO.FindProperty("wallMap");

    }

    public override void OnInspectorGUI()
    {
        if (mapSO == null) return;

        mapSO.Update();       

        int width = widthProperty.intValue;
        int height = heightProperty.intValue;

        EditorGUILayout.Space();

        EditorGUI.indentLevel = 0;

        GUIStyle tableStyle = new GUIStyle("box");
        tableStyle.padding = new RectOffset(10, 10, 10, 10);
        tableStyle.margin.left = 32;

        GUIStyle headerColumnStyle = new GUIStyle();
        headerColumnStyle.fixedWidth = 35;

        GUIStyle columnStyle = new GUIStyle();
        columnStyle.fixedWidth = 65;

        GUIStyle rowStyle = new GUIStyle();
        rowStyle.fixedHeight = 25;

        GUIStyle rowHeaderStyle = new GUIStyle();
        rowHeaderStyle.fixedWidth = columnStyle.fixedWidth - 1;

        GUIStyle columnHeaderStyle = new GUIStyle();
        columnHeaderStyle.fixedWidth = 30;
        columnHeaderStyle.fixedHeight = 25.5f;

        GUIStyle columnLabelStyle = new GUIStyle();
        columnLabelStyle.fixedWidth = rowHeaderStyle.fixedWidth - 6;
        columnLabelStyle.alignment = TextAnchor.MiddleCenter;
        columnLabelStyle.fontStyle = FontStyle.Bold;

        GUIStyle cornerLabelStyle = new GUIStyle();
        cornerLabelStyle.fixedWidth = 42;
        cornerLabelStyle.alignment = TextAnchor.MiddleRight;
        cornerLabelStyle.fontStyle = FontStyle.BoldAndItalic;
        cornerLabelStyle.fontSize = 14;
        cornerLabelStyle.padding.top = -5;

        GUIStyle rowLabelStyle = new GUIStyle();
        rowLabelStyle.fixedWidth = 25;
        rowLabelStyle.alignment = TextAnchor.MiddleRight;
        rowLabelStyle.fontStyle = FontStyle.Bold;

        GUIStyle enumStyle = new GUIStyle("popup");
        rowStyle.fixedWidth = 65;

        EditorGUILayout.BeginHorizontal(tableStyle);
        for (int x = width; x > -1; x--)
        {
            EditorGUILayout.BeginVertical((x == width) ? headerColumnStyle : columnStyle);
            for (int y = height; y > -1; y--)
            {
                if (x == width && y == height)
                {
                    EditorGUILayout.BeginVertical(rowHeaderStyle);
                    EditorGUILayout.LabelField("[X,Y]", cornerLabelStyle);
                    EditorGUILayout.EndHorizontal();
                }
                else if (x == width)
                {
                    EditorGUILayout.BeginVertical(columnHeaderStyle);
                    EditorGUILayout.LabelField(y.ToString(), rowLabelStyle);
                    EditorGUILayout.EndHorizontal();
                }
                else if (y == height)
                {
                    EditorGUILayout.BeginVertical(rowHeaderStyle);
                    EditorGUILayout.LabelField(x.ToString(), columnLabelStyle);
                    EditorGUILayout.EndHorizontal();
                }

                if (x < width && y < height)
                {
                    SerializedProperty cellPropperty = wallMapProperty.GetArrayElementAtIndex(y + x * width);
                    WallTypes type = (WallTypes)cellPropperty.intValue;
                    EditorGUILayout.BeginHorizontal(rowStyle);
                    cellPropperty.intValue = (int)(WallTypes)EditorGUILayout.EnumPopup(type, enumStyle);
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndHorizontal();

        mapSO.ApplyModifiedProperties();
    }
}


