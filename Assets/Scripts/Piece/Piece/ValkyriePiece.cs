using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValkyriePiece : Piece
{
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
        if (star == 0)
        {
            Instantiate(skillEffects, target.transform.position, Quaternion.identity);
            Damage(180f);
        }
        else if (star == 1)
        {
            Instantiate(skillEffects, target.transform.position, Quaternion.identity);
            Damage(270f);
        }
        else if (star == 2)
        {
            Instantiate(skillEffects, target.transform.position, Quaternion.identity);
            Damage(450f);
        }
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }
}
