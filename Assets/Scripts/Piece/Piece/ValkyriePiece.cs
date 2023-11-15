using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValkyriePiece : Piece
{
    protected override void Attack()
    {
        if (mana <= 100)
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
            Damage(180f);
        }
        else if (star == 1)
        {
            Damage(270f);
        }
        else if (star == 2)
        {
            Damage(450f);
        }
    }
}
