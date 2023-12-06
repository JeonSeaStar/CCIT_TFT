using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OsirisPiece : Piece
{
    float pieceHealth = 5000f;

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
        FindLeastHealthPiece(abilityPower * (1 + (abilityPowerCoefficient / 100)));
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    public void FindLeastHealthPiece(float heal)
    {
        if (fieldManager.myFilePieceList != null)
        {
            SoundManager.instance.Play("SandKingdom/S_Osiris", SoundManager.Sound.Effect);
            for (int i = 0; i < fieldManager.myFilePieceList.Count; i++)
            {
                if (pieceHealth > fieldManager.myFilePieceList[i].health)
                {
                    pieceHealth = fieldManager.myFilePieceList[i].health;
                }
                Instantiate(skillEffects, fieldManager.myFilePieceList[i].transform.position, Quaternion.identity);
                fieldManager.myFilePieceList[i].health += heal;
            }
        }
    }
    public override void SkillUpdateText()
    {
        pieceData.skillExplain = string.Format("현재 대상을 기준으로 직선 범위에 {0}의 피해를 주는 휩쓸기를 시전합니다.", abilityPower * (1 + (abilityPowerCoefficient / 100)));
    }
}
