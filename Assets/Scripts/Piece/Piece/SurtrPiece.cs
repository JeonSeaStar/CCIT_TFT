using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurtrPiece : Piece
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
            Instantiate(skillEffects, this.transform.position, Quaternion.identity);
            this.shield = attackDamage * 2.5f;
        }
        else if (star == 1)
        {
            Instantiate(skillEffects, this.transform.position, Quaternion.identity);
            this.shield = attackDamage * 3.7f;
        }
        else if (star == 2)
        {
            Instantiate(skillEffects, this.transform.position, Quaternion.identity);
            this.shield = attackDamage * 5f;
        }
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }
}
