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
    public override void SkillUpdateText()
    {
        pieceData.skillExplain = string.Format("전방 2개의 타일에 {0}의 피해를 입히는 발차기를 합니다.", (abilityPower * (1 + (abilityPowerCoefficient / 100))));
    }
}
