using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : Piece
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
            SoundManager.instance.Play("Wolf_Series/S_Attack_Wolf_Cub", SoundManager.Sound.Effect);
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
            DamageAndStundSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)), 1.5f);
        else if(star ==1)
            DamageAndStundSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)), 2f);
        else if(star == 2)
            DamageAndStundSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)), 2.5f);
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    public void DamageAndStundSkill(float damage, float time)
    {
        if (dead)
            return;
        if (target != null)
        {
            SkillState();
            SoundManager.instance.Play("FrostyWind/S_Drauger", SoundManager.Sound.Effect);
            Instantiate(skillEffects, target.transform.position, Quaternion.identity);
            target.SetStun(time);
            Damage(damage);
        }
    }

    public override void SkillUpdateText()
    {
        if (star == 0)
            pieceData.skillExplain = string.Format("현재 공격대상에게 빠르게 굴러가 {0}의 피해를 입히고 {1}초 동안 기절시킵니다.", abilityPower * (1 + (abilityPowerCoefficient / 100)), 1.5);
        else if (star == 1)
            pieceData.skillExplain = string.Format("현재 공격대상에게 빠르게 굴러가 {0}의 피해를 입히고 {1}초 동안 기절시킵니다.", abilityPower * (1 + (abilityPowerCoefficient / 100)), 2);
        else if (star == 2)
            pieceData.skillExplain = string.Format("현재 공격대상에게 빠르게 굴러가 {0}의 피해를 입히고 {1}초 동안 기절시킵니다.", abilityPower * (1 + (abilityPowerCoefficient / 100)), 2.5);
    }

    public override void Dead()
    {
        SoundManager.instance.Play("Wolf_Series/S_Death_Were_Wolf", SoundManager.Sound.Effect);
        StopAllCoroutines();
        currentTile.InitTile();
        gameObject.SetActive(false);
    }
}
