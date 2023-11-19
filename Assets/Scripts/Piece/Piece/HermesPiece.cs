using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HermesPiece : Piece
{
    protected override IEnumerator Attack()
    {
        if (mana >= 75 && target != null)
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
        StartCoroutine(NextBehavior());
    }

    void BlindSkill(float time)
    {
        target.SetBlind(time);
    }
}
