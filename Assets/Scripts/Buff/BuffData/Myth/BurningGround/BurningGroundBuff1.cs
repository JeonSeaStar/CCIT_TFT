using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Buff Datas/Myth/BurningGroundBuff1")]
public class BurningGroundBuff1 : BuffData
{
    public override void DirectEffect(Piece piece, bool isAdd)
    {
        if (isAdd)
        {
            percentHealth = true;
            percentAttackDamage = true;

            health = 20;
            attackDamage = 15;
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
