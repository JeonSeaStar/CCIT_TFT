using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPiece : Piece
{
    protected override IEnumerator Attack()
    {
        base.Attack();
        yield return new WaitForSeconds(attackSpeed);
        StartCoroutine(NextBehavior());
    }

    protected override IEnumerator Skill()
    {
        base.Skill();
        yield return new WaitForSeconds(attackSpeed);
        StartCoroutine(NextBehavior());
    }
}
