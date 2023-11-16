using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphinxPiece : Piece
{
    protected override void Attack()
    {
        if (mana <= 80)
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
            SphinxSkill(1f, 0.7f);
        else if (star == 1)
            SphinxSkill(1.5f, 1.2f);
        else if (star == 2)
            SphinxSkill(2.1f, 1.8f);
    }

    void SphinxSkill(float damage, float time)
    {
        target.Damage(attackDamage * damage);
        target.SetStun(time);
    }
}
