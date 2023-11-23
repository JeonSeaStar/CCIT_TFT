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
                piece.StartNextBehavior();

            foreach (var piece in ArenaManager.Instance.fieldManagers[0].enemyFilePieceList)
                piece.StartNextBehavior();
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

    public List<Tile> GetFrontLine(Tile tile)
    {
        List<Tile> frontLine = new List<Tile>();

        int x = tile.listX;
        int y = tile.listY;

        if (indexCheck(y, x))
            if (grid[y].tile[x] != null)
                frontLine.Add(grid[y].tile[x]);

        if (indexCheck(y, x + 1))
            if (grid[y].tile[x + 1] != null)
                frontLine.Add(grid[y].tile[x + 1]);

        if (indexCheck(y, x + 2))
            if (grid[y].tile[x + 2] != null)
                frontLine.Add(grid[y].tile[x + 2]);

        if (indexCheck(y, x + 3))
            if (grid[y].tile[x + 3] != null)
                frontLine.Add(grid[y].tile[x + 3]);

        if (indexCheck(y, x + 4))
            if (grid[y].tile[x + 4] != null)
                frontLine.Add(grid[y].tile[x + 4]);

        if (indexCheck(y, x + 5))
            if (grid[y].tile[x + 5] != null)
                frontLine.Add(grid[y].tile[x + 5]);

        if (indexCheck(y, x + 6))
            if (grid[y].tile[x + 6] != null)
                frontLine.Add(grid[y].tile[x + 6]);

        return frontLine;
    }

    public List<Tile> GetFront(Tile tile)
    {
        List<Tile> front = new List<Tile>();

        int x = tile.listX;
        int y = tile.listY;

        if (y % 2 == 0)
        {
            if (indexCheck(y + 1, x + 1))
                if (grid[y + 1].tile[x + 1] != null)
                    front.Add(grid[y + 1].tile[x + 1]);
            if (indexCheck(y + 1, x))
                if (grid[y + 1].tile[x] != null)
                    front.Add(grid[y + 1].tile[x]);
        }

        if (y % 2 != 0)
        {
            if (indexCheck(y + 1, x))
                if (grid[y + 1].tile[x] != null)
                    front.Add(grid[y + 1].tile[x]);
            if (indexCheck(y + 1, x - 1))
                if (grid[y + 1].tile[x - 1] != null)
                    front.Add(grid[y + 1].tile[x - 1]);
        }
        return front;
    }

    public List<Tile> GetSide(Tile tile)
    {
        List<Tile> side = new List<Tile>();

        int x = tile.listX;
        int y = tile.listY;

        if (indexCheck(y, x + 1))
            if (grid[y].tile[x + 1] != null)
                side.Add(grid[y].tile[x + 1]);

        if (indexCheck(y, x - 1))
            if (grid[y].tile[x - 1] != null)
                side.Add(grid[y].tile[x - 1]);


        return side;
    }

    public List<Tile> GetStrangeSide(Tile tile)
    {
        List<Tile> StrangeSide = new List<Tile>();

        int x = tile.listX;
        int y = tile.listY;

        if (indexCheck(y, x))
            if (grid[y].tile[x + 1] != null)
                StrangeSide.Add(grid[y].tile[x]);

        if (indexCheck(y, x + 1))
            if (grid[y].tile[x + 1] != null)
                StrangeSide.Add(grid[y].tile[x + 1]);

        if (indexCheck(y, x - 1))
            if (grid[y].tile[x - 1] != null)
                StrangeSide.Add(grid[y].tile[x - 1]);

        if (indexCheck(y, x - 2))
            if (grid[y].tile[x - 2] != null)
                StrangeSide.Add(grid[y].tile[x - 2]);

        return StrangeSide;
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

    void FindPath(Piece piece, Tile startTile, Tile targetTile, int distance, Piece target)
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

            //if (currentTile == targetTile)
            if (GetDistance(currentTile, targetTile) == distance)
            {
                RetracePath(piece, startTile, currentTile, target);
                //RetracePath(piece, startTile, targetTile);
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

    void RetracePath(Piece piece, Tile startTile, Tile endTile, Piece target)
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
        candidatePath.target = target;

        piece.candidatePath.Add(candidatePath);
    }

    public void SetPath(Piece piece, List<Tile> path)
    {
        piece.nextTile = path[0];
    }

    public void SetCandidatePath(Piece piece)
    {
        piece.candidatePath = new List<CandidatePath>();
        Piece enemy = piece.target;

        if (!enemy.dead)
        {
            bool canReach = false;
            List<Tile> enemyNeighborTiles = GetNeighbor(enemy.currentTile);
            foreach (Tile tile in enemyNeighborTiles)
                if (!tile.IsFull)
                    canReach = true;

            if (canReach)
                FindPath(piece, piece.currentTile, enemy.currentTile, 1, enemy);
            else
                FindPath(piece, piece.currentTile, GetTargetTile(piece.currentTile, enemy.currentTile), GetDistance(GetTargetTile(piece.currentTile, enemy.currentTile), enemy.currentTile), enemy);
        }
        if (piece.candidatePath.Count > 0)
        {
            if (piece.candidatePath[0].path.Count > 0)
            {
                SetPath(piece, piece.candidatePath[0].path);
            }
        }
        else
        {
            piece.StartNextBehavior();
        }
    }

    public Tile GetTargetTile(Tile startTile, Tile targetTile)
    {
        List<Tile> candidateList = new List<Tile>();
        List<Tile> checkedTiles = new List<Tile>();
        candidateList.Add(targetTile);

        while (true)
        {
            for (int i = 0; i < candidateList.Count; i++)
            {
                if (candidateList[i].IsFull)
                {
                    checkedTiles.Add(candidateList[i]);
                    candidateList.Remove(candidateList[i]);
                }
            }

            if (candidateList.Count != 0)
            {
                break;
            }
            else
            {
                for (int i = 0; i < checkedTiles.Count; i++)
                {
                    List<Tile> neighborTiles = GetNeighbor(checkedTiles[i]);

                    for (int j = 0; j < neighborTiles.Count; j++)
                        if (!checkedTiles.Contains(neighborTiles[j]))
                            candidateList.Add(neighborTiles[j]);
                }
            }
        }


        Tile newTargetTile = candidateList[0];
        int minDistance = GetDistance(startTile, candidateList[0]);
        for (int i = 1; i < candidateList.Count; i++)
        {
            int distance = GetDistance(startTile, candidateList[i]);
            if (minDistance > distance)
            {
                minDistance = distance;
                newTargetTile = candidateList[i];
            }
        }

        return newTargetTile;
    }

    //public void SetCandidatePath(Piece piece, List<Piece> enemies)
    //{
    //    piece.candidatePath = new List<CandidatePath>();
    //    int minCostArray = 0;

    //    foreach (Piece enemy in enemies)
    //    {
    //        if (!enemy.dead)
    //            FindPath(piece, piece.currentTile, enemy.currentTile);
    //    }

    //    for (int i = 0; i < piece.candidatePath.Count; i++)
    //    {
    //        if (piece.candidatePath[i].cost < piece.candidatePath[minCostArray].cost)
    //            minCostArray = i;
    //    }

    //    if (piece.candidatePath[minCostArray].path.Count > 0)
    //    {
    //        piece.target = piece.candidatePath[minCostArray].target;
    //        SetPath(piece, piece.candidatePath[minCostArray].path);
    //    }
    //    else
    //    {
    //        piece.IdleState();
    //    }
    //}

    public void SetTarget(Piece piece, List<Piece> enemies)
    {
        piece.candidatePath = new List<CandidatePath>();
        int minCostArray = 0;

        for (int i = 0; i < enemies.Count; i++)
        {
            Piece enemy = enemies[i];

            if (!enemy.dead)
            {
                bool canReach = false;
                List<Tile> enemyNeighborTiles = GetNeighbor(enemy.currentTile);
                foreach (Tile tile in enemyNeighborTiles)
                    if (!tile.IsFull)
                        canReach = true;

                if (canReach)
                    FindPath(piece, piece.currentTile, enemy.currentTile, 1, enemy);
                else
                    FindPath(piece, piece.currentTile, GetTargetTile(piece.currentTile, enemy.currentTile), GetDistance(GetTargetTile(piece.currentTile, enemy.currentTile), enemy.currentTile) + 1, enemy);
            }
        }

        for (int i = 0; i < piece.candidatePath.Count; i++)
        {
            if (piece.candidatePath[i].cost < piece.candidatePath[minCostArray].cost)
                minCostArray = i;
        }

        if (piece.candidatePath.Count > 0)
            piece.target = piece.candidatePath[minCostArray].target;
        else
            piece.StartNextBehavior();
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