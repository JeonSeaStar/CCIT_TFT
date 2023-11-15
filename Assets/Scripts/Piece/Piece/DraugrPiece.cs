using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraugrPiece : Piece
{
    protected override void Skill()
    {
        base.Skill();
        if (star == 0)
        {
            if (mana == 60)
            {
                Damage(attackDamage * 1.35f);
                mana = 0;
            }
        }
        else if(star == 1)
        {
            if (mana == 60)
            {
                Damage(attackDamage * 2f);
                mana = 0;
            }
        }
        else if (star == 2)
        {
            if (mana == 60)
            {
                Damage(attackDamage * 3f);
                mana = 0;
            }
        }
    }
}
