using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenrirPiece : Piece
{
    public TriggerCheckSkill fenrirSkill;

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
        GetLocationMultiRangeSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)));
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    void GetLocationMultiRangeSkill(float damage)
    {
        if (dead)
            return;
        SkillState();
        SoundManager.instance.Play("FrostyWind/S_Fenrir", SoundManager.Sound.Effect);
        Quaternion rot = transform.rotation;
        Instantiate(skillEffects, transform.position, rot);
        fenrirSkill.gameObject.SetActive(true);
        fenrirSkill.damage = damage;
    }
    public override void SkillUpdateText()
    {
        pieceData.skillExplain = string.Format("���� 2���� ������ {0}�� ���ظ� ������ �����⸦ �մϴ�.", (abilityPower * (1 + (abilityPowerCoefficient / 100))));
    }
}
