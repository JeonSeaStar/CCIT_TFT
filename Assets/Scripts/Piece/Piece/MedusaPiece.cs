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
            Damage(attackDamage * 1.6f);
            Instantiate(skillEffects, target.transform.position, Quaternion.identity);
        }
        else if (star == 1)
        {
            Damage(attackDamage * 2.5f);
            Instantiate(skillEffects, target.transform.position, Quaternion.identity);
        }
        else if (star == 2)
        {
            Damage(attackDamage * 2.6f);
            Instantiate(skillEffects, target.transform.position, Quaternion.identity);
        }

        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }
}
