using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Buff Datas/United/UnderWorldBuff1")]
public class UnderWorldBuff1 : BuffData
{
    public override void CoroutineEffect()
    {
        FieldManager.Instance.StartCoroutine(UnderWorld());
    }

    IEnumerator UnderWorld()
    {
        int _enemyCount = FieldManager.Instance.enemyFilePieceList.Count;
        Piece enemy = FieldManager.Instance.enemyFilePieceList[Random.Range(0, _enemyCount)];
        enemy.pieceData.CalculateBuff(enemy, this);

        yield return new WaitForSeconds(25f);

        if(enemy.gameObject.activeSelf == true && FieldManager.Instance.roundType == FieldManager.RoundType.Battle) enemy.Dead();
        enemy.pieceData.CalculateBuff(enemy, this, false);
    }
}