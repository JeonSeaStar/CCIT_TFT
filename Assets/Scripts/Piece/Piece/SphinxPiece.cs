using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphinxPiece : Piece
{
    public override IEnumerator Attack()
    {
        if (mana <= 80 && target != null)
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
            SphinxSkill(1f, 0.7f);
        else if (star == 1)
            SphinxSkill(1.5f, 1.2f);
        else if (star == 2)
            SphinxSkill(2.1f, 1.8f);
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    void SphinxSkill(float damage, float time)
    {
        target.SkillDamage(attackDamage * damage);
        target.SetStun(time);
    }
}
