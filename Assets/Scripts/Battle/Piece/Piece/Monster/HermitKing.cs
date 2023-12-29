using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HermitKing : Piece
{
    public bool speedAttackUp;
    public float currentAttackSpeed;
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
            SoundManager.instance.Play("Shell_Series/S_Attack_Wolf_Cub", SoundManager.Sound.Effect);
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
            AttackSpeedUp(4, abilityPower);
        else if(star == 1)
            AttackSpeedUp(5, abilityPower);
        else if(star ==2)
            AttackSpeedUp(5, abilityPower);
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    public void AttackSpeedUp(float time, float rof)
    {
        if (dead)
            return;
        if (target != null)
        {
            SkillState();
            SoundManager.instance.Play("Shell_Series/S_Drauger", SoundManager.Sound.Effect);
            Instantiate(skillEffects, transform.position, Quaternion.identity);
            currentAttackSpeed = attackSpeed;
            SetAttackSpeedUp(time, rof);
        }
    }

    void SetAttackSpeedUp(float time, float rof)
    {
        speedAttackUp = true;
        if (gameObject.activeSelf)
        {
            attackSpeed = rof;
            Invoke("AttackSpeedUpClear", time);
        }
    }

    void AttackSpeedUpClear()
    {
        speedAttackUp = false;
        if (gameObject.activeSelf)
            attackSpeed = currentAttackSpeed;
    }

    public override void SkillUpdateText()
    {
        if (star == 0)
            pieceData.skillExplain = string.Format("{0}초 동안 자신의 공격속도를 {1}으로 고정시킵니다.", 4, abilityPower);
        else if (star == 1)
            pieceData.skillExplain = string.Format("{0}초 동안 자신의 공격속도를 {1}으로 고정시킵니다.", 5, abilityPower);
        else if (star == 2)
            pieceData.skillExplain = string.Format("{0}초 동안 자신의 공격속도를 {1}으로 고정시킵니다.", 6, abilityPower);
    }

    public override void Dead()
    {
        SoundManager.instance.Play("Shell_Series/S_Death_Were_Wolf", SoundManager.Sound.Effect);
        StopAllCoroutines();
        currentTile.InitTile();
        gameObject.SetActive(false);
    }
}
