using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Buff Datas/Animals/HamsterBuff1")]
public class HamsterBuff1 : BuffData
{
    public GameObject pieceParent;
    public PieceData miniHamster;
    [SerializeField] List<GameObject> hamsterList = new List<GameObject>();
    [SerializeField] PathFinding pathFinding;
    public override void BattleStartEffect(bool isAdd)
    {
        if (isAdd) hamsterList.Clear();
        else if (!isAdd)
        {
            foreach (var _hamster in hamsterList)
            {
                ArenaManager.Instance.fieldManagers[0].myFilePieceList.Remove(_hamster.GetComponent<Piece>());
                Destroy(_hamster);
            }
            hamsterList.Clear();
        }
    }

    public override void CoroutineEffect()
    {
        ArenaManager.Instance.fieldManagers[0].StartCoroutine(Hamster());
    }
    
    IEnumerator Hamster()
    {
        pieceParent = GameObject.Find("Pieces");
        pathFinding = ArenaManager.Instance.fieldManagers[0].pathFinding;
        while (true)
        {
            yield return new WaitForSeconds(5f);
            //빈 타일 찾고
            List<Tile> _randomTile = new List<Tile>();
            for (int i = 0; i < pathFinding.grid[0].tile.Count; i++)
            {
                if (pathFinding.grid[0].tile[i].IsFull == false) _randomTile.Add(pathFinding.grid[0].tile[i]);
            }

            if (_randomTile.Count > 0)
            {
                //생성
                GameObject _miniHamster = Instantiate(miniHamster.piecePrefab, pieceParent.transform);
                ArenaManager.Instance.fieldManagers[0].myFilePieceList.Add(_miniHamster.GetComponent<Piece>());
                hamsterList.Add(_miniHamster);

                //위치 정해주고
                int _randomSpot = Random.Range(0, _randomTile.Count);
                Tile spawnTile = pathFinding.grid[0].tile[_randomSpot];
                spawnTile.IsFull = true;
                spawnTile.walkable = false;
                spawnTile.piece = _miniHamster.GetComponent<Piece>();

                //타일 정보와 미니 햄스터 위치 갱신
                spawnTile.piece.isOwned = true;
                spawnTile.piece.currentTile = spawnTile;
                spawnTile.piece.targetTile = spawnTile;
                spawnTile.piece.transform.position = new Vector3(spawnTile.transform.position.x, 0, spawnTile.transform.position.z);
                spawnTile.piece.NextBehavior();
            }
            _randomTile.Clear();
        }
    }
}
