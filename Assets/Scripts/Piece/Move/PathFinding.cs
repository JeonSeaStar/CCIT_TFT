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

    [Serializable]
    public class NodeList
    {
        public List<Tile> tile = new List<Tile>();
    }
    public List<NodeList> grid;
    //public List<NodeList> tileList;
    //public List<List<Node>> grid;

    //테스트용
    public Piece startPiece;
    public Piece targetPiece;

    void Start()
    {
        CreateGrid();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            List<Tile> neighbor = GetNeighbor(grid[Y].tile[X]);
            int i = 0;
            for (int j = 0; j < neighbor.Count; j++)
            {
                i++;
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            startPiece.currentNode.node.walkable = true;
            foreach(var enemy in FieldManager.instance.enemyFilePieceList)
                enemy.currentNode.node.walkable = true;
            SetCandidatePath(startPiece, FieldManager.instance.enemyFilePieceList);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            startPiece.Move();
        }
    }

    void CreateGrid()
    {
        int gridStartX = 0;
        int gridStartZ = 0;
        for (int y = 0; y < gridSizeY; y++)
        {
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

                grid[y].tile[x].node = node;
            }
        }
    }

    List<Tile> GetNeighbor(Tile tile)
    {
        List<Tile> neighbor = new List<Tile>();

        int x = tile.node.listX;
        int y = tile.node.listY;

        if (y % 2 == 0)
        {
            if (indexCheck(y + 1, x + 1))
                if (grid[y + 1].tile[x + 1] != null)
                    neighbor.Add(grid[y + 1].tile[x + 1]);
            if (indexCheck(y, x + 1))
                if (grid[y].tile[x + 1] != null)
                    neighbor.Add(grid[y].tile[x + 1]);
            if (indexCheck(y - 1, x + 1))
                if (grid[y - 1].tile[x + 1] != null)
                    neighbor.Add(grid[y - 1].tile[x + 1]);
            if (indexCheck(y - 1, x))
                if (grid[y - 1].tile[x] != null)
                    neighbor.Add(grid[y - 1].tile[x]);
            if (indexCheck(y, x - 1))
                if (grid[y].tile[x - 1] != null)
                    neighbor.Add(grid[y].tile[x - 1]);
            if (indexCheck(y + 1, x))
                if (grid[y + 1].tile[x] != null)
                    neighbor.Add(grid[y + 1].tile[x]);
        }

        if (y % 2 != 0)
        {
            if (indexCheck(y + 1, x))
                if (grid[y + 1].tile[x] != null)
                    neighbor.Add(grid[y + 1].tile[x]);
            if (indexCheck(y, x + 1))
                if (grid[y].tile[x + 1] != null)
                    neighbor.Add(grid[y].tile[x + 1]);
            if (indexCheck(y - 1, x))
                if (grid[y - 1].tile[x] != null)
                    neighbor.Add(grid[y - 1].tile[x]);
            if (indexCheck(y - 1, x - 1))
                if (grid[y - 1].tile[x - 1] != null)
                    neighbor.Add(grid[y - 1].tile[x - 1]);
            if (indexCheck(y, x - 1))
                if (grid[y].tile[x - 1] != null)
                    neighbor.Add(grid[y].tile[x - 1]);
            if (indexCheck(y + 1, x - 1))
                if (grid[y + 1].tile[x - 1] != null)
                    neighbor.Add(grid[y + 1].tile[x - 1]);
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

    public int GetDistance(Node currentNode, Node targetNode)
    {
        int x = Mathf.Abs(currentNode.gridX - targetNode.gridX);
        int y = Mathf.Abs(currentNode.gridY - targetNode.gridY);
        int z = Mathf.Abs(currentNode.gridZ - targetNode.gridZ);

        int distance = Mathf.Max(x, y, z);

        return distance;
    }

    void FindPath(Piece piece, Tile startNode, Tile targetNode)
    {
        List<Tile> openNode = new List<Tile>();
        HashSet<Tile> closedNode = new HashSet<Tile>();
        openNode.Add(startNode);

        while (openNode.Count > 0)
        {
            Tile currentNode = openNode[0];

            for (int i = 1; i < openNode.Count; i++)
            {
                if (openNode[i].node.fCost < currentNode.node.fCost || openNode[i].node.fCost == currentNode.node.fCost && openNode[i].node.hCost < currentNode.node.hCost)
                    currentNode = openNode[i];
            }

            openNode.Remove(currentNode);
            closedNode.Add(currentNode);

            if (currentNode == targetNode)
            {
                RetracePath(piece, startNode, targetNode);
                return;
            }

            foreach (Tile neighbor in GetNeighbor(currentNode))
            {
                if (!neighbor.node.walkable || closedNode.Contains(neighbor))
                    continue;

                int newMovementCostToNeighbor = currentNode.node.gCost + GetDistance(currentNode.node, neighbor.node);
                if (newMovementCostToNeighbor < neighbor.node.gCost || !openNode.Contains(neighbor))
                {
                    neighbor.node.gCost = newMovementCostToNeighbor;
                    neighbor.node.hCost = GetDistance(neighbor.node, targetNode.node);
                    neighbor.node.parent = currentNode.node;

                    if (!openNode.Contains(neighbor))
                    {
                        openNode.Add(neighbor);
                    }
                }
            }
        }
    }

    void RetracePath(Piece piece, Tile startNode, Tile endNode)
    {
        CandidatePath candidatePath = new CandidatePath();
        candidatePath.path = new List<Tile>();

        List<Tile> path = new List<Tile>();
        Tile currentNode = endNode;

        while (currentNode != startNode)
        {
            candidatePath.path.Add(currentNode);
            path.Add(currentNode);
            currentNode = grid[currentNode.node.parent.listY].tile[currentNode.node.parent.listX];
        }
        path.Reverse();
        candidatePath.path.Reverse();

        piece.candidatePath.Add(candidatePath);
    }

    public void SetPath(Piece piece, List<Tile> path)
    {
        piece.path = path;
    }

    public void SetCandidatePath(Piece piece, List<Piece> Enemies)
    {
        int minCostArray = 0;

        foreach (Piece enemy in Enemies)
        {
            FindPath(piece, piece.currentNode, enemy.currentNode);
        }

        for(int i = 0; i < piece.candidatePath.Count; i++)
        {
            if (piece.candidatePath[i].cost < piece.candidatePath[minCostArray].cost)
            {
                minCostArray = i;
                piece.target = Enemies[i];
            }
        }

        SetPath(piece, piece.candidatePath[minCostArray].path);
    }

    public int GetClosePiece(Piece piece)
    {
        Tile currentTile = piece.currentNode;
        List<Piece> enemyList = FieldManager.instance.enemyFilePieceList;

        int closeDistance = 99;
        int closePieceIndex = -1;

        for(int i = 0; i < enemyList.Count; i++)
        {
            int distance = GetDistance(currentTile.node, enemyList[i].currentNode.node);
            if (closeDistance > distance)
            {
                closeDistance = distance;
                closePieceIndex = i;
            }
        }

        return closePieceIndex;
    }
}

[Serializable]
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
    public bool walkable = true;

    public Node(int gridX, int gridY, int gridZ)
    {
        this.gridX = gridX;
        this.gridY = gridY;
        this.gridZ = gridZ;
    }
}

[Serializable]
public class CandidatePath
{
    public List<Tile> path;
    public int cost
    {
        get
        {
            int pathCost = 0;
            foreach (var tile in path)
            {
                pathCost += tile.node.gCost;
            }

            return pathCost;
        }
    }
}