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
                FieldManager.Instance.myFilePieceList.Remove(_hamster.GetComponent<Piece>());
                Destroy(_hamster);
            }
            hamsterList.Clear();
        }
    }

    public override void CoroutineEffect()
    {
        FieldManager.Instance.StartCoroutine(Hamster());
    }
    
    IEnumerator Hamster()
    {
        pieceParent = GameObject.Find("Pieces");
        pathFinding = FieldManager.Instance.pathFinding;
        while (true)
        {
            yield return new WaitForSeconds(5f);

            var _battleResult = FieldManager.Instance.BattleResult;
            if (_battleResult != FieldManager.Result.VICTORY || _battleResult != FieldManager.Result.DEFEAT) break;

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
                _miniHamster.name = "미니 햄스터";
                FieldManager.Instance.myFilePieceList.Add(_miniHamster.GetComponent<Piece>());
                hamsterList.Add(_miniHamster);

                //위치 정해주고
                int _randomSpot = Random.Range(0, _randomTile.Count);
                Tile spawnTile = pathFinding.grid[0].tile[_randomSpot];
                //spawnTile.transform.GetChild(2).gameObject.SetActive(true);
                spawnTile.IsFull = true;
                spawnTile.walkable = false;
                spawnTile.piece = _miniHamster.GetComponent<Piece>();

                //타일 정보와 미니 햄스터 위치 갱신
                spawnTile.piece.isOwned = true;
                spawnTile.piece.currentTile = spawnTile;
                spawnTile.piece.targetTile = spawnTile;
                TileManager.Instance.ActiveHamsterSpawnEffect(spawnTile.gameObject);

                float _height = FieldManager.Instance.groundHeight;
                spawnTile.piece.transform.position = new Vector3(spawnTile.transform.position.x, _height, spawnTile.transform.position.z);
                spawnTile.piece.StartNextBehavior();
            }
            _randomTile.Clear();
        }
    }
}
