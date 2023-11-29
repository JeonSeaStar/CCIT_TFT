using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Buff Datas/Animals/FrogBuff2")]
public class FrogBuff2 : BuffData
{
    public GameObject frogRain;
    public override void DirectEffect(Piece piece, bool isAdd)
    {
        int _star = piece.star;
        if (isAdd)
        {
            piece.attackDamage += piece.pieceData.attackDamage[_star] * 0.2f;
            piece.armor += piece.pieceData.armor[_star] * 0.2f;
        }
        else if (!isAdd)
        {
            piece.attackDamage -= piece.pieceData.attackDamage[_star] * 0.2f;
            piece.armor -= piece.pieceData.armor[_star] * 0.2f;
        }
    }

    public override void BattleStartEffect(bool isAdd)
    {
        if (isAdd) frogRain.SetActive(true);
        else if (!isAdd) frogRain.SetActive(false);
    }
}
