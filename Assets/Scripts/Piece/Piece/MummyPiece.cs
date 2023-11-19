using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MummyPiece : Piece
{
    protected override IEnumerator Attack()
    {
        if (mana <= 60)
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
            this.shield = 130f;
        }
        else if (star == 1)
        {
            this.shield = 230f;
        }
        else if (star == 2)
        {
            this.shield = 500f;
        }
        yield return new WaitForSeconds(attackSpeed);
        StartCoroutine(NextBehavior());
    }
}
