using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreyaPiece : Piece
{
    protected override void Attack()
    {
        if (mana >= 80 && target != null)
        {
            Skill();
            mana = 0;
            Invoke("NextBehavior", attackSpeed);
        }
        else
        {
            base.Attack();
        }
    }

    protected override void Skill()
    {
        if (star == 0)
            FreezeSkill(1.2f, 1f);
        else if (star == 1)
            FreezeSkill(2.3f, 1.5f);
        else if (star == 2)
            FreezeSkill(3.5f, 2f);
    }

    void FreezeSkill(float damage, float time)
    {
        target.SkillDamage(damage);
        target.SetFreeze(time); //몇 초 동안 프리즈 되는지 시간 필요해 보임
    }
}
