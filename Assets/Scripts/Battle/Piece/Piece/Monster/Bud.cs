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
            SoundManager.instance.Play("Nepenthes_Seris/S_Attack_Bud", SoundManager.Sound.Effect);
            DoAttack();
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
        SoundManager.instance.Play("Nepenthes_Seris/S_Skill_Bud", SoundManager.Sound.Effect);
        SkillState();
        Instantiate(skillEffects, transform.position, Quaternion.identity);
        health += heal;
    }
    public override void SkillUpdateText()
    {
        pieceData.skillExplain = string.Format("꽃가루를 뿌려 자신의 체력을 {0}만큼 회복시킵니다.", 200);
    }
}
