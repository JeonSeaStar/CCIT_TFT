using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphinxPiece : Piece
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
        if (star == 0)
            StunAndDamageSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)), 0.7f);
        else if (star == 1)
            StunAndDamageSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)), 1.2f);
        else if (star == 2)
            StunAndDamageSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)), 1.8f);
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    void StunAndDamageSkill(float damage, float time)
    {
        if(target != null)
        {
            if (dead)
                return;
            SkillState();
            SoundManager.instance.Play("SandKingdom/S_Spinx", SoundManager.Sound.Effect);
            Instantiate(skillEffects, new Vector3(target.transform.position.x, target.transform.position.y + 0.8f, target.transform.position.z), Quaternion.identity);
            target.SetStun(time);
            Damage(damage);
        }
    }
    public override void SkillUpdateText()
    {
        if (star == 0)
            pieceData.skillExplain = string.Format("현재 대상에게 펀치를 날려 {0}의 피해를 입히고 {1}초 동안 기절시킵니다.", abilityPower * (1 + (abilityPowerCoefficient / 100)), 0.7);
        else if (star == 1)
            pieceData.skillExplain = string.Format("현재 대상에게 펀치를 날려 {0}의 피해를 입히고 {1}초 동안 기절시킵니다.", abilityPower * (1 + (abilityPowerCoefficient / 100)), 1.2);
        else if (star == 2)
            pieceData.skillExplain = string.Format("현재 대상에게 펀치를 날려 {0}의 피해를 입히고 {1}초 동안 기절시킵니다.", abilityPower * (1 + (abilityPowerCoefficient / 100)), 1.8);
    }
}
