using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorusPiece : Piece
{
    [SerializeField] private GameObject bullet;
    protected override IEnumerator Attack()
    {
        if (mana <= 70 && target != null)
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

    protected override IEnumerator Skill() //아직 구현 X
    {
        base.Skill();
        yield return new WaitForSeconds(attackSpeed);
        StartCoroutine(NextBehavior());
    }
}
