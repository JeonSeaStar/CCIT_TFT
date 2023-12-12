using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Buff Datas/Myth/FrostyWindBuff2")]
public class FrostyWindBuff2 : BuffData
{
    List<Piece> frostyWindPiece = new List<Piece>();
    public override void BattleStartEffect(bool isAdd)
    {
        if (isAdd == false)
        {
            windCool = 3;
            ArenaManager.Instance.fieldManagers[0].buffManager.frostyWind.SetActive(false);
        }
    }
    public override void CoroutineEffect()
    {
        frostyWindPiece.Clear();
        ArenaManager.Instance.fieldManagers[0].StartCoroutine(FrostyWind());
        ArenaManager.Instance.fieldManagers[0].StartCoroutine(FrostyWindBuff());
    }
    int windCool = 3;
    IEnumerator FrostyWind()
    {
        while (true)
        {
            yield return new WaitForSeconds(windCool);

            windCool = 6;
            int _count = Random.Range(1, 4);
            List<Piece> enemyList = new List<Piece>();
            foreach (var _activePiece in ArenaManager.Instance.fieldManagers[0].enemyFilePieceList)
            {
                if (_activePiece.gameObject.activeSelf == true) enemyList.Add(_activePiece);
            }

            ArenaManager.Instance.fieldManagers[0].buffManager.frostyWind.SetActive(true);
            ArenaManager.Instance.fieldManagers[0].Invoke("DeActiveFrostyWind", 1f);
            for (int i = 0; i < _count; i++)
            {
                if (enemyList.Count == 0) break;
                else
                {
                    Piece enemy = enemyList[Random.Range(0, enemyList.Count)];
                    if (enemy.immune != true)
                    {
                        enemy.SetFreeze(3);
                        Debug.Log("서리바람이 " + enemy.gameObject.name + "을 빙결시킵니다.");
                    }
                    enemyList.Remove(enemy);
                }
            }
        }
    }
    IEnumerator FrostyWindBuff()
    {
        foreach (var _frostyWind in ArenaManager.Instance.fieldManagers[0].myFilePieceList)
        {
            if (_frostyWind.pieceData.myth == PieceData.Myth.FrostyWind) frostyWindPiece.Add(_frostyWind);
        }
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            if (frostyWindPiece.Count > 0)
            {
                for (int i = 0; i < frostyWindPiece.Count; i++)
                {
                    if (frostyWindPiece[i].health <= frostyWindPiece[i].pieceData.health[frostyWindPiece[i].star] * 0.5f)
                    {
                        frostyWindPiece[i].pieceData.CalculateBuff(frostyWindPiece[i], this);
                        frostyWindPiece.Remove(frostyWindPiece[i]);
                    }
                }
            }
        }
    }
    void DeActiveFrostyWind()
    {
        ArenaManager.Instance.fieldManagers[0].buffManager.frostyWind.SetActive(false);
    }
}
