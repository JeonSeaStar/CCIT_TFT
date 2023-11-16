using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraugrPiece : Piece
{
    protected override void Attack()
    {
        if(mana <= 60)
        {
            Skill();
            mana = 0;
        }
        else
        {
            base.Attack();
        }
    }

    protected override void Skill()
    {
        base.Skill();
        if (star == 0)
        {
            Damage(attackDamage * 1.35f);
        }
        else if (star == 1)
        {
            Damage(attackDamage * 2f);
        }
        else if (star == 2)
        {
            Damage(attackDamage * 3f);
        }
    }
}
