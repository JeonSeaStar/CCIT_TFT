using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApollonPiece : Piece
{
    PathFinding pathFinding;
    protected override void Attack()
    {
        if (mana >= 120)
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
            GetLocationMultiRangeSkill(1f);
        else if (star == 1)
            GetLocationMultiRangeSkill(1.5f);
        else if (star == 2)
            GetLocationMultiRangeSkill(2f);
    }

    void GetLocationMultiRangeSkill(float time)
    {
        pathFinding = ArenaManager.Instance.fieldManagers[0].pathFinding;
        List<Tile> _getNeigbor = pathFinding.GetNeighbor(currentTile);
        foreach (var _Neigbor in _getNeigbor)
        {
            Piece _targets = _Neigbor.piece;
            if (_targets == null)
            {
                Debug.Log("대상 없음");
            }
            else if (!_targets.isOwned)
            {
                _targets.SetStun(time);
            }
        }
    }
}

