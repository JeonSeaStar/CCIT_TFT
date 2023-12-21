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
        AttackSpeedUp(4, 0.2f);
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
            SoundManager.instance.Play("FrostyWind/S_Drauger", SoundManager.Sound.Effect);
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
        pieceData.skillExplain = string.Format("{0}초 동안 자신의 공격속도를 {1}으로 고정시킵니다.", 4, 0.2f);
    }

    public override void Dead()
    {
        SoundManager.instance.Play("Wolf_Series/S_Death_Were_Wolf", SoundManager.Sound.Effect);
        StopAllCoroutines();
        currentTile.InitTile();
        gameObject.SetActive(false);
    }
}
