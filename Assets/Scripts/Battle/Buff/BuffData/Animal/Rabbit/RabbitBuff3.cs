using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Buff Datas/Animals/RabbitBuff3")]
public class RabbitBuff3 : BuffData
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
            foreach (var _rabbit in ArenaManager.Instance.fieldManagers[0].myFilePieceList)
            {
                if (_rabbit.pieceData.animal == PieceData.Animal.Rabbit && _rabbit.gameObject.activeSelf == true)
                {
                    _rabbit.isRabbitSynergeActiveCheck = true;
                }
            }
            yield return new WaitForSeconds(4f);
        }

    }
}
