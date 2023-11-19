using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HermesPiece : Piece
{
    public override IEnumerator Attack()
    {
        if (mana >= 75 && target != null)
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
            BlindSkill(1.5f);
        }
        else if (star == 1)
        {
            BlindSkill(2f);
        }
        else if (star == 2)
        {
            BlindSkill(2.5f);
        }
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    void BlindSkill(float time)
    {
        target.SetBlind(time);
    }
}
