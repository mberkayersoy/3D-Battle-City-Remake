using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;


[CustomEditor(typeof(MapSO))]
public class MapSOEditor : Editor
{

    private SerializedObject mapSO;

    SerializedProperty widthProperty;
    SerializedProperty heightProperty;
    SerializedProperty wallMapProperty;

    public void OnEnable()
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
        for (int x = -1; x < width; x++)
        {
            EditorGUILayout.BeginVertical((x == width) ? headerColumnStyle : columnStyle);
            for (int y = height; y > -1; y--)
            {
                if (x == -1 && y == height)
                {
                    EditorGUILayout.BeginVertical(rowHeaderStyle);
                    EditorGUILayout.LabelField("[X,Y]", cornerLabelStyle);
                    EditorGUILayout.EndHorizontal();
                }
                else if (x == -1)
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

                if (x > -1 && y < height)
                {
                    SerializedProperty cellPropperty = wallMapProperty.GetArrayElementAtIndex(y + x * width);
                    WallTypes type = (WallTypes)cellPropperty.intValue;

                    switch (type)
                    {
                        case WallTypes.Bricks:
                            enumStyle.normal.background = MakeTex(2, 2, new Color(1f, 0f, 0f, 0.5f));
                            break;
                        default:
                            enumStyle.normal.background = Texture2D.grayTexture;
                            break;
                    }


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

    private Texture2D MakeTex(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; ++i)
        {
            pix[i] = col;
        }
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }
}


