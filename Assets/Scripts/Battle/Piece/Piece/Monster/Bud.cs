using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bud : Piece
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
            SoundManager.instance.Play("Nepenthes_Seris/S_Attack_Bud", SoundManager.Sound.Effect);
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
        HealSkill(200f);
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    void HealSkill(float heal)
    {
        if (dead)
            return;
        SoundManager.instance.Play("Nepenthes_Seris/S_Skill_Bud", SoundManager.Sound.Effect);
        SkillState();
        Instantiate(skillEffects, transform.position, Quaternion.identity);
        health += heal;
    }

    public override void SkillUpdateText()
    {
        pieceData.skillExplain = string.Format("꽃가루를 뿌려 자신의 체력을 {0}만큼 회복시킵니다.", 200);
    }

    public override void Dead()
    {
        SoundManager.instance.Play("Nepenthes_Seris/S_Death_Bud", SoundManager.Sound.Effect);
        StopAllCoroutines();
        currentTile.InitTile();
        gameObject.SetActive(false);
    }
}
