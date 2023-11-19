using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeimdallrPiece : Piece
{
    PathFinding pathFinding;
    protected override IEnumerator Attack()
    {
        if (mana >= 110)
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

    protected override IEnumerator Skill()
    {
        if (star == 0)
            GetLocationMultiRangeSkill(attackDamage * 1.5f);
        else if (star == 1)
            GetLocationMultiRangeSkill(attackDamage * 2.2f);
        else if (star == 2)
            GetLocationMultiRangeSkill(attackDamage * 3.5f);
    }

    void GetLocationMultiRangeSkill(float heal)
    {
        pathFinding = ArenaManager.Instance.fieldManagers[0].pathFinding;
        List<Tile> _getNeigbor = pathFinding.GetNeighbor(currentTile);
        foreach (var _Neigbor in _getNeigbor)
        {
            Piece _targets = _Neigbor.piece;
            if (_targets == null)
            {
                Debug.Log("대상없음");
            }
            else if(_targets.isOwned)
            {
                _targets.health += heal;
            }

        }
    }
}
