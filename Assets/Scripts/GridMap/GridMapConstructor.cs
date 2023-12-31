using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMapConstructor : MonoBehaviour
{
    public Grid grid;
    //public Map consturctedMap;
    public WallTypes[] wallMap = new WallTypes[width * height];
    private const int width = 15;
    private const int height = 15;
    public GameObject groundCube;
    public GameObject border;
    public GameObject brickWall;
    public GameObject staticWall;
    public GameObject grassWall;
    public GameObject eagle;
    public GameObject spawnAreaPlayer;
    public GameObject spawnAreaEnemy;
    private void Awake()
    {
        grid = GetComponent<Grid>();
    }
    void Start()
    {
        ConstructGround(width, height);
        ConstructMap();
    }

    public void ConstructGround(int width, int height)
    {
        // Create ground as one piece
        Vector3 centerPosition = Vector3.Lerp(grid.GetCellCenterWorld(new Vector3Int(0, 0, 0)), grid.GetCellCenterWorld(new Vector3Int(width * 2 - 1, 0, height * 2 - 1)), 0.5f);
        GameObject cell = Instantiate(groundCube, centerPosition, Quaternion.identity, gameObject.transform);
        cell.transform.localScale = new Vector3(width, 1, height);
    }

    public void ConstructMap()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (wallMap[j + i * width] == WallTypes.Empty)
                {

                }
                else if (wallMap[j + i * width] == WallTypes.Bricks)
                {
                    PutObjectToCell(new Vector3Int(i, 1, j), brickWall, false);
                }
                else if (wallMap[j + i * width] == WallTypes.Border)
                {
                    PutObjectToCell(new Vector3Int(i, 1, j), border, false);
                }
                else if (wallMap[j + i * width] == WallTypes.Static)
                {
                    PutObjectToCell(new Vector3Int(i, 1, j), staticWall, false);
                }
                else if (wallMap[j + i * width] == WallTypes.Grass)
                {
                    PutObjectToCell(new Vector3Int(i, 1, j), grassWall, false);
                }
                else if (wallMap[j + i * width] == WallTypes.PlayerSpawn)
                {
                    PutObjectToCell(new Vector3Int(i, 1, j), spawnAreaPlayer, false, WallTypes.PlayerSpawn);
                }
                else if (wallMap[j + i * width] == WallTypes.EnemySpawn)
                {
                    PutObjectToCell(new Vector3Int(i, 1, j), spawnAreaEnemy, false, WallTypes.EnemySpawn);
                }
                else if (wallMap[j + i * width] == WallTypes.Eagle)
                {
                    PutObjectToCell(new Vector3Int(i, 1, j), eagle, false, WallTypes.Eagle);
                }
            }
        }
    }

    public void PutObjectToCell(Vector3Int position, GameObject cube, bool is4Piece = false, WallTypes wallTypes = WallTypes.Empty)
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

    public void TryPutObjectToSelectedCellPosition(Vector3Int position, WallTypes wallType, bool is4Piece = false)
    {
        if (IsIndexEmpty(position))
        {
            SetIndex(position.x, position.z, width, wallType);
        }
        else
        {
            return;
        }
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
                    GameObject piece = Instantiate(GetWallObject(wallType), worldPosition, Quaternion.identity, gameObject.transform);
                    piece.transform.localScale = new Vector3(cellScale.x, cellScale.y, cellScale.z);
                    piece.name = "Cell: " + position.x + "-" + position.z + " Grid: (" + gridCoords.x + "," + gridCoords.z + ")";

                }
            }
        }
        else
        {
            Vector3 worldPosition = Vector3.Lerp(grid.GetCellCenterWorld(realPositionOnGrid), grid.GetCellCenterWorld(realPositionOnGrid + new Vector3Int(1, 0, 1)), 0.5f);
            GameObject cell = Instantiate(GetWallObject(wallType), worldPosition, Quaternion.identity, gameObject.transform);
            cell.name = "Cell: " + position.x + "-" + position.z;

        }
    }

    public void RemoveObjectFromCell(Vector3Int position, GameObject wall ,bool is4Piece = false)
    {
        if (wallMap[GetIndex(position.x, position.z, width)] != WallTypes.Border)
        {
            wallMap[GetIndex(position.x, position.z, width)] = WallTypes.Empty;
            Destroy(wall);
        }

       
    }

    private int GetIndex(int x, int y, int width)
    {
        return y + x * width;
    }
    private void SetIndex(int x, int y, int width, WallTypes wallType)
    {
        wallMap[GetIndex(x, y, width)] = wallType;
    }
    private bool IsIndexEmpty(Vector3Int position)
    {
       if (wallMap[GetIndex(position.x, position.z, width)] == WallTypes.Empty)
       {
            return true;
       }

        return false;
    }

    public GameObject GetWallObject(WallTypes wallType)
    {
        switch (wallType)
        {
            default:
            case WallTypes.Bricks:
                return brickWall;
            case WallTypes.Static:
                return staticWall;
            case WallTypes.Grass:
                return grassWall;
            case WallTypes.PlayerSpawn:
                return spawnAreaPlayer;
            case WallTypes.EnemySpawn:
                return spawnAreaEnemy;
            case WallTypes.Eagle:
                return eagle;
        }
    }
}
