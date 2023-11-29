using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(menuName = "Scriptable Object/Buff Datas/Animals/RabbitBuff1")]
public class RabbitBuff1 : BuffData
{
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
                if(!rabbit.isRabbitSynergeActiveCheck) rabbit.isRabbitSynergeActiveCheck = true;
            }
            yield return new WaitForSeconds(6f);
        }

    }
}
