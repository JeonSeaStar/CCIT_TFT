using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurtrPiece : Piece
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
            this.shield = attackDamage * 2.5f;
        }
        else if (star == 1)
        {
            this.shield = attackDamage * 3.7f;
        }
        else if (star == 2)
        {
            this.shield = attackDamage * 5f;
        }
    }
}
