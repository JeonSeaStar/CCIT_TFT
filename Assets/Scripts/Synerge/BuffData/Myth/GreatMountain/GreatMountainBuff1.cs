using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Buff Datas/Myth/GreatMountainBuff1")]
public class GreatMountainBuff1 : BuffData
{
    public List<PieceData> greatMountainPieceData; //One Cost
    
    GameObject pieceParent;
    GameObject greatMountainPiece;

    PathFinding pathFinding;
    Tile spawnTile;
    public override void BattleStartEffect(bool isAdd)
    {
        pieceParent = GameObject.Find("Pieces");
        pathFinding = FieldManager.Instance.pathFinding;
        if (isAdd)
        {
            int _count = greatMountainPieceData.Count; 
            int _randomCount = Random.Range(0, _count);
            greatMountainPiece = Instantiate(greatMountainPieceData[_randomCount].piecePrefab, pieceParent.transform);
            greatMountainPiece.name = "그리스 시너지 생성 기물";
            FieldManager.Instance.myFilePieceList.Add(greatMountainPiece.GetComponent<Piece>());

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

                        float _height = FieldManager.Instance.groundHeight;
                        greatMountainPiece.transform.position = new Vector3(spawnTile.transform.position.x, _height, spawnTile.transform.position.z);
                        return;
                    }
                }
            }
        }
        else if (!isAdd)
        {
            FieldManager.Instance.myFilePieceList.Remove(greatMountainPiece.GetComponent<Piece>());
            greatMountainPiece.GetComponent<Piece>().currentTile.IsFull = false;
            greatMountainPiece.GetComponent<Piece>().currentTile.walkable = true;

            Destroy(greatMountainPiece);
            greatMountainPiece = null;
            spawnTile = null;
        }
    }
}
