using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Buff Datas/Animals/FrogBuff3")]
public class FrogBuff3 : BuffData
{
    public GameObject frogRain;
    public GameObject thunder;
    public override void DirectEffect(Piece piece, bool isAdd)
    {
        int _star = piece.star;
        if (isAdd)
        {
            piece.attackDamage += piece.pieceData.attackDamage[_star] * 0.3f;
            piece.armor += piece.pieceData.armor[_star] * 0.3f;

            if (frogRain.activeSelf == false) frogRain.SetActive(true);
            if (thunder.activeSelf == false) thunder.SetActive(true);
        }
        else if (!isAdd)
        {
            piece.attackDamage -= piece.pieceData.attackDamage[_star] * 0.3f;
            piece.armor -= piece.pieceData.armor[_star] * 0.3f;

            if (frogRain.activeSelf == true) frogRain.SetActive(false);
            if (thunder.activeSelf == true) thunder.SetActive(false);
        }
    }

    public override void BattleStartEffect(bool isAdd)
    {
        //
    }
}
