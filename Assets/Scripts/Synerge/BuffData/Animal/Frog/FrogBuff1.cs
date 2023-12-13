using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Buff Datas/Animals/FrogBuff1")]
public class FrogBuff1 : BuffData
{
    public GameObject frogRain;
    public override void DirectEffect(Piece piece, bool isAdd)
    {
        int _star = piece.star;
        if (isAdd)
        {
            piece.attackDamage += piece.pieceData.attackDamage[_star] * 0.1f;
            piece.armor += piece.pieceData.armor[_star] * 0.1f;
        }
        else if (!isAdd)
        {
            piece.attackDamage -= piece.pieceData.attackDamage[_star] * 0.1f;
            piece.armor -= piece.pieceData.armor[_star] * 0.1f;
        }
    }

    public override void BattleStartEffect(bool isAdd)
    {
        //if (isAdd == true) FieldManager.Instance.FrogRain(true);
        //else if (isAdd == false) FieldManager.Instance.FrogRain(false);
    }
}
