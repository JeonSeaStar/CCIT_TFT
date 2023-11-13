using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValkyriePiece : Piece
{
    protected override void Skill()
    {
        base.Skill();
        if(star == 0)
        {
            if (mana == 100)
            {
                Damage(180f);
                mana = 0;
            }
        }
        else if(star == 1)
        {
            if (mana == 100)
            {
                Damage(270f);
                mana = 0;
            }
        }
        else if(star == 2)
        {
            if (mana == 100)
            {
                Damage(450f);
                mana = 0;
            }
        }
    }
}
