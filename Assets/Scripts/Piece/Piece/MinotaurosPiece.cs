using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinotaurosPiece : Piece
{
    public override IEnumerator Attack()
    {
        if (mana >= 100)
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
        {
            this.shield = 150f;
            Instantiate(skillEffects, this.transform.position, Quaternion.identity);
        }
        else if (star == 1)
        {
            this.shield = 250f;
            Instantiate(skillEffects, this.transform.position, Quaternion.identity);
        }
        else if (star == 2)
        {
            this.shield = 350f;
            Instantiate(skillEffects, this.transform.position, Quaternion.identity);
        }
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }
}
