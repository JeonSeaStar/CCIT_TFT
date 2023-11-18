using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphinxPiece : Piece
{
    protected override void Attack()
    {
        if (mana <= 80 && target != null)
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
            SphinxSkill(1f, 0.7f);
        else if (star == 1)
            SphinxSkill(1.5f, 1.2f);
        else if (star == 2)
            SphinxSkill(2.1f, 1.8f);
    }

    void SphinxSkill(float damage, float time)
    {
        target.SkillDamage(attackDamage * damage);
        target.SetStun(time);
    }
}