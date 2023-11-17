using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HermesPiece : Piece
{
    protected override void Attack()
    {
        if (mana <= 75)
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
            BlindSkill(1.5f);
        }
        else if (star == 1)
        {
            BlindSkill(2f);
        }
        else if (star == 2)
        {
            BlindSkill(2.5f);
        }
    }

    void BlindSkill(float time)
    {
        target.SetBlind(time);
    }
}
