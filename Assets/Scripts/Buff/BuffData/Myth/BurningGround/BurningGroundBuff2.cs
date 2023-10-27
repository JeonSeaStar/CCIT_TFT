using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Buff Datas/Myth/BurningGroundBuff2")]
public class BurningGroundBuff2 : BuffData
{
    public override void DirectEffect(Piece piece, bool isAdd)
    {
        if (isAdd)
        {
            percentHealth = true;
            percentAttackDamage = true;

            health = 30;
            attackDamage = 20;
        }
        else if (!isAdd)
        {
            percentHealth = false;
            percentAttackDamage = false;

            health = 0;
            attackDamage = 0;
        }
    }
}
