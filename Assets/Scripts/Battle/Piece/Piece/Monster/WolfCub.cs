using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfCub : Piece
{
    public override IEnumerator Attack()
    {
        if (mana >= 60)
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
        ShieldSkill(300f);
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    void ShieldSkill(float shield)
    {
        Instantiate(skillEffects, this.transform.position, Quaternion.identity);
        this.shield = shield;
    }
}
