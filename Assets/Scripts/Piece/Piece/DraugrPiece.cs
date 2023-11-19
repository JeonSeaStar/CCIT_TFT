using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraugrPiece : Piece
{
    protected override IEnumerator Attack()
    {
        if(mana >= 60 && target != null)
        {
            Skill();
            mana = 0;
            yield return new WaitForSeconds(attackSpeed);
            StartCoroutine(NextBehavior());
        }
        else
        {
            base.Attack();
        }
    }

    protected override IEnumerator Skill()
    {
        if (star == 0)
            target.SkillDamage(attackDamage * 1.35f);
        else if (star == 1)
            target.SkillDamage(attackDamage * 2f);
        else if (star == 2)
            target.SkillDamage(attackDamage * 3f);
        yield return new WaitForSeconds(attackSpeed);
        StartCoroutine(NextBehavior());
    }
}
