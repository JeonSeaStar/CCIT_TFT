using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MummyPiece : Piece
{
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
        ShieldSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)));
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    void ShieldSkill(float shield)
    {
        if (dead)
            return;
        SkillState();
        SoundManager.instance.Play("SandKingdom/S_Mummy", SoundManager.Sound.Effect);
        Instantiate(skillEffects, this.transform.position, Quaternion.identity);
        this.shield = shield;
    }

    public override void SkillUpdateText()
    {
        pieceData.skillExplain = string.Format("�ڽſ��� �ش븦 ���ϰ� ���� {0}�� ��ȣ���� ����ϴ�.", abilityPower * (1 + (abilityPowerCoefficient / 100)));
    }
}
