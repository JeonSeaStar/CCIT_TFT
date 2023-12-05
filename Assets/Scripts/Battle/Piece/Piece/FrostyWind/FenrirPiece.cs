using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenrirPiece : Piece
{
    public TriggerCheckSkill fenrirSkill;

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
        Quaternion rot = transform.rotation;
        Instantiate(skillEffects, transform.position, rot);
        fenrirSkill.gameObject.SetActive(true);
        fenrirSkill.damage = damage;
    }
}
