using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MummyPiece : Piece
{
    protected override void Attack()
    {
        if (mana <= 60)
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
            this.shield = 130f;
        }
        else if (star == 1)
        {
            this.shield = 230f;
        }
        else if (star == 2)
        {
            this.shield = 500f;
        }
    }
}
