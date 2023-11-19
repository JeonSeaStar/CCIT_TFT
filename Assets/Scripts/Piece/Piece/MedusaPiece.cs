using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedusaPiece : Piece
{
    protected override IEnumerator Attack()
    {
        if (mana <= 40 && target != null)
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
            target.SkillDamage(attackDamage * 1.6f);
        else if (star == 1)
            target.SkillDamage(attackDamage * 2.5f);
        else if (star == 2)
            target.SkillDamage(attackDamage * 2.6f);

        yield return new WaitForSeconds(attackSpeed);
        StartCoroutine(NextBehavior());
    }
}
