using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraugrPiece : Piece
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
        AttackSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)));
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    public void AttackSkill(float damage)
    {
        if (target != null)
        {
            SoundManager.instance.Play("FrostyWind/S_Drauger", SoundManager.Sound.Effect);
            Instantiate(skillEffects, target.transform.position, Quaternion.identity);
            Damage(damage);
        }
    }

    public override void SkillUpdateText()
    {
        pieceData.skillExplain = string.Format("현재 대상에게 {0}의 피해를 주는 세로 베기를 시전합니다.", abilityPower * (1 + (abilityPowerCoefficient / 100)));
    }
}
