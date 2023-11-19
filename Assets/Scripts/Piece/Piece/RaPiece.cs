using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaPiece : Piece
{
    public override IEnumerator Attack()
    {
        if (mana <= 70 && target != null)
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

    public override IEnumerator Skill() //���� ���� X
    {
        base.Skill();
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }
}
