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
        pathFinding = ArenaManager.Instance.fieldManagers[0].pathFinding;
        if (isAdd)
        {
            int _count = greatMountainPieceData.Count; 
            int _randomCount = Random.Range(0, _count);
            greatMountainPiece = Instantiate(greatMountainPieceData[_randomCount].piecePrefab, pieceParent.transform);
            ArenaManager.Instance.fieldManagers[0].myFilePieceList.Add(greatMountainPiece.GetComponent<Piece>());

            if (ArenaManager.Instance.fieldManagers[0].DualPlayers[0].isExpedition == true)
            {
                for (int i = 4; i < 7; i++)
                {
                    for (int j = 0; j < pathFinding.grid[j].tile.Count; j++)
                    {
                        if (pathFinding.grid[i].tile[j].GetComponent<Tile>().IsFull == false)
                        {
                            //기물 소환 타일 정보 저장
                            spawnTile = pathFinding.grid[i].tile[j].GetComponent<Tile>();
                            spawnTile.IsFull = true;
                            spawnTile.piece = greatMountainPiece.GetComponent<Piece>();

                            greatMountainPiece.GetComponent<Piece>().currentTile = spawnTile;
                            greatMountainPiece.GetComponent<Piece>().targetTile = spawnTile;
                            greatMountainPiece.transform.position = new Vector3(spawnTile.transform.position.x, 0, spawnTile.transform.position.z);
                            return;
                        }
                    }
                }
            }
            else if(ArenaManager.Instance.fieldManagers[0].DualPlayers[0].isExpedition == false)
            {
                for (int i = 3; i > 0; i--)
                {
                    for (int j = 0; j < pathFinding.grid[j].tile.Count; j++)
                    {
                        if (pathFinding.grid[i].tile[j].GetComponent<Tile>().IsFull == false)
                        {
                            //기물 소환 타일 정보 저장
                            spawnTile = pathFinding.grid[i].tile[j].GetComponent<Tile>();
                            spawnTile.IsFull = true;
                            spawnTile.piece = greatMountainPiece.GetComponent<Piece>();

                            greatMountainPiece.GetComponent<Piece>().currentTile = spawnTile;
                            greatMountainPiece.GetComponent<Piece>().targetTile = spawnTile;
                            greatMountainPiece.transform.position = new Vector3(spawnTile.transform.position.x, 0, spawnTile.transform.position.z);
                            return;
                        }
                    }
                }
            }
        }
        else if (!isAdd)
        {
            ArenaManager.Instance.fieldManagers[0].myFilePieceList.Remove(greatMountainPiece.GetComponent<Piece>());
            Destroy(greatMountainPiece);
            spawnTile.IsFull = false;

            spawnTile.piece = null;
            greatMountainPiece = null;
            spawnTile = null;
        }
    }
}
