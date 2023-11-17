using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnubisPiece : Piece
{
    protected override void Attack()
    {
        if (mana >= 150 && target != null)
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
            target.SkillDamage(attackDamage * 1.5f);
        else if (star == 1)
            target.SkillDamage(attackDamage * 2.5f);
        else if (star == 2)
            target.SkillDamage(attackDamage * 5f);
    }
}
