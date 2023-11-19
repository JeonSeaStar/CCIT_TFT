using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinotaurosPiece : Piece
{
    protected override IEnumerator Attack()
    {
        if (mana <= 100)
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
            this.shield = 150f;
        else if (star == 1)
            this.shield = 250f;
        else if (star == 2)
            this.shield = 350f;
        yield return new WaitForSeconds(attackSpeed);
        StartCoroutine(NextBehavior());
    }
}
