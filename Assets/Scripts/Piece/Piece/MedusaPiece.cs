using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedusaPiece : Piece
{
    protected override void Attack()
    {
        if (mana <= 40)
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
            target.Damage(attackDamage * 1.6f);
        else if (star == 1)
            target.Damage(attackDamage * 2.5f);
        else if (star == 2)
            target.Damage(attackDamage * 2.6f);
    }
}
