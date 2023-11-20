using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnubisPiece : Piece
{
    public override IEnumerator Attack()
    {
        if (mana >= 150 && target != null)
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
            target.SkillDamage(attackDamage * 1.5f);
        }
        else if (star == 1)
        {
            Instantiate(skillEffects, target.transform.position, Quaternion.identity);
            target.SkillDamage(attackDamage * 2.5f);
        }
        else if (star == 2)
        {
            Instantiate(skillEffects, target.transform.position, Quaternion.identity);
            target.SkillDamage(attackDamage * 5f);
        }
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }
}
