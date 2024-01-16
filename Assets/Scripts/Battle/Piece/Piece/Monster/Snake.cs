using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : Piece
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
    public override void DoAttack()
    {
        if (stun || freeze)
        {
            pieceState = State.IDLE;
            return;
        }
        if (target != null)
        {
            invincible = false;
            SoundManager.instance.Play("Snake_Series/S_Attack_Snake", SoundManager.Sound.Effect);
            Damage(attackDamage);
            mana += manaRecovery;
            StartNextBehavior();
        }
        else
            IdleState();
    }

    public override IEnumerator Skill()
    {
        if(star == 0)
            BlindSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)), 1.3f);
        else if(star == 1)
            BlindSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)), 1.5f);
        else if(star == 2)          
            BlindSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)), 1.8f);
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    public void BlindSkill(float damage, float time)
    {
        if (dead)
            return;
        if (target != null)
        {
            SkillState();
            SoundManager.instance.Play("Snake_Series/S_Skill_Snake", SoundManager.Sound.Effect);
            Instantiate(skillEffects, target.transform.position, Quaternion.identity);
            Damage(damage);
            if(target != null)
                target.SetBlind(time);
        }
    }

    public override void SkillUpdateText()
    {
        if (star == 0)
            pieceData.skillExplain = string.Format("현재 공격 대상에게 산성 침을 뱉어 {0}의 피해를 입히고 {1}초 동안 실명시킵니다.", abilityPower * (1 + (abilityPowerCoefficient / 100)), 1.3);
        else if (star == 1)
            pieceData.skillExplain = string.Format("현재 공격 대상에게 산성 침을 뱉어 {0}의 피해를 입히고 {1}초 동안 실명시킵니다.", abilityPower * (1 + (abilityPowerCoefficient / 100)), 1.5);
        else if (star == 2)
            pieceData.skillExplain = string.Format("현재 공격 대상에게 산성 침을 뱉어 {0}의 피해를 입히고 {1}초 동안 실명시킵니다.", abilityPower * (1 + (abilityPowerCoefficient / 100)), 1.8);
    }

    public override void Dead()
    {
        SoundManager.instance.Play("Snake_Series/S_Death_Snake", SoundManager.Sound.Effect);
        StopAllCoroutines();
        currentTile.InitTile();
        gameObject.SetActive(false);
    }
}
