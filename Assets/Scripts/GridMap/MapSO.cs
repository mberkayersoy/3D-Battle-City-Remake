using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName= "Map", menuName = "Create Battle City Map")]
[Serializable]
public class MapSO : ScriptableObject
{
    [SerializeField]
    public int width = 15;
    [SerializeField]
    public int height = 15;
    [SerializeField]
    public WallTypes[] wallMap = new WallTypes[15 * 15];
}
