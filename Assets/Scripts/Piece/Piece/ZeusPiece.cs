using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeusPiece : Piece
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
            StartCoroutine(GetLockTarget(attackDamage * 7.35f, 3f));
        }
        else if (star == 1)
        {
            StartCoroutine(GetLockTarget(attackDamage * 11f, 6f));
        }
        else if (star == 2)
        {
            StartCoroutine(GetLockTarget(attackDamage * 27.5f, 9f));
        }
        yield return new WaitForSeconds(attackSpeed);
        StartCoroutine(NextBehavior());
    }

    IEnumerator GetLockTarget(float damage, float time)
    {
        for (int i = 0; i < time; i++)
        {
            if (target == null)
                yield break; //or yield return null;
            else
                target.SkillDamage(damage);
            yield return new WaitForSeconds(1f);
        }
    }
}
