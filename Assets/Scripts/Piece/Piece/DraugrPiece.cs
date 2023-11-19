using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraugrPiece : Piece
{
    public override IEnumerator Attack()
    {
        if (mana >= 60 && target != null)
        {
            StartSkill();
            mana = 0;
            yield return new WaitForSeconds(attackSpeed);
            StartNextBehavior();
        }
        else
        {
            DoAttack();
        }
    }

    public override IEnumerator Skill()
    {
        if (star == 0)
            target.SkillDamage(attackDamage * 1.35f);
        else if (star == 1)
            target.SkillDamage(attackDamage * 2f);
        else if (star == 2)
            target.SkillDamage(attackDamage * 3f);
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }
}
