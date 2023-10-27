using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Buff Datas/Myth/SandKingdomBuff2")]
public class SandKingdomBuff2 : BuffData
{
    public override void DirectEffect(Piece piece, bool isAdd)
    {
        if (isAdd) piece.pieceData.CalculateBuff(piece, this);
        else if (!isAdd) piece.shield = 0;
    }

    public override void BattleStartEffect(bool isAdd)
    {
        if (isAdd) Debug.Log("¸ð·¡ ¹Ù¶÷ Å´");
        else if (!isAdd) Debug.Log("¸ð·¡ ¹Ù¶÷ ²û");
    }

    public override void CoroutineEffect()
    {
        ArenaManager.Instance.fieldManagers[0].StartCoroutine(SandKingdom());
    }

    IEnumerator SandKingdom()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            foreach (var enemy in ArenaManager.Instance.fieldManagers[0].enemyFilePieceList)
            {
                if (enemy.gameObject.activeSelf == true)
                {
                    enemy.Damage(enemy, 10);
                }
            }
        }
    }
}
