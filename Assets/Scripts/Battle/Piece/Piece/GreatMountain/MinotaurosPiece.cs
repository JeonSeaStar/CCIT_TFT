using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinotaurosPiece : Piece
{
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
        SkillState();
        ShieldSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)));
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    void ShieldSkill(float shield)
    {
        SoundManager.instance.Play("GreatMountain/S_Minotauros", SoundManager.Sound.Effect);
        Instantiate(skillEffects, this.transform.position, Quaternion.identity);
        this.shield = shield;
    }
    public override void SkillUpdateText()
    {
        pieceData.skillExplain = string.Format("{0}의 피해를 흡수하는 보호막을 얻습니다.", (abilityPower * (1 + (abilityPowerCoefficient / 100))));
    }
}
