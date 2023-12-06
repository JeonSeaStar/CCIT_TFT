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
            //print(name + "(��)��" + target.name + "���� �Ϲ� ������ �մϴ�.");
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
        pieceData.skillExplain = string.Format("�ɰ��縦 �ѷ� �ڽ��� ü���� {0}��ŭ ȸ����ŵ�ϴ�.", 200);
    }

    public override void Dead()
    {
        SoundManager.instance.Play("Nepenthes_Seris/S_Death_Bud", SoundManager.Sound.Effect);
        StopAllCoroutines();
        currentTile.InitTile();
        gameObject.SetActive(false);
    }
}
