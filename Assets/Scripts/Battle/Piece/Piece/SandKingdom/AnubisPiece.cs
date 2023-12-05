using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnubisPiece : Piece
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
        AttackSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)));
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    public void AttackSkill(float damage)
    {
        if (target != null)
        {
            SoundManager.instance.Play("SandKingdom/S_Anubis", SoundManager.Sound.Effect);
            Instantiate(skillEffects, target.transform.position, Quaternion.identity);
            Damage(damage);
        }
    }
    public override void SkillUpdateText()
    {
        pieceData.skillExplain = string.Format("���� ��󿡰� {0}�� ���ظ� �ִ� ����ġ�⸦ �����մϴ�.", abilityPower * (1 + (abilityPowerCoefficient / 100)));
    }
}
