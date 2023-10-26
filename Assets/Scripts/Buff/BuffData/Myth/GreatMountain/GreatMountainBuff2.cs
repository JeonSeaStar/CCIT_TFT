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
        pathFinding = GameObject.Find("PathFinding").GetComponent<PathFinding>();
        if (isAdd)
        {
            int _count = greatMountainPieceData2.Count;
            int _randomCount = Random.Range(0, _count);
            greatMountainPiece = Instantiate(greatMountainPieceData2[_randomCount].piecePrefab, pieceParent.transform);
            ArenaManager.Instance.fieldManagers[0].myFilePieceList.Add(greatMountainPiece.GetComponent<Piece>());

            for (int i = 3; i > 0; i++)
            {
                for (int j = 0; j < pathFinding.grid[j].tile.Count; j++)
                {
                    if (pathFinding.grid[i].tile[j].GetComponent<Tile>().IsFull == false)
                    {
                        //�⹰ ��ȯ Ÿ�� ���� ����
                        spawnTile = pathFinding.grid[i].tile[j].GetComponent<Tile>();
                        spawnTile.IsFull = true;
                        spawnTile.piece = greatMountainPiece;

                        greatMountainPiece.GetComponent<Piece>().currentTile = spawnTile;
                        greatMountainPiece.GetComponent<Piece>().targetTile = spawnTile;
                        greatMountainPiece.transform.position = new Vector3(spawnTile.transform.position.x, 0, spawnTile.transform.position.z);
                        return;
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
