using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedusaPiece : Piece
{
    public override IEnumerator Attack()
    {
        if (mana >= maxMana && target != null)
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
        Attackkill(abilityPower * (1 + (abilityPowerCoefficient / 100)));
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    void Attackkill(float damage)
    {
        if(target != null)
        {
            Instantiate(skillEffects, target.transform.position, Quaternion.identity);
            Damage(damage);
        }
    }
    public override void SkillUpdateText()
    {
        pieceData.skillExplain = string.Format("현재 대상에게 {0}의 피해를 입히는 독니를 발사합니다.", (abilityPower * (1 + (abilityPowerCoefficient / 100))));
    }
}
