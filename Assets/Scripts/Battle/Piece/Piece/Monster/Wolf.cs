using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : Piece
{
    public override IEnumerator Attack()
    {
        if (mana >= pieceData.mana[star])
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
            SoundManager.instance.Play("Wolf_Series/S_Attack_Wolf", SoundManager.Sound.Effect);
            //print(name + "(이)가" + target.name + "에게 일반 공격을 합니다.");
            Damage(attackDamage);
            //mana += 100;
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
        ShieldSkill(500f);
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    void ShieldSkill(float shield)
    {
        if (dead)
            return;
        SoundManager.instance.Play("Wolf_Series/S_Skill_Wolf", SoundManager.Sound.Effect);
        SkillState();
        Instantiate(skillEffects, this.transform.position, Quaternion.identity);
        this.shield = shield;
    }

    public override void SkillUpdateText()
    {
        pieceData.skillExplain = string.Format("{0}의 피해를 흡수하는 보호막을 얻습니다.", 500);
    }

    public override void Dead()
    {
        SoundManager.instance.Play("Wolf_Series/S_Death_Wolf", SoundManager.Sound.Effect);
        StopAllCoroutines();
        currentTile.InitTile();
        gameObject.SetActive(false);
    }
}
