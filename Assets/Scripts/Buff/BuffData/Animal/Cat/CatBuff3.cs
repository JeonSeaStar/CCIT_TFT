using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Buff Datas/Animals/CatBuff3")]
public class CatBuff3 : BuffData
{
    public override void DirectEffect(Piece piece, bool isAdd)
    {
        throw new System.NotImplementedException();
    }

    public override void BattleStartEffect(bool isAdd)
    {
        base.BattleStartEffect(isAdd);
    }

    public override void CoroutineEffect()
    {
        ArenaManager.Instance.fieldManagers[0].StartCoroutine(Cat());
    }

    IEnumerator Cat()
    {
        yield return new WaitForSeconds(3f);

        foreach(var cat in ArenaManager.Instance.fieldManagers[0].myFilePieceList)
        {
            if(cat.pieceData.animal == PieceData.Animal.Cat && cat.gameObject.activeSelf == true)
            {
                cat.invincible = true; 
            }
        }
    }
}

