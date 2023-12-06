using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HermesPiece : Piece
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

    public override IEnumerator Skill()
    {
        SkillState();
        if (star == 0)
        {
            BlindSkill(1.5f);
        }
        else if (star == 1)
        {
            BlindSkill(2f);
        }
        else if (star == 2)
        {
            BlindSkill(2.5f);
        }
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    void BlindSkill(float time)
    {
        if(target != null)
        {
            SoundManager.instance.Play("GreatMountain/S_Hermes", SoundManager.Sound.Effect);
            Instantiate(skillEffects, target.transform.position, Quaternion.identity);
            target.SetBlind(time);
            SetDebuff("Blind", time);
        }
    }
    public override void SkillUpdateText()
    {
        if (star == 0)
        {
            pieceData.skillExplain = string.Format("현재 대상을 {0}초 동안 실명 상태로 만듭니다.", 1.5);
        }
        else if (star == 1)
        {
            pieceData.skillExplain = string.Format("현재 대상을 {0}초 동안 실명 상태로 만듭니다.", 2);
        }
        else if (star == 2)
        {
            pieceData.skillExplain = string.Format("현재 대상을 {0}초 동안 실명 상태로 만듭니다.", 2.5);
        }
    }
}
