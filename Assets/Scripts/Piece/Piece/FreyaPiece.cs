using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreyaPiece : Piece
{
    public override IEnumerator Attack()
    {
        if (mana >= 80 && target != null)
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
            FreezeSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)), 1f);
        else if (star == 1)
            FreezeSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)), 1.5f);
        else if (star == 2)
            FreezeSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)), 2f);
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    void FreezeSkill(float damage, float time)
    {
        if(target != null)
        {
            Instantiate(skillEffects, target.transform.position, Quaternion.identity);
            SetDebuff("Freeze", time, target);
            Damage(damage);
        }
    }
}
