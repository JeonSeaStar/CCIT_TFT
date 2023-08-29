using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PathFinding : MonoBehaviour
{
    public GameObject tileGameObject;
    public float tileXDistance = 0;
    public float tileYDistance = 0;
    public int gridSizeX = 7;
    public int gridSizeY = 8;

    public int X;
    public int Y;

    public List<Tile> tileList = new List<Tile>();
    public List<List<Node>> grid;

    void Start()
    {
        CreateGrid();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            List<Node> neighbor = NeighborNode(grid[Y][X]);
            Debug.Log("Node[" + Y + "][" + X + "]: (" + grid[Y][X].gridX + ", " + grid[Y][X].gridY + ", " + grid[Y][X].gridZ + ")");
            int i = 0;
            for (int j = 0; j < neighbor.Count; j++)
            {
                Debug.Log(i + ") Node[" + neighbor[i].listY + "][" + neighbor[i].listX + "]: (" + neighbor[i].gridX + ", " + neighbor[i].gridY + ", " + neighbor[i].gridZ + ")");
                i++;
            }
        }
    }

    void CreateGrid()
    {
        grid = new List<List<Node>>();
        int gridStartX = 0;
        int gridStartZ = 0;
        float positionX = 0;
        for (int y = 0; y < gridSizeY; y++)
        {
            grid.Add(new List<Node>());

            if (y != 0)
            {
                if (y % 2 == 0)
                {
                    gridStartZ--;
                    positionX = 0;
                }
                if (y % 2 != 0)
                {
                    gridStartX--;
                    positionX = tileXDistance / 2f;
                }
            }

            for (int x = 0; x < gridSizeX; x++)
            {
                int gridX = gridStartX + x;
                int gridY = y;
                int gridZ = gridStartZ - x;
                //Node node = new Node(gridX, gridY, gridZ);
                GameObject tile = Instantiate(tileGameObject, new Vector3(tileXDistance * x - positionX, 0, tileYDistance * y), Quaternion.identity);
                Node node = tile.AddComponent<Node>();
                node.gridX = gridX;
                node.gridY = gridY;
                node.gridZ = gridZ;
                node.listX = x;
                node.listY = y;
                node.gridX = gridX;
                node.gridY = gridY;
                node.gridZ = gridZ;
                Debug.Log(gridX + ", " + gridY + ", " + gridZ);
                grid[y].Add(node);

                //GameObject tile = Instantiate(tileGameObject, new Vector3(tileXDistance * x - positionX, 0, tileYDistance * y), Quaternion.identity);
                //Node nodeComponent = tile.AddComponent<Node>();
                //nodeComponent = node;
            }
        }
    }

    List<Node> NeighborNode(Node node)
    {
        List<Node> neighbor = new List<Node>();

        int x = node.listX;
        int y = node.listY;

        try
        {
            if (y % 2 == 0)
            {
                if (indexCheck(y + 1, x + 1))
                    if (grid[y + 1][x + 1] != null)
                        neighbor.Add(grid[y + 1][x + 1]);
                if (indexCheck(y, x + 1))
                    if (grid[y][x + 1] != null)
                        neighbor.Add(grid[y][x + 1]);
                if (indexCheck(y - 1, x))
                    if (grid[y - 1][x] != null)
                        neighbor.Add(grid[y - 1][x + 1]);
                if (indexCheck(y - 1, x - 1))
                    if (grid[y - 1][x - 1] != null)
                        neighbor.Add(grid[y - 1][x]);
                if (indexCheck(y, x - 1))
                    if (grid[y][x - 1] != null)
                        neighbor.Add(grid[y][x - 1]);
                if (indexCheck(y + 1, x))
                    if (grid[y + 1][x] != null)
                        neighbor.Add(grid[y + 1][x]);
            }

            if (y % 2 != 0)
            {
                if (indexCheck(y + 1, x))
                    if (grid[y + 1][x] != null)
                        neighbor.Add(grid[y + 1][x]);
                if (indexCheck(y, x + 1))
                    if (grid[y][x + 1] != null)
                        neighbor.Add(grid[y][x + 1]);
                if (indexCheck(y - 1, x))
                    if (grid[y - 1][x] != null)
                        neighbor.Add(grid[y - 1][x]);
                if (indexCheck(y - 1, x - 1))
                    if (grid[y - 1][x - 1] != null)
                        neighbor.Add(grid[y - 1][x - 1]);
                if (indexCheck(y, x - 1))
                    if (grid[y][x - 1] != null)
                        neighbor.Add(grid[y][x - 1]);
                if (indexCheck(y + 1, x - 1))
                    if (grid[y + 1][x - 1] != null)
                        neighbor.Add(grid[y + 1][x - 1]);
            }
        }
        catch (IndexOutOfRangeException e)
        {
            Debug.Log(e + ": 배열 범위를 벗어남");
        }

        return neighbor;
    }

    bool indexCheck(int x, int y)
    {
        bool index = true;

        if (x < 0 || y < 0)
        {
            index = false;
        }

        if (x == gridSizeY || y == gridSizeX)
        {
            index = false;
        }
        return index;
    }

    int ManhattanDistance(Node currentNode, Node targetNode)
    {
        int x = Mathf.Abs(currentNode.gridX - targetNode.gridX);
        int y = Mathf.Abs(currentNode.gridY - targetNode.gridY);
        int z = Mathf.Abs(currentNode.gridZ - targetNode.gridZ);

        return x + y + z;
    }
}

public class Node : MonoBehaviour
{
    public int listX;
    public int listY;

    public int gridX;
    public int gridY;
    public int gridZ;
    public Node parentNode;
    public int gCost, hCost;
    public int fCost { get { return hCost + gCost; } }
    public bool canMove;

    public Node(int gridX, int gridY, int gridZ)
    {
        this.gridX = gridX;
        this.gridY = gridY;
        this.gridZ = gridZ;
    }
}