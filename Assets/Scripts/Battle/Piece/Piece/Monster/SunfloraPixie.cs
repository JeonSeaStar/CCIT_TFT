using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunfloraPixie : Piece
{
    public override IEnumerator Attack()
    {
        if (mana >= pieceData.mana[star] && target != null)
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
            SoundManager.instance.Play("Wolf_Series/S_Attack_Were_Wolf", SoundManager.Sound.Effect);
            Damage(attackDamage);
            mana += manaRecovery;
            StartNextBehavior();
        }
        else
        {
            IdleState();
        }
    }

    public override IEnumerator Skill()
    {
        TargetSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)));
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    //총알을 쏴서 여러번 반복 시행 한다?
    void TargetSkill(float heal)
    {
        if (dead)
            return;
        if (target != null)
        {
            SkillState();
            SoundManager.instance.Play("Nepenthes_Series/S_Skil_Bloom", SoundManager.Sound.Effect);
            Instantiate(skillEffects, target.transform.position, Quaternion.identity);
            target.health = target.health + heal;
        }
    }

    public override void SkillUpdateText()
    {
        pieceData.skillExplain = string.Format("현재 공격 대상의 체력을 {0}만큼 회복시킵니다.", abilityPower * (1 + (abilityPowerCoefficient / 100)));
    }

    public override void Dead()
    {
        SoundManager.instance.Play("Wolf_Series/S_Death_Were_Wolf", SoundManager.Sound.Effect);
        StopAllCoroutines();
        currentTile.InitTile();
        gameObject.SetActive(false);
    }
}
