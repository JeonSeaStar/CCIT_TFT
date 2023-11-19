using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurtrPiece : Piece
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
            this.shield = attackDamage * 2.5f;
        }
        else if (star == 1)
        {
            this.shield = attackDamage * 3.7f;
        }
        else if (star == 2)
        {
            this.shield = attackDamage * 5f;
        }
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }
}
