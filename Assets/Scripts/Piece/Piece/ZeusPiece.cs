using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeusPiece : Piece
{
    public override IEnumerator Attack()
    {
        if (mana >= 60)
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
            StartCoroutine(GetLockTarget(abilityPower * (1 + (abilityPowerCoefficient / 100)), 3f));
        }
        else if (star == 1)
        {
            StartCoroutine(GetLockTarget(abilityPower * (1 + (abilityPowerCoefficient / 100)), 6f));
        }
        else if (star == 2)
        {
            StartCoroutine(GetLockTarget(abilityPower * (1 + (abilityPowerCoefficient / 100)), 9f));
        }
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    IEnumerator GetLockTarget(float damage, float time)
    {
        for (int i = 0; i < time; i++)
        {
            if (target == null)
                yield break; //or yield return null;
            else
            {
                Damage(damage);
                Instantiate(skillEffects, target.transform.position, Quaternion.identity);
            }
            yield return new WaitForSeconds(1f);
        }
    }
}
