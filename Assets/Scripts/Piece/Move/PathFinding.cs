using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    public int gridSizeX = 7;
    public int gridSizeY = 8;

    [Serializable]
    public class TileList
    {
        public List<Tile> tile = new List<Tile>();
    }
    public List<TileList> grid;
    //public List<TileList> tileList;
    //public List<List<Tile>> grid;

    void Start()
    {
        CreateGrid();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            //내 기물 움직임 시작, 다음에 적 움직임까지 추가해서 테스트
            foreach (var piece in ArenaManager.Instance.fieldManagers[0].myFilePieceList)
                piece.NextBehavior();

            foreach (var piece in ArenaManager.Instance.fieldManagers[0].enemyFilePieceList)
                piece.NextBehavior();
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

    public List<Tile> GetNeighbor(Tile tile)
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

    public int GetDistance(Tile currentTile, Tile targetTile)
    {
        int x = Mathf.Abs(currentTile.gridX - targetTile.gridX);
        int y = Mathf.Abs(currentTile.gridY - targetTile.gridY);
        int z = Mathf.Abs(currentTile.gridZ - targetTile.gridZ);

        int distance = Mathf.Max(x, y, z);

        return distance;
    }

    void FindPath(Piece piece, Tile startTile, Tile targetTile)
    {
        List<Tile> openTile = new List<Tile>();
        HashSet<Tile> closedTile = new HashSet<Tile>();
        openTile.Add(startTile);

        while (openTile.Count > 0)
        {
            Tile currentTile = openTile[0];

            for (int i = 1; i < openTile.Count; i++)
            {
                if (openTile[i].fCost < currentTile.fCost || openTile[i].fCost == currentTile.fCost && openTile[i].hCost < currentTile.hCost)
                    currentTile = openTile[i];
            }

            openTile.Remove(currentTile);
            closedTile.Add(currentTile);

            if (currentTile == targetTile)
            {
                RetracePath(piece, startTile, targetTile);
                return;
            }

            foreach (Tile neighbor in GetNeighbor(currentTile))
            {
                //여기 if문에 현재 기물이 서 있는 타일 !walkable여도 이동할 수 있게
                if (!neighbor.walkable || closedTile.Contains(neighbor))
                    continue;

                int newMovementCostToNeighbor = currentTile.gCost + GetDistance(currentTile, neighbor);
                if (newMovementCostToNeighbor < neighbor.gCost || !openTile.Contains(neighbor))
                {
                    neighbor.gCost = newMovementCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, targetTile);
                    neighbor.parent = currentTile;

                    if (!openTile.Contains(neighbor))
                    {
                        openTile.Add(neighbor);
                    }
                }
            }
        }
    }

    void RetracePath(Piece piece, Tile startTile, Tile endTile)
    {
        CandidatePath candidatePath = new CandidatePath();
        candidatePath.path = new List<Tile>();

        List<Tile> path = new List<Tile>();
        Tile currentTile = endTile;

        while (currentTile != startTile)
        {
            candidatePath.path.Add(currentTile);
            path.Add(currentTile);
            currentTile = grid[currentTile.parent.listY].tile[currentTile.parent.listX];
        }
        path.Reverse();
        candidatePath.path.Reverse();
        candidatePath.target = endTile.piece;

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
                FindPath(piece, piece.currentTile, enemy.currentTile);
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
        Tile currentTile = piece.currentTile;
        List<Piece> enemyList = ArenaManager.Instance.fieldManagers[0].enemyFilePieceList;

        int closeDistance = 99;
        int closePieceIndex = -1;

        for (int i = 0; i < enemyList.Count; i++)
        {
            int distance = GetDistance(currentTile, enemyList[i].currentTile);
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