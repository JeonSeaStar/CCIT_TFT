using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedusaPiece : Piece
{
    public override IEnumerator Attack()
    {
        if (mana >= 40 && target != null)
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
        if (star == 0)
        {
            Attackkill(attackDamage * 1.6f);
        }
        else if (star == 1)
        {
            Attackkill(attackDamage * 2.5f);
        }
        else if (star == 2)
        {
            Attackkill(attackDamage * 2.6f);
        }

        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    void Attackkill(float damage)
    {
        if(target != null)
        {
            Damage(damage);
            Instantiate(skillEffects, target.transform.position, Quaternion.identity);
        }
    }
}
