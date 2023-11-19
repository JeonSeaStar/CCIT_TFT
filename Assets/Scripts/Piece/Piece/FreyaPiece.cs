using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreyaPiece : Piece
{
    public override IEnumerator Attack()
    {
        if (mana >= 80 && target != null)
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
        if (star == 0)
            FreezeSkill(1.2f, 1f);
        else if (star == 1)
            FreezeSkill(2.3f, 1.5f);
        else if (star == 2)
            FreezeSkill(3.5f, 2f);
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    void FreezeSkill(float damage, float time)
    {
        target.SkillDamage(damage);
        target.SetFreeze(time); //몇 초 동안 프리즈 되는지 시간 필요해 보임
    }
}
