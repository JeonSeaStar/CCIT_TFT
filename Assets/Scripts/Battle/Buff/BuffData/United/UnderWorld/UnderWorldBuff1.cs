using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Buff Datas/United/UnderWorldBuff1")]
public class UnderWorldBuff1 : BuffData
{
    public override void CoroutineEffect()
    {
        ArenaManager.Instance.fieldManagers[0].StartCoroutine(UnderWorld());
    }

    IEnumerator UnderWorld()
    {
        int _enemyCount = ArenaManager.Instance.fieldManagers[0].enemyFilePieceList.Count;
        Piece enemy = ArenaManager.Instance.fieldManagers[0].enemyFilePieceList[Random.Range(0, _enemyCount)];
        enemy.pieceData.CalculateBuff(enemy, this);

        yield return new WaitForSeconds(25f);

        if(enemy.gameObject.activeSelf == true && ArenaManager.Instance.roundType == ArenaManager.RoundType.Battle) enemy.Dead();
        enemy.pieceData.CalculateBuff(enemy, this, false);
    }
}