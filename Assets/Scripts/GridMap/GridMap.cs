using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

[ExecuteInEditMode]
public class GridMap : MonoBehaviour
{
    public GameObject groundCube;
    public GameObject brickWall;
    public GameObject staticWall;
    public GameObject grassWall;
    public GameObject spawnAreaPlayer;
    public GameObject spawnAreaEnemy;

    Grid grid;
    public MapSO map;

    private void Awake()
    {
        grid = GetComponent<Grid>();
    }
    void Start()
    {
        //ClearMap();
        //ConstructGround(12, 12);
        //ConstructMap();
    }

    public void ClearMap()
    {
        // Destory all previous childs
        var tempList = transform.Cast<Transform>().ToList();
        foreach (var child in tempList)
        {
            if (Application.isEditor)
            {
                GameObject.DestroyImmediate(child.gameObject);
            }
            else
            {
                GameObject.Destroy(child.gameObject);
            }
        }
    }

    public void ConstructGround(int width, int height)
    {
        // Create ground as one piece
        Vector3 centerPosition = Vector3.Lerp(grid.GetCellCenterWorld(new Vector3Int(0, 0, 0)), grid.GetCellCenterWorld(new Vector3Int(width * 2 -1, 0, height * 2 -1)), 0.5f);
        GameObject cell = Instantiate(groundCube, centerPosition, Quaternion.identity, gameObject.transform);
        cell.transform.localScale = new Vector3(width, 1, height);
        cell.name = "Ground";

        //for (int i = 0; i < width; i++)
        //{
        //    for (int j = 0; j < height; j++)
        //    {
        //        PutObjectToCell(new Vector3Int(i, 0, j), cube);
        //    }
        //}
    }

    public void ConstructMap()
    {
        for (int i = 0; i < map.width; i++)
        {
            for (int j = 0; j < map.height; j++)
            {

                if (map.wallMap[j + i * map.width] == WallTypes.Empty)
                {
                    //GameObject cell = Instantiate(emptyCube, position, Quaternion.identity, gameObject.transform);
                    //cell.transform.localScale = new Vector3(cellScale.x, cellScale.y, cellScale.z);
                }
                else if (map.wallMap[j + i * map.width] == WallTypes.Bricks)
                {
                    PutObjectToCell(new Vector3Int(i, 1, j), brickWall, true);
                }
                else if (map.wallMap[j + i * map.width] == WallTypes.Static)
                {
                    PutObjectToCell(new Vector3Int(i, 1, j), staticWall, false);
                }
                else if (map.wallMap[j + i * map.width] == WallTypes.Grass)
                {
                    PutObjectToCell(new Vector3Int(i, 1, j), grassWall, false);
                }
                else if (map.wallMap[j + i * map.width] == WallTypes.PlayerSpawn)
                {
                    PutObjectToCell(new Vector3Int(i, 0, j), spawnAreaPlayer, false);
                }
                else if (map.wallMap[j + i * map.width] == WallTypes.EnemySpawn)
                {
                    PutObjectToCell(new Vector3Int(i, 0, j), spawnAreaEnemy, false);
                }
            }
        }
    }

    public void PutObjectToCell(Vector3Int position, GameObject cube, bool is4Piece = false)
    {
        Vector3 cellScale = grid.cellSize;
        Vector3Int realPositionOnGrid = position * new Vector3Int(2, 1, 2);

        if (is4Piece)
        {
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    Vector3Int gridCoords = realPositionOnGrid + new Vector3Int(i, 0, j);
                    Vector3 worldPosition = grid.GetCellCenterWorld(gridCoords);
                    GameObject piece = Instantiate(cube, worldPosition, Quaternion.identity, gameObject.transform);
                    piece.transform.localScale = new Vector3(cellScale.x, cellScale.y, cellScale.z);
                    piece.name = "Cell: " + position.x + "-" + position.z + " Grid: (" + gridCoords.x + "," + gridCoords.z + ")";
                }
            }

        }
        else
        {
            Vector3 worldPosition = Vector3.Lerp(grid.GetCellCenterWorld(realPositionOnGrid), grid.GetCellCenterWorld(realPositionOnGrid + new Vector3Int(1, 0, 1)), 0.5f);
            GameObject cell = Instantiate(cube, worldPosition, Quaternion.identity, gameObject.transform);
            cell.name = "Cell: " + position.x + "-" + position.z;
        }
    }


}
