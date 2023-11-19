using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPiece : Piece
{
    public override IEnumerator Attack()
    {
        DoAttack();
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    public override IEnumerator Skill()
    {
        base.StartSkill();
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }
}
