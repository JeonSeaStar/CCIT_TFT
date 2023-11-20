using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Buff Datas/Myth/GreatMountainBuff2")]
public class GreatMountainBuff2 : BuffData
{
    public List<PieceData> greatMountainPieceData2; //Two Cost

    GameObject pieceParent;
    GameObject greatMountainPiece;

    PathFinding pathFinding;
    Tile spawnTile;
    public override void BattleStartEffect(bool isAdd)
    {
        pieceParent = GameObject.Find("Pieces");
        pathFinding = ArenaManager.Instance.fieldManagers[0].pathFinding;
        if (isAdd)
        {
            int _count = greatMountainPieceData2.Count;
            int _randomCount = Random.Range(0, _count);
            greatMountainPiece = Instantiate(greatMountainPieceData2[_randomCount].piecePrefab, pieceParent.transform);
            greatMountainPiece.name = "그리스 시너지 생성 기물";
            ArenaManager.Instance.fieldManagers[0].myFilePieceList.Add(greatMountainPiece.GetComponent<Piece>());

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
                        spawnTile.piece = greatMountainPiece.GetComponent<Piece>();

                        spawnTile.piece.currentTile = spawnTile;
                        spawnTile.piece.targetTile = spawnTile;
                        spawnTile.piece.isOwned = true;
                        greatMountainPiece.transform.position = new Vector3(spawnTile.transform.position.x, 0, spawnTile.transform.position.z);
                        return;
                    }
                }
            }
        }
        else if (!isAdd)
        {
            ArenaManager.Instance.fieldManagers[0].myFilePieceList.Remove(greatMountainPiece.GetComponent<Piece>());
            greatMountainPiece.GetComponent<Piece>().currentTile.IsFull = false;
            greatMountainPiece.GetComponent<Piece>().currentTile.walkable = true;

            Destroy(greatMountainPiece);
            greatMountainPiece = null;
            spawnTile = null;
        }
    }
}
