using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonInferno : Piece
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
            SoundManager.instance.Play("Dragon_Series/S_Attack_DragonInferno", SoundManager.Sound.Effect);
            Damage(attackDamage);
            mana += manaRecovery;
            StartNextBehavior();
        }
        else
            IdleState();
    }

    public override IEnumerator Skill()
    {
        AllEnemyDamge(abilityPower * (1 + (abilityPowerCoefficient / 100)));
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    void AllEnemyDamge(float damage)
    {
        if (dead)
            return;
        if (fieldManager.myFilePieceList.Count > 0)
        {
            SkillState();
            SoundManager.instance.Play("Dragon_Series/S_Skill_DragonInferno", SoundManager.Sound.Effect);
            foreach (var _targets in fieldManager.myFilePieceList)
            {
                Instantiate(skillEffects, transform.position, Quaternion.identity);
                Damage(_targets, damage);
            }
        }
        else
            return;
    }
    public override void SkillUpdateText()
    {
        pieceData.skillExplain = string.Format("불꽃의 기운을 폭발시켜 모든 적에게 {0}의 피해를 입힙니다.", abilityPower * (1 + (abilityPowerCoefficient / 100)));
    }

    public override void Dead()
    {
        SoundManager.instance.Play("Dragon_Series/S_Death_DragonInferno", SoundManager.Sound.Effect);
        StopAllCoroutines();
        currentTile.InitTile();
        gameObject.SetActive(false);
    }
}
