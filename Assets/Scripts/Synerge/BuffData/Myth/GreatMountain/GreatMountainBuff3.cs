using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Buff Datas/Myth/GreatMountainBuff3")]
public class GreatMountainBuff3 : BuffData
{
    public List<PieceData> gods; //One Cost

    GameObject pieceParent;
    GameObject god;

    PathFinding pathFinding;
    Tile spawnTile;
    public override void BattleStartEffect(bool isAdd)
    {
        pieceParent = GameObject.Find("Pieces");
        pathFinding = FieldManager.Instance.pathFinding;

        int _randomCount = Random.Range(0, 4);
        // 0 판도라
        // 1 제우스 벼락
        // 2 프로메텡우스
        // 3 포세이돈

        if (_randomCount == 0)
        {
            if (isAdd)
            {
                god = Instantiate(gods[0].piecePrefab, pieceParent.transform);
                god.name = "판도라의 상자";
                FieldManager.Instance.myFilePieceList.Add(god.GetComponent<Piece>());

                for (int i = 3; i < 7; i++)
                {
                    for (int j = 0; j < pathFinding.grid[j].tile.Count; j++)
                    {
                        if (pathFinding.grid[i].tile[j].IsFull == false)
                        {
                            //기물 소환 타일 정보 저장
                            spawnTile = pathFinding.grid[i].tile[j];
                            spawnTile.IsFull = true;
                            spawnTile.walkable = false;
                            spawnTile.piece = god.GetComponent<Piece>();

                            spawnTile.piece.currentTile = spawnTile;
                            spawnTile.piece.targetTile = spawnTile;
                            spawnTile.piece.isOwned = true;

                            float _height = FieldManager.Instance.groundHeight;
                            god.transform.position = new Vector3(spawnTile.transform.position.x, _height, spawnTile.transform.position.z);
                            return;
                        }
                    }
                }
            }
            else
            {
                FieldManager.Instance.myFilePieceList.Remove(god.GetComponent<Piece>());
                god.GetComponent<Piece>().currentTile.IsFull = false;
                god.GetComponent<Piece>().currentTile.walkable = true;

                Destroy(god);
                god = null;
                spawnTile = null;
            }
        }
        else if (_randomCount == 1)
        {
            if (isAdd)
            {
                int time = Random.Range(10, 51);
                IEnumerator Thunder()
                {
                    yield return new WaitForSeconds(time);
                    int ranTile = Random.Range(0, 2);
                    Tile tile = (ranTile == 0) ? pathFinding.grid[5].tile[4] : pathFinding.grid[7].tile[3];
                    if (ranTile == 0) FieldManager.Instance.buffManager.thunder1.SetActive(true);
                    else if (ranTile == 1) FieldManager.Instance.buffManager.thunder2.SetActive(true);

                    HashSet<Tile> tiles = new HashSet<Tile>();
                    tiles.Add(tile);

                    foreach(var t in pathFinding.WideGetNeighbor(tile))
                    {
                        tiles.Add(t);
                    }

                    foreach(var tt in tiles)
                    {
                        foreach(var ttt in pathFinding.GetNeighbor(tt))
                        {
                            tiles.Add(ttt);
                        }
                    }

                    foreach (var tttt in tiles)
                    {
                        if (tttt.piece != null)
                        {
                            //tttt.piece.Damage(pathFinding.GetDistance(tile, tttt));
                            if (pathFinding.GetDistance(tile, tttt) == 0) tttt.piece.Damage(500);
                            else if (pathFinding.GetDistance(tile, tttt) == 1) tttt.piece.Damage(300);
                            else if (pathFinding.GetDistance(tile, tttt) == 2) tttt.piece.Damage(150);
                            else if (pathFinding.GetDistance(tile, tttt) == 3) tttt.piece.Damage(50);
                        }
                    }
                }

                FieldManager.Instance.StartCoroutine(Thunder());
            }
        }
        else if (_randomCount == 2)
        {
            if (isAdd)
            {
                IEnumerator Fire()
                {
                    List<Tile> tiles = new List<Tile>();
                    for (int i = 4; i < pathFinding.grid.Count; i++)
                    {
                        for (int j = 0; j < pathFinding.grid[i].tile.Count; j++)
                            tiles.Add(pathFinding.grid[i].tile[j]);
                    }
                    Shuffle(tiles);

                    for (int i = 0; i < 7; i++)
                    {
                        GameObject fire = Instantiate(FieldManager.Instance.buffManager.thunder1);
                        fire.transform.position = tiles[i].transform.position;
                        fire.SetActive(true);
                        if (tiles[i].piece != null) tiles[i].piece.Damage(Random.Range(75, 301));
                    }
                    yield return new WaitForSeconds(8f);
                }
                FieldManager.Instance.StartCoroutine(Fire());
            }
        }
        else if (_randomCount == 3)
        {
            if (isAdd)
            {
                IEnumerator Water()
                {
                    //여기서 홍수 켜주고
                    for(int i = 0; i < 6; i++)
                    {
                        foreach (var piece in FieldManager.Instance.enemyFilePieceList)
                            piece.Damage(100);
                        yield return new WaitForSeconds(0.5f);
                    }
                }

                FieldManager.Instance.StartCoroutine(Water());
            }
        }
    }

    void Shuffle<T>(List<T> list)
    {
        System.Random random = new System.Random();

        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
