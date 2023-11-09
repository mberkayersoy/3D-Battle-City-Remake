using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.AI.Navigation;
using UnityEngine;

[ExecuteInEditMode]
public class GridMap : MonoBehaviour
{
    public MapSO currentMap;
    Grid grid;

    public GameObject groundCube;
    public GameObject border;
    public GameObject brickWall;
    public GameObject staticWall;
    public GameObject grassWall;
    public GameObject eagle;
    public GameObject spawnAreaPlayer;
    public GameObject spawnAreaEnemy;
    public Transform mainTargetTransform;
    public Transform playerSpawnTransform;
    public List<Transform> enemySpawnTransform = new List<Transform>();
    public List<Vector3> emptyCellPositions = new List<Vector3>();

    private void Awake()
    {
        grid = GetComponent<Grid>();
    }
    void Start()
    {
        ClearMap();
        ConstructGround(15, 15);
        ConstructMap();
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
        cell.AddComponent<NavMeshManager>();

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
        for (int i = 0; i < currentMap.width; i++)
        {
            for (int j = 0; j < currentMap.height; j++)
            {
                if (currentMap.wallMap[j + i * currentMap.width] == WallTypes.Empty)
                {
                    AddEmptyCellPosition(i, j);
                }
                else if (currentMap.wallMap[j + i * currentMap.width] == WallTypes.Bricks)
                {
                    PutObjectToCell(new Vector3Int(i, 1, j), brickWall, true);
                }
                else if (currentMap.wallMap[j + i * currentMap.width] == WallTypes.Border)
                {
                    PutObjectToCell(new Vector3Int(i, 0, j), border, false);
                }
                else if (currentMap.wallMap[j + i * currentMap.width] == WallTypes.Static)
                {
                    PutObjectToCell(new Vector3Int(i, 1, j), staticWall, false);
                }
                else if (currentMap.wallMap[j + i * currentMap.width] == WallTypes.Grass)
                {
                    PutObjectToCell(new Vector3Int(i, 1, j), grassWall, false);
                }
                else if (currentMap.wallMap[j + i * currentMap.width] == WallTypes.PlayerSpawn)
                {
                    PutObjectToCell(new Vector3Int(i, 0, j), spawnAreaPlayer, false, WallTypes.PlayerSpawn);
                }
                else if (currentMap.wallMap[j + i * currentMap.width] == WallTypes.EnemySpawn)
                {
                    PutObjectToCell(new Vector3Int(i, 0, j), spawnAreaEnemy, false, WallTypes.EnemySpawn);
                }
                else if (currentMap.wallMap[j + i * currentMap.width] == WallTypes.Eagle)
                {
                    PutObjectToCell(new Vector3Int(i, 0, j), eagle, false, WallTypes.Eagle);
                }
            }
        }
    }

    public void AddEmptyCellPosition(int i, int j)
    {
        if (i <= 2 || i >= currentMap.width - 1 || j <= 2 || j >= currentMap.width) return;

        Vector3Int gridCoords = new Vector3Int(i * 2, 0, j * 2);
        Vector3 worldPosition = grid.GetCellCenterWorld(gridCoords);
        emptyCellPositions.Add(worldPosition);
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

            if (wallTypes == WallTypes.EnemySpawn)
            {
                enemySpawnTransform.Add(cell.transform.GetChild(0));
                GameManager.Instance.CurrentLevelManager.EnemySpawnPoints.Add(cell.transform.GetChild(0));
            }
            else if (wallTypes == WallTypes.PlayerSpawn)
            {
                playerSpawnTransform = cell.transform.GetChild(0);
                GameManager.Instance.CurrentLevelManager.PlayerSpawnPoints = cell.transform.GetChild(0);
            }
            else if (wallTypes == WallTypes.Eagle)
            {
                mainTargetTransform = cell.transform.GetChild(0);
                GameManager.Instance.CurrentLevelManager.EnemyMainTarget = cell.transform.GetChild(0);
            }
        }
    }


}
