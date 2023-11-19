using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValkyriePiece : Piece
{
    public override IEnumerator Attack()
    {
        if (mana <= 100 && target != null)
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
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }
}
