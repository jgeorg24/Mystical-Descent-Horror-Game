using System.Collections.Generic;
using UnityEngine;

public class GridVisualizer : MonoBehaviour
{
    public GameObject terrain;
    public GameObject enemy;

    private int gridSizeX, gridSizeZ;
    private float cellSize;
    public Node[,] grid;
    public List<Node> path;

    void Start()
    {
        InitializeGrid();
    }

void InitializeGrid()
{
    Vector3 terrainSize = terrain.GetComponent<Terrain>().terrainData.size;
    Vector3 enemySize = enemy.GetComponent<Collider>().bounds.size;
    cellSize = Mathf.Max(enemySize.x, enemySize.z);
    gridSizeX = Mathf.CeilToInt(terrainSize.x / cellSize);
    gridSizeZ = Mathf.CeilToInt(terrainSize.z / cellSize);
    grid = new Node[gridSizeX, gridSizeZ];

    float nodeHeightOffset = enemySize.y / 2;

    for (int x = 0; x < gridSizeX; x++)
    {
        for (int z = 0; z < gridSizeZ; z++)
        {
            Vector3 worldPoint = new Vector3(x * cellSize, nodeHeightOffset, z * cellSize) + terrain.transform.position;
            bool walkable = !Physics.CheckBox(worldPoint, new Vector3(cellSize / 2, cellSize / 2, cellSize / 2), Quaternion.identity, LayerMask.GetMask("Walls"));
            grid[x, z] = new Node(walkable, worldPoint, x, z);
        }
    }
}


    // void VisualizeGrid()
    // {
    //     for (int x = 0; x < gridSizeX; x++)
    //     {
    //         for (int z = 0; z < gridSizeZ; z++)
    //         {
    //             Vector3 cellCenter = new Vector3(x * cellSize + cellSize / 2, 0, z * cellSize + cellSize / 2) + terrain.transform.position;
    //             Debug.DrawLine(cellCenter - Vector3.right * cellSize / 2, cellCenter + Vector3.right * cellSize / 2, grid[x, z].walkable ? Color.green : Color.red, 100f);
    //             Debug.DrawLine(cellCenter - Vector3.forward * cellSize / 2, cellCenter + Vector3.forward * cellSize / 2, grid[x, z].walkable ? Color.green : Color.red, 100f);
    //         }
    //     }
    // }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x - terrain.transform.position.x) / terrain.GetComponent<Terrain>().terrainData.size.x;
        float percentZ = (worldPosition.z - terrain.transform.position.z) / terrain.GetComponent<Terrain>().terrainData.size.z;
        percentX = Mathf.Clamp01(percentX);
        percentZ = Mathf.Clamp01(percentZ);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int z = Mathf.RoundToInt((gridSizeZ - 1) * percentZ);
        return grid[x, z];
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int z = -1; z <= 1; z++)
            {
                if (x == 0 && z == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkZ = node.gridY + z;

                if (checkX >= 0 && checkX < gridSizeX && checkZ >= 0 && checkZ < gridSizeZ)
                {
                    neighbours.Add(grid[checkX, checkZ]);
                }
            }
        }

        return neighbours;
    }
}
