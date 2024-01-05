using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValkyriePiece : Piece
{
    public TriggerCheckSkill valkyrieSkill;
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
        TwoTileSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)));
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    public void TwoTileSkill(float damage)
    {
        if (dead)
            return;
        SkillState();
        SoundManager.instance.Play("FrostyWind/S_Valkyre", SoundManager.Sound.Effect);
        Quaternion rotation = transform.rotation;
        Instantiate(skillEffects, transform.position, rotation);
        if (!dead)
            valkyrieSkill.gameObject.SetActive(true);
        valkyrieSkill.damage = damage;
    }
    public override void SkillUpdateText()
    {
        pieceData.skillExplain = string.Format("현재 대상 방향으로 창을 내질러 2칸 범위에 {0}의 피해를 입힙니다. ", (abilityPower * (1 + (abilityPowerCoefficient / 100))));
    }
}
