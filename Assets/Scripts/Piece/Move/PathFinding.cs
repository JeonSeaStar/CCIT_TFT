using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
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
            List<Node> neighbor = GetNeighbor(grid[Y][X]);
            int i = 0;
            for (int j = 0; j < neighbor.Count; j++)
            {
                i++;
            }
        }
    }

    void CreateGrid()
    {
        grid = new List<List<Node>>();
        int gridStartX = 0;
        int gridStartZ = 0;
        for (int y = 0; y < gridSizeY; y++)
        {
            grid.Add(new List<Node>());

            if (y != 0)
            {
                if (y % 2 == 0)
                {
                    gridStartZ--;
                }
                if (y % 2 != 0)
                {
                    gridStartX--;
                }
            }

            for (int x = 0; x < gridSizeX; x++)
            {
                int gridX = gridStartX + x;
                int gridY = y;
                int gridZ = gridStartZ - x;
                Node node = new Node(gridX, gridY, gridZ);
                node.listX = x;
                node.listY = y;
                node.gridX = gridX;
                node.gridY = gridY;
                node.gridZ = gridZ;
                grid[y].Add(node);
            }
        }
    }

    List<Node> GetNeighbor(Node node)
    {
        List<Node> neighbor = new List<Node>();

        int x = node.listX;
        int y = node.listY;

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

    int GetDistance(Node currentNode, Node targetNode)
    {
        int x = Mathf.Abs(currentNode.gridX - targetNode.gridX);
        int y = Mathf.Abs(currentNode.gridY - targetNode.gridY);
        int z = Mathf.Abs(currentNode.gridZ - targetNode.gridZ);

        int distance = Mathf.Max(x, y, z);

        return distance;
    }

    void FindPath(Node startNode, Node targetNode)
    {
        List<Node> openNode = new List<Node>();
        HashSet<Node> closedNode = new HashSet<Node>();
        openNode.Add(startNode);

        while(openNode.Count > 0)
        {
            Node currentNode = openNode[0];
            for(int i = 1; i < openNode.Count; i++)
            {
                if(openNode[i].fCost < currentNode.fCost || openNode[i].fCost == currentNode.fCost && openNode[i].hCost < currentNode.hCost)
                    currentNode = openNode[i];
            }

            openNode.Remove(currentNode);
            closedNode.Add(currentNode);

            if (currentNode == targetNode)
            {
                RetracePath(startNode, targetNode);
                return;
            }

            foreach(Node neighbor in GetNeighbor(currentNode))
            {
                if (!neighbor.walkable || closedNode.Contains(neighbor))
                    continue;

                int newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
                if(newMovementCostToNeighbor < neighbor.gCost || !openNode.Contains(neighbor))
                {
                    neighbor.gCost = newMovementCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.parent = currentNode;

                    if (!openNode.Contains(neighbor))
                        openNode.Add(neighbor);
                }
            }
        }
    }

    void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
    }
}

[System.Serializable]
public class Node
{
    public int listX;
    public int listY;

    public int gridX;
    public int gridY;
    public int gridZ;
    public Node parent;
    public int gCost, hCost;
    public int fCost { get { return hCost + gCost; } }
    public bool walkable;

    public Node(int gridX, int gridY, int gridZ)
    {
        this.gridX = gridX;
        this.gridY = gridY;
        this.gridZ = gridZ;
    }
}