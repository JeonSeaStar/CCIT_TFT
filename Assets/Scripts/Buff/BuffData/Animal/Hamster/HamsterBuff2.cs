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
        pathFinding = GameObject.Find("PathFinding").GetComponent<PathFinding>();
        while (true)
        {
            yield return new WaitForSeconds(4f);
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
                    _miniHamster.name = "πÃ¥œ «‹Ω∫≈Õ";
                    ArenaManager.Instance.fieldManagers[0].myFilePieceList.Add(_miniHamster.GetComponent<Piece>());
                    hamsterList.Add(_miniHamster);

                    int _randomSpot = Random.Range(0, _randomTile.Count);
                    Tile spawnTile = pathFinding.grid[0].tile[_randomSpot].GetComponent<Tile>();
                    spawnTile.IsFull = true;
                    spawnTile.walkable = false;
                    spawnTile.piece = _miniHamster.GetComponent<Piece>();

                    spawnTile.piece.isOwned = true;
                    spawnTile.piece.currentTile = spawnTile;
                    spawnTile.piece.targetTile = spawnTile;

                    float _height = ArenaManager.Instance.fieldManagers[0].groundHeight;
                    spawnTile.piece.transform.position = new Vector3(spawnTile.transform.position.x, _height, spawnTile.transform.position.z);
                    spawnTile.piece.StartNextBehavior();
                    _randomTile.RemoveAt(_randomSpot);
                }
            }
            _randomTile.Clear();
        }
    }
}
