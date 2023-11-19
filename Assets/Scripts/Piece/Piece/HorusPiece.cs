using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorusPiece : Piece
{
    [SerializeField] private GameObject bullet;
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

    public override IEnumerator Skill() //아직 구현 X
    {
        StartSkill();
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }
}
