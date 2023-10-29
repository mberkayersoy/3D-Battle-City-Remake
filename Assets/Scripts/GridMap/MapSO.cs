using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum WallTypes {
    Empty = 0,
    Bricks,
    Static,
    Grass
}

[CreateAssetMenu(fileName= "Map", menuName = "Create Battle City Map")]
[Serializable]
public class MapSO : ScriptableObject
{
    [SerializeField]
    public int width = 12;
    [SerializeField]
    public int height = 12;
    [SerializeField]
    public WallTypes[] wallMap = new WallTypes[12 * 12];
}
