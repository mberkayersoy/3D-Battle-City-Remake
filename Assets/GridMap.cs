using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[ExecuteInEditMode]
public class GridMap : MonoBehaviour
{
    public GameObject cube;
    public GameObject emptyCube;
    public GameObject brickWall;

    Grid grid;
    public MapSO map;

    // Start is called before the first frame update
    void Start()
    {
        grid = GetComponent<Grid>();

    }

    public void ClearMap()
    {
        // Destory all previous childs
        foreach (Transform child in gameObject.transform)
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
        Vector3 cellScale = grid.cellSize;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector3 position = grid.GetCellCenterWorld(new Vector3Int(i, 0, j));
                GameObject cell = Instantiate(cube, position, Quaternion.identity, gameObject.transform);
                cell.transform.localScale = new Vector3(cellScale.x, cellScale.y, cellScale.z);
            }
        }
    }

    public void ConstructMap()
    {
        Vector3 cellScale = grid.cellSize;

        for (int i = 0; i < map.width; i++)
        {
            for (int j = 0; j < map.height; j++)
            {
                Vector3 position = grid.GetCellCenterWorld(new Vector3Int(i, 1, j));

                if (map.wallMap[i, j] == WallTypes.Empty)
                {
                    GameObject cell = Instantiate(emptyCube, position, Quaternion.identity, gameObject.transform);
                    cell.transform.localScale = new Vector3(cellScale.x, cellScale.y, cellScale.z);

                }
                else if (map.wallMap[i, j] == WallTypes.Bricks)
                {
                    GameObject cell = Instantiate(brickWall, position, Quaternion.identity, gameObject.transform);
                    cell.transform.localScale = new Vector3(cellScale.x, cellScale.y, cellScale.z);
                }

            }
        }
    }
}
