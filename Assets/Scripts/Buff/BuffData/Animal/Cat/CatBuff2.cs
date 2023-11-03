using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Buff Datas/Animals/CatBuff2")]
public class CatBuff2 : BuffData
{
    public override void DirectEffect(Piece piece, bool isAdd)
    {
        foreach (var cat in ArenaManager.Instance.fieldManagers[0].myFilePieceList)
        {
            if (cat.pieceData.animal == PieceData.Animal.Cat) cat.isCatSynergeActiveCheck = isAdd;
        }
    }
}
