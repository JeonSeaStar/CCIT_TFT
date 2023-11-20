using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MummyPiece : Piece
{
    public override IEnumerator Attack()
    {
        if (mana <= 60)
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
            Instantiate(skillEffects, this.transform.position, Quaternion.identity);
            this.shield = 130f;
        }
        else if (star == 1)
        {
            Instantiate(skillEffects, this.transform.position, Quaternion.identity);
            this.shield = 230f;
        }
        else if (star == 2)
        {
            Instantiate(skillEffects, this.transform.position, Quaternion.identity);
            this.shield = 500f;
        }
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }
}
