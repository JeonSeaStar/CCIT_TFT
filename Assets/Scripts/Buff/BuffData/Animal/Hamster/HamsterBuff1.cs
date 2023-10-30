using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Buff Datas/Animals/HamsterBuff1")]
public class HamsterBuff1 : BuffData
{
    public GameObject pieceParent;
    public PieceData miniHamster;
    List<Piece> hamsterList = new List<Piece>();

    public override void BattleStartEffect(bool isAdd)
    {
        //if (isAdd) null;
        if (!isAdd)
        {
            foreach (var _hamster in hamsterList) ArenaManager.Instance.fieldManagers[0].myFilePieceList.Remove(_hamster);
            foreach (var _hamster in hamsterList) Destroy(_hamster);
        }
    }

    public override void CoroutineEffect()
    {
        ArenaManager.Instance.fieldManagers[0].StartCoroutine(Hamster());
    }
    PathFinding pathFinding;
    IEnumerator Hamster()
    {
        pathFinding = GameObject.Find("PathFinding").GetComponent<PathFinding>();
        Messenger _me = ArenaManager.Instance.fieldManagers[0].DualPlayers[0];
        while (true)
        {
            yield return new WaitForSeconds(5f);
            //빈 타일 찾고
            List<Tile> _randomTile = new List<Tile>();
            int _spawnStartIndex = (_me.isExpedition) ? 7 : 0;
            for (int i = 0; i < pathFinding.grid[_spawnStartIndex].tile.Count; i++)
            {
                if (pathFinding.grid[_spawnStartIndex].tile[i].IsFull == false) _randomTile.Add(pathFinding.grid[_spawnStartIndex].tile[i]);
            }

            if (_randomTile.Count > 0)
            {
                //생성
                GameObject _miniHamster = Instantiate(miniHamster.piecePrefab, pieceParent.transform);
                ArenaManager.Instance.fieldManagers[0].myFilePieceList.Add(_miniHamster.GetComponent<Piece>());
                hamsterList.Add(_miniHamster.GetComponent<Piece>());

                //위치 정해주고
                int _randomSpot = Random.Range(0, _randomTile.Count);
                Tile spawnTile = pathFinding.grid[_spawnStartIndex].tile[_randomSpot].GetComponent<Tile>();
                spawnTile.IsFull = true;
                spawnTile.piece = _miniHamster.GetComponent<Piece>();

                //타일 정보와 미니 햄스터 위치 갱신
                _miniHamster.GetComponent<Piece>().currentTile = spawnTile;
                _miniHamster.GetComponent<Piece>().targetTile = spawnTile;
                _miniHamster.transform.position = new Vector3(spawnTile.transform.position.x, 0, spawnTile.transform.position.z);
            }
        }
    }
}
