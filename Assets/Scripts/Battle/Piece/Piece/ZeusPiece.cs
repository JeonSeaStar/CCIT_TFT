using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : Piece
{
    public override IEnumerator Attack()
    {
        yield return new WaitForSeconds(attackSpeed);
    }

    public override IEnumerator Skill()
    {
        yield return new WaitForSeconds(attackSpeed);
    }
}
