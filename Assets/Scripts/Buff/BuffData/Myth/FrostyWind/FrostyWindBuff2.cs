using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Buff Datas/Myth/FrostyWindBuff2")]
public class FrostyWindBuff2 : BuffData
{
    List<Piece> frostyWindPiece;
    public override void CoroutineEffect()
    {
        ArenaManager.Instance.fieldManagers[0].StartCoroutine(FrostyWind());
        ArenaManager.Instance.fieldManagers[0].StartCoroutine(FrostyWindBuff());
    }

    IEnumerator FrostyWind()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);

            int _count = Random.Range(1, 4);
            List<Piece> enemyList = ArenaManager.Instance.fieldManagers[0].enemyFilePieceList;
            Piece[] enemyPiece = new Piece[_count];

            for (int i = 0; i < _count; i++)
            {
                enemyPiece[i] = enemyList[Random.Range(0, enemyList.Count)];
                if (!enemyList[i].immune) { enemyList[i].SetFreeze(); }
                enemyList.Remove(enemyPiece[i]);
            }
        }
    }
    IEnumerator FrostyWindBuff()
    {
        foreach (var _frostyWind in ArenaManager.Instance.fieldManagers[0].myFilePieceList)
        {
            if (_frostyWind.pieceData.myth == PieceData.Myth.FrostyWind)
                frostyWindPiece.Add(_frostyWind);
        }
        while (true)
        {
            yield return new WaitForSeconds(1f);
            foreach (var healthCheck in frostyWindPiece)
            {
                if (healthCheck.health <= healthCheck.pieceData.health[healthCheck.star] * 0.5f)
                {
                    healthCheck.pieceData.CalculateBuff(healthCheck, this);
                    frostyWindPiece.Remove(healthCheck);
                }
            }
            if (frostyWindPiece.Count == 0) break;
        }
    }
}
