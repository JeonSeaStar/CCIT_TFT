using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bud : Piece
{
    public override IEnumerator Attack()
    {
        if (mana >= 70 && target != null)
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
        HealSkill(200f);
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    void HealSkill(float heal)
    {
        SkillState();
        Instantiate(skillEffects, transform.position, Quaternion.identity);
        health += heal;
    }
}
