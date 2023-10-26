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
        pathFinding = GameObject.Find("PathFinding").GetComponent<PathFinding>();
        if (isAdd)
        {
            int _count = gods.Count;
            int _randomCount = Random.Range(0, _count);
            god = Instantiate(gods[_randomCount].piecePrefab, pieceParent.transform);
            ArenaManager.Instance.fieldManagers[0].myFilePieceList.Add(god.GetComponent<Piece>());

            for (int i = 3; i > 0; i++)
            {
                for (int j = 0; j < pathFinding.grid[j].tile.Count; j++)
                {
                    if (pathFinding.grid[i].tile[j].GetComponent<Tile>().IsFull == false)
                    {
                        //기물 소환 타일 정보 저장
                        spawnTile = pathFinding.grid[i].tile[j].GetComponent<Tile>();
                        spawnTile.IsFull = true;
                        spawnTile.piece = god;

                        god.GetComponent<Piece>().currentTile = spawnTile;
                        god.GetComponent<Piece>().targetTile = spawnTile;
                        god.transform.position = new Vector3(spawnTile.transform.position.x, 0, spawnTile.transform.position.z);
                        return;
                    }
                }
            }
        }
        else if (!isAdd)
        {
            ArenaManager.Instance.fieldManagers[0].myFilePieceList.Remove(god.GetComponent<Piece>());
            Destroy(god);
            spawnTile.IsFull = false;

            spawnTile.piece = null;
            god = null;
            spawnTile = null;
        }
    }
}
