using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(menuName = "Scriptable Object/Buff Datas/Animals/RabbitBuff1")]
public class RabbitBuff1 : BuffData
{
    public override void DirectEffect(Piece piece, bool isAdd)
    {
        throw new System.NotImplementedException();
    }

    public override void CoroutineEffect()
    {
        ArenaManager.Instance.fieldManagers[0].StartCoroutine(Rabbit());
    }

    PathFinding pathFinding;
    IEnumerator Rabbit()
    {
        pathFinding = ArenaManager.Instance.fieldManagers[0].pathFinding;

        while (true)
        {
            List<Piece> _rabbitPiece = new List<Piece>();
            foreach (var _rabbit in ArenaManager.Instance.fieldManagers[0].myFilePieceList)
            {
                if (_rabbit.pieceData.animal == PieceData.Animal.Rabbit && _rabbit.gameObject.activeSelf == true)
                    _rabbitPiece.Add(_rabbit);
            }
            foreach (var rabbit in _rabbitPiece)
            {
                rabbit.isRabbitSynergeActiveCheck = true;

                //rabbit.currentTile.IsFull = false;

                //var _enemyList = ArenaManager.Instance.fieldManagers[0].enemyFilePieceList;
                //List<Piece> _enemyActiveList = new List<Piece>();
                //foreach(var _activePiece in _enemyList)
                //{
                //    if (_activePiece.gameObject.activeSelf == true) _enemyActiveList.Add(_activePiece);
                //}

                //Piece _targetPiece = _enemyList[Random.Range(0, _enemyList.Count)];
                //var tiles = pathFinding.GetNeighbor(_targetPiece.currentTile);
                //List<Tile> _targetTiles = new List<Tile>();
                //foreach(var target in tiles)
                //{
                //    if (target.IsFull) _targetTiles.Add(target);
                //}

                //주변 타일 리스트에 넣어 놓은거중에서 랜덤으로 하나 골라서 걸루 움직여주면됌
                //IsFull도 바꿔주고 등등등
                
            }
            yield return new WaitForSeconds(3f);
        }

    }
}
