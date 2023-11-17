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
        pathFinding = ArenaManager.Instance.fieldManagers[0].pathFinding;
        if (isAdd)
        {
            int _count = gods.Count;
            int _randomCount = Random.Range(0, _count);
            god = Instantiate(gods[_randomCount].piecePrefab, pieceParent.transform);
            ArenaManager.Instance.fieldManagers[0].myFilePieceList.Add(god.GetComponent<Piece>());

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
                        god.transform.position = new Vector3(spawnTile.transform.position.x, 0, spawnTile.transform.position.z);
                        return;
                    }
                }
            }
        }
        else if (!isAdd)
        {
            ArenaManager.Instance.fieldManagers[0].myFilePieceList.Remove(god.GetComponent<Piece>());
            god.GetComponent<Piece>().currentTile.IsFull = false;
            god.GetComponent<Piece>().currentTile.walkable = true;

            Destroy(god);
            god = null;
            spawnTile = null;
        }
    }
}
