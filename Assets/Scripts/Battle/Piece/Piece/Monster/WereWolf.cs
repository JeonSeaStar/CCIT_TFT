using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WereWolf : Piece
{
    //�䳢 �ó��� ó�� �ڷ� ���� �κ� �־�� ��
    public override IEnumerator Attack()
    {
        if (mana >= 100 && target != null)
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

    public override IEnumerator Skill()
    {
        Attackkill(200f);
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    void Attackkill(float damage)
    {
        if (target != null)
        {
            Instantiate(skillEffects, target.transform.position, Quaternion.identity);
            Damage(damage);
        }
    }
}
