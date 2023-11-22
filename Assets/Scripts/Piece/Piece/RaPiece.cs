using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaPiece : Piece
{
    public override IEnumerator Attack()
    {
        if (mana >= 70 && target != null)
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
            RaSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)), 0.7f);
        }
        else if (star == 1)
        {
            RaSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)), 1.2f);
        }
        else if (star == 2)
        {
            RaSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)), 1.7f);
        }
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    public void RaSkill(float damage, float time)
    {
        if (fieldManager.enemyFilePieceList != null)
        {
            for (int i = 0; i < fieldManager.enemyFilePieceList.Count; i++)
            {
                fieldManager.enemyFilePieceList[i].Damage(damage);
                fieldManager.enemyFilePieceList[i].SetStun(time);
                Instantiate(skillEffects, fieldManager.enemyFilePieceList[i].transform.position, Quaternion.identity);
            }
        }
    }
}
