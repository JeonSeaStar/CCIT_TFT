using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Buff Datas/Animals/CatBuff4")]
public class CatBuff4 : BuffData
{
    public override void DirectEffect(Piece piece, bool isAdd)
    {
        int _star = piece.star;
        if (isAdd == true) piece.attackDamage += piece.pieceData.attackDamage[_star] * 0.3f;
        else if (isAdd == false) piece.attackDamage -= piece.pieceData.attackDamage[_star] * 0.3f;
    }
}
