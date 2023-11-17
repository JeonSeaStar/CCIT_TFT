using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeusPiece : Piece
{
    protected override void Attack()
    {
        if (mana <= 60)
        {
            Skill();
            mana = 0;
        }
        else
        {
            base.Attack();
        }
    }

    protected override void Skill()
    {
        base.Skill();
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
    }

    IEnumerator GetLockTarget(float damage, float time)
    {
        for (int i = 0; i < time; i++)
        {
            if (target == null)
                yield return null;
            else
                target.Damage(damage);
            yield return new WaitForSeconds(1f);
        }
    }
}
