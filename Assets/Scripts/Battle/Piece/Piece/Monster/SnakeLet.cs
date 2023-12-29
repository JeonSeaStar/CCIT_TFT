using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeLet : Piece
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
            SoundManager.instance.Play("Snake_Series/S_Attack_Wolf_Cub", SoundManager.Sound.Effect);
            Damage(attackDamage);
            mana += manaRecovery;
            StartNextBehavior();
        }
        else
            IdleState();
    }

    public override IEnumerator Skill()
    {
        BuffSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)));
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    public void BuffSkill(float damage)
    {
        if (dead)
            return;
        if (target != null)
        {
            SkillState();
            SoundManager.instance.Play("Snake_Series/S_Drauger", SoundManager.Sound.Effect);
            Instantiate(skillEffects, target.transform.position, Quaternion.identity);
            if (fieldManager.enemyFilePieceList.Count > 0)
            {
                foreach (var _target in fieldManager.enemyFilePieceList)
                {
                    _target.attackDamage = _target.attackDamage + damage;
                }
            }
            else
                return;
        }
    }

    public override void SkillUpdateText()
    {
        pieceData.skillExplain = string.Format("이번 전투동안 모든 아군의 공격력을 {0} 만큼 증가시킵니다.", abilityPower * (1 + (abilityPowerCoefficient / 100)));
    }

    public override void Dead()
    {
        SoundManager.instance.Play("Snake_Series/S_Death_Were_Wolf", SoundManager.Sound.Effect);
        StopAllCoroutines();
        currentTile.InitTile();
        gameObject.SetActive(false);
    }
}
