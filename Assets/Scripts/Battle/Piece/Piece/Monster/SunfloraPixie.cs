using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunfloraPixie : Piece
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
            SoundManager.instance.Play("SandKingdom/Sound_for_Horus_01", SoundManager.Sound.Effect);
            foreach(var _targets in fieldManager.myFilePieceList)
            {
                Instantiate(skillEffects, _targets.transform.position, Quaternion.identity);
                Damage(_targets,damage);
            }
        }
        else
            return;
    }
    public override void SkillUpdateText()
    {
        pieceData.skillExplain = string.Format("1초동안 정신 집중한 뒤 모든 적에게 {0}의 피해를 주는 빛의 심판을 내립니다.", abilityPower * (1 + (abilityPowerCoefficient / 100)));
    }

    public override void Dead()
    {
        SoundManager.instance.Play("Wolf_Series/S_Death_Were_Wolf", SoundManager.Sound.Effect);
        StopAllCoroutines();
        currentTile.InitTile();
        gameObject.SetActive(false);
    }
}
