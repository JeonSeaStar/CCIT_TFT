using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurtrPiece : Piece
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
        ShieldSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)));
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    void ShieldSkill(float shield)
    {
        if (dead)
            return;
        SkillState();
        SoundManager.instance.Play("FrostyWind/S_Surtr", SoundManager.Sound.Effect);
        Instantiate(skillEffects, this.transform.position, Quaternion.identity);
        this.shield = shield;
    }
    public override void SkillUpdateText()
    {
        pieceData.skillExplain = string.Format("{0}의 피해를 흡수하는 보호막을 얻는 주술을 시전합니다.", (abilityPower * (1 + (abilityPowerCoefficient / 100))));
    }
}
