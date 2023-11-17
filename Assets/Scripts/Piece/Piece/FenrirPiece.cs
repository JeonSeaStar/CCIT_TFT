using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenrirPiece : Piece
{
    PathFinding pathFinding;
    protected override void Attack()
    {
        if (mana >= 80)
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
            GetLocationMultiRangeSkill(attackDamage * 2.35f);
        else if (star == 1)
            GetLocationMultiRangeSkill(attackDamage * 3.4f);
        else if (star == 2)
            GetLocationMultiRangeSkill(attackDamage * 5.4f);
    }

    void GetLocationMultiRangeSkill(float damage)
    {
        pathFinding = ArenaManager.Instance.fieldManagers[0].pathFinding;
        List<Tile> _getNeigbor = pathFinding.GetFront(currentTile);
        foreach (var _Neigbor in _getNeigbor)
        {
            Piece _targets = _Neigbor.piece;
            if(_targets == null)
            {
                Debug.Log("대상없음");
            }
            else if(!_targets.isOwned)
            {
                _targets.SkillDamage(damage);
            }
        }
    }
}
