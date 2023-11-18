using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValkyriePiece : Piece
{
    protected override void Attack()
    {
        if (mana <= 100 && target != null)
        {
            Skill();
            mana = 0;
            Invoke("NextBehavior", attackSpeed);
        }
        else
        {
            base.Attack();
        }
    }

    protected override void Skill()
    {
        if (star == 0)
        {
            target.SkillDamage(180f);
        }
        else if (star == 1)
        {
            target.SkillDamage(270f);
        }
        else if (star == 2)
        {
            target.SkillDamage(450f);
        }
    }
}
