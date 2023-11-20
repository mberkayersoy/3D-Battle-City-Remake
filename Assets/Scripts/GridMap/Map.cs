using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Map 
{
    public int width = 15;
    public int height = 15;
    [SerializeField]
    public WallTypes[] wallMap = new WallTypes[15 * 15];
}
