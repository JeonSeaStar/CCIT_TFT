using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Buff Datas/Animals/HamsterBuff2")]
public class HamsterBuff2 : BuffData
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
            yield return new WaitForSeconds(4f);

            var _battleResult = FieldManager.Instance.BattleResult;
            if (_battleResult != FieldManager.Result.VICTORY || _battleResult != FieldManager.Result.DEFEAT) break;

            List<Tile> _randomTile = new List<Tile>();
            for (int i = 0; i < pathFinding.grid[0].tile.Count; i++)
            {
                if (pathFinding.grid[0].tile[i].IsFull == false) _randomTile.Add(pathFinding.grid[0].tile[i]);
            }
            for (int i = 0; i < 2; i++)
            {
                if (_randomTile.Count > 0)
                {
                    GameObject _miniHamster = Instantiate(miniHamster.piecePrefab, pieceParent.transform);
                    _miniHamster.name = "�̴� �ܽ���";
                    var _miniPiece = _miniHamster.GetComponent<Piece>();
                    AugmentManager.Instance.HamsterAugmentCheck(_miniPiece);
                    FieldManager.Instance.myFilePieceList.Add(_miniPiece);
                    hamsterList.Add(_miniHamster);

                    int _randomSpot = Random.Range(0, _randomTile.Count);
                    Tile spawnTile = pathFinding.grid[0].tile[_randomSpot].GetComponent<Tile>();
                    spawnTile.IsFull = true;
                    spawnTile.walkable = false;
                    spawnTile.piece = _miniHamster.GetComponent<Piece>();

                    spawnTile.piece.isOwned = true;
                    spawnTile.piece.currentTile = spawnTile;
                    spawnTile.piece.targetTile = spawnTile;
                    TileManager.Instance.ActiveHamsterSpawnEffect(spawnTile.gameObject);

                    float _height = FieldManager.Instance.groundHeight;
                    spawnTile.piece.transform.position = new Vector3(spawnTile.transform.position.x, _height, spawnTile.transform.position.z);
                    spawnTile.piece.StartNextBehavior();
                    _randomTile.RemoveAt(_randomSpot);
                }
            }
            _randomTile.Clear();
        }
    }
}
