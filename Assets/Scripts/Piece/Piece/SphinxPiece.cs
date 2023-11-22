using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphinxPiece : Piece
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
            SphinxSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)), 0.7f);
        else if (star == 1)
            SphinxSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)), 1.2f);
        else if (star == 2)
            SphinxSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)), 1.8f);
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    void SphinxSkill(float damage, float time)
    {
        if(target != null)
        {
            Instantiate(skillEffects, target.transform.position, Quaternion.identity);
            Damage(damage);
            target.SetStun(time);
            SetDebuff("Stun", time);
        }
    }
}
