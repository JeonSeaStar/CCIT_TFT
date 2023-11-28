using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SethPiece : Piece
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
        GetLocationMultiRangeSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)));
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    void GetLocationMultiRangeSkill(float damage)
    {
        Quaternion rot = transform.rotation;
        Instantiate(skillEffects, transform.position, rot);
        //foreach (var _Neigbor in _getNeigbor)
        //{
        //    Piece _targets = _Neigbor.piece;
        //    if (_targets == null)
        //    {
        //        Debug.Log("대상없음");
        //    }
        //    else if (!_targets.isOwned)
        //    {
        //        Damage(_targets, damage);
        //    }
        //}
    }
}
