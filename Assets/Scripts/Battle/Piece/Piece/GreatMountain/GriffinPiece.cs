using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GriffinPiece : Piece
{
    public TriggerCheckSkill griffinSkill;
    public override IEnumerator Attack()
    {
        if (mana >= maxMana)
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
        GetLocationMultiRangeSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)));
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    void GetLocationMultiRangeSkill(float damage)
    {
        Quaternion rotation = transform.rotation;
        Instantiate(skillEffects, transform.position, rotation);
        griffinSkill.gameObject.SetActive(true);
        griffinSkill.damage = damage;
    }
}
