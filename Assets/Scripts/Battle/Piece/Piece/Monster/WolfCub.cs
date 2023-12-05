using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfCub : Piece
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
            SoundManager.instance.Play("Wolf_Series/S_Attack_Wolf_Cub", SoundManager.Sound.Effect);
            DoAttack();
        }
    }

    public override IEnumerator Skill()
    {
        ShieldSkill(300f);
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    void ShieldSkill(float shield)
    {
        SoundManager.instance.Play("Wolf_Series/S_Skill_Wolf_Cub", SoundManager.Sound.Effect);
        SkillState();
        Instantiate(skillEffects, this.transform.position, Quaternion.identity);
        this.shield = shield;
    }

    public override void SkillUpdateText()
    {
        pieceData.skillExplain = string.Format("{0}의 피해를 흡수하는 보호막을 얻습니다.", 300);
    }

    public override void Dead()
    {
        SoundManager.instance.Play("Wolf_Series/S_Death_Wolf_Cub", SoundManager.Sound.Effect);
        base.Dead();
    }
}
