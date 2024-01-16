using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SethPiece : Piece
{
    [SerializeField] private GameObject sethSkill;
    [SerializeField] private SethSkill skill;
    public override IEnumerator Attack()
    {
        if (mana >= maxMana)
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
        SoundManager.instance.Play("SandKingdom/S_Seth", SoundManager.Sound.Effect);
        GetTargetRangeSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)));
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    void GetTargetRangeSkill(float damage) //세트는 대상의 위치에서 스킬을 사용
    {
        if (target != null)
        {
            if (dead)
                return;
            SkillState();
            Quaternion rot = transform.rotation;
            Instantiate(skillEffects, transform.position, rot);
            Instantiate(sethSkill, target.transform.position, Quaternion.identity);
            skill.par = this;
            skill.damage = damage;
            
        }
    }
    public override void SkillUpdateText()
    {
        pieceData.skillExplain = string.Format("현재 대상을 기준으로 직선 범위에 {0}의 피해를 주는 휩쓸기를 시전합니다.", abilityPower * (1 + (abilityPowerCoefficient / 100)));
    }
}
