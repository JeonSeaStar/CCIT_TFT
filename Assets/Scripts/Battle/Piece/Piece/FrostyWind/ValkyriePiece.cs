using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValkyriePiece : Piece
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

    public void AttackSkill(float damage) //��Ű�� ���� �ʿ�
    {
        if (dead)
            return;
        if (target != null)
        {
            SkillState();
            SoundManager.instance.Play("FrostyWind/S_Valkyre", SoundManager.Sound.Effect);
            Instantiate(skillEffects, new Vector3(target.transform.position.x, target.transform.position.y + 0.8f, target.transform.position.z), Quaternion.identity);
            Damage(damage);
        }
    }
    public override void SkillUpdateText()
    {
        pieceData.skillExplain = string.Format("���� ��� �������� â�� ������ 2ĭ ������ {0}�� ���ظ� �����ϴ�. ", (abilityPower * (1 + (abilityPowerCoefficient / 100))));
    }
}
