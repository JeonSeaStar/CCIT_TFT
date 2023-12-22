using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OsirisPiece : Piece
{
    Piece target;

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
        LeastHealthPieceHeal(abilityPower * (1 + (abilityPowerCoefficient / 100)));
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    public void LeastHealthPieceHeal(float heal)
    {
        if (dead)
            return;
        if (fieldManager.myFilePieceList != null)
        {
            SkillState();
            float value = 10000f;
            for (int i = 0; i < fieldManager.myFilePieceList.Count; i++)
            {
                if (value > fieldManager.myFilePieceList[i].health)
                {
                    value = fieldManager.myFilePieceList[i].health;
                    target = fieldManager.myFilePieceList[i];
                }
            }
            if(target != null)
            {
                SoundManager.instance.Play("SandKingdom/S_Osiris", SoundManager.Sound.Effect);
                Instantiate(skillEffects, target.transform.position, Quaternion.identity);
                target.health += heal;
            }
        }
    }
    public override void SkillUpdateText()
    {
        pieceData.skillExplain = string.Format("���� ����� �������� ���� ������ {0}�� ���ظ� �ִ� �۾��⸦ �����մϴ�.", abilityPower * (1 + (abilityPowerCoefficient / 100)));
    }
}
