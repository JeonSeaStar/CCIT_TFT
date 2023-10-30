using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        List<Piece> _rabbitPiece = new List<Piece>();
        foreach (var _rabbit in ArenaManager.Instance.fieldManagers[0].myFilePieceList)
        {
            if (_rabbit.pieceData.animal == PieceData.Animal.Rabbit) _rabbitPiece.Add(_rabbit);
        }

        while (true)
        {
            foreach(var rabbit in _rabbitPiece)
            {
                rabbit.currentTile.IsFull = false;

                var _enemyList = ArenaManager.Instance.fieldManagers[0].enemyFilePieceList;
                List<Piece> _enemyActiveList = new List<Piece>();
                foreach(var _activePiece in _enemyList)
                {
                    if (_activePiece.gameObject.activeSelf == true) _enemyActiveList.Add(_activePiece);
                }

                Piece _targetPiece = _enemyList[Random.Range(0, _enemyList.Count)];
                var tiles = pathFinding.GetNeighbor(_targetPiece.currentTile);
                List<Tile> _targetTiles = new List<Tile>();
                foreach(var target in tiles)
                {
                    if (target.IsFull) _targetTiles.Add(target);
                }

                //�ֺ� Ÿ�� ����Ʈ�� �־� �������߿��� �������� �ϳ� ��� �ɷ� �������ָ��
                //IsFull�� �ٲ��ְ� ����
                
            }
            yield return new WaitForSeconds(3f);
        }

    }
}
