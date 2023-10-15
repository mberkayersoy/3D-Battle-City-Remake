using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum WallTypes {
    Empty = 0,
    Bricks,
}

[CreateAssetMenu]
public class MapSO : ScriptableObject
{
    readonly public int width = 12;
    readonly public int height = 12;
    public WallTypes[,] wallMap = new WallTypes[12, 12];
}
