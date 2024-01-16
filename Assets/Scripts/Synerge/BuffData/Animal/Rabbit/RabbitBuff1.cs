using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(menuName = "Scriptable Object/Buff Datas/Animals/RabbitBuff1")]
public class RabbitBuff1 : BuffData
{
    public override void CoroutineEffect()
    {
        FieldManager.Instance.StartCoroutine(Rabbit());
    }

    PathFinding pathFinding;
    IEnumerator Rabbit()
    {
        pathFinding = FieldManager.Instance.pathFinding;

        while (true)
        {
            foreach (var _rabbit in FieldManager.Instance.myFilePieceList)
            {
                if (_rabbit.pieceData.animal == PieceData.Animal.Rabbit && _rabbit.gameObject.activeSelf == true)
                {
                    _rabbit.isRabbitSynergeActiveCheck = true;
                }
            }
            yield return new WaitForSeconds(6f);
        }

    }
}
