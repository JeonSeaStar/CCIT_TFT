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
        if (Input.GetKeyDown(KeyCode.F))
        {
            //내 기물 움직임 시작, 다음에 적 움직임까지 추가해서 테스트
            foreach (var piece in FieldManager.instance.myFilePieceList)
            {
                piece.NextBehavior();
            }
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
                grid[y].tile[x].listX = x;
                grid[y].tile[x].listY = y;
                grid[y].tile[x].gridX = gridX;
                grid[y].tile[x].gridY = gridY;
                grid[y].tile[x].gridZ = gridZ;
                grid[y].tile[x].walkable = true;
            }
        }
    }

    List<Tile> GetNeighbor(Tile tile)
    {
        List<Tile> neighbor = new List<Tile>();

        int x = tile.listX;
        int y = tile.listY;

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

    public int GetDistance(Tile currentNode, Tile targetNode)
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
                if (openNode[i].fCost < currentNode.fCost || openNode[i].fCost == currentNode.fCost && openNode[i].hCost < currentNode.hCost)
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
                //여기 if문에 현재 기물이 서 있는 타일 !walkable여도 이동할 수 있게
                if (!neighbor.walkable || closedNode.Contains(neighbor))
                    continue;

                int newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
                if (newMovementCostToNeighbor < neighbor.gCost || !openNode.Contains(neighbor))
                {
                    neighbor.gCost = newMovementCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.parent = currentNode;

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
            currentNode = grid[currentNode.parent.listY].tile[currentNode.parent.listX];
        }
        path.Reverse();
        candidatePath.path.Reverse();
        candidatePath.target = endNode.piece.GetComponent<Piece>();

        piece.candidatePath.Add(candidatePath);
    }

    public void SetPath(Piece piece, List<Tile> path)
    {
        piece.path = path;
    }

    public void SetCandidatePath(Piece piece, List<Piece> enemies)
    {
        piece.candidatePath = new List<CandidatePath>();
        int minCostArray = 0;

        foreach (Piece enemy in enemies)
        {
            if(!enemy.dead)
                FindPath(piece, piece.currentNode, enemy.currentNode);
        }

        for (int i = 0; i < piece.candidatePath.Count; i++)
        {
            if (piece.candidatePath[i].cost < piece.candidatePath[minCostArray].cost)
                minCostArray = i;
        }

        piece.target = piece.candidatePath[minCostArray].target;
        SetPath(piece, piece.candidatePath[minCostArray].path);
    }

    public int GetClosePiece(Piece piece)
    {
        Tile currentTile = piece.currentNode;
        List<Piece> enemyList = FieldManager.instance.enemyFilePieceList;

        int closeDistance = 99;
        int closePieceIndex = -1;

        for (int i = 0; i < enemyList.Count; i++)
        {
            int distance = GetDistance(currentTile, enemyList[i].currentNode);
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
public class CandidatePath
{
    public Piece target;
    public List<Tile> path;
    public int cost
    {
        get
        {
            int pathCost = 0;
            foreach (var tile in path)
            {
                pathCost += tile.gCost;
            }

            return pathCost;
        }
    }
}