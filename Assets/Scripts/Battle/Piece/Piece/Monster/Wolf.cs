using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : Piece
{
    public override IEnumerator Attack()
    {
        if (mana >= pieceData.mana[star])
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
        ShieldSkill(500f);
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    void ShieldSkill(float shield)
    {
        SkillState();
        Instantiate(skillEffects, this.transform.position, Quaternion.identity);
        this.shield = shield;
    }
    public override void SkillUpdateText()
    {
        pieceData.skillExplain = string.Format("{0}의 피해를 흡수하는 보호막을 얻습니다.", 500);
    }
}
