using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MimirPiece : Piece
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
            SoundManager.instance.Play("FrostyWind/S_Mimir", SoundManager.Sound.Effect);
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
        pieceData.skillExplain = string.Format("현재 체력이 가장 낮은 아군 기물의 체력을 {0}만큼 회복시킵니다.", (abilityPower * (1 + (abilityPowerCoefficient / 100))));
    }
}
