using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeimdallrPiece : Piece
{
    PathFinding pathFinding;
    protected override void Attack()
    {
        if (mana <= 110)
        {
            Skill();
            mana = 0;
        }
        else
        {
            base.Attack();
        }
    }

    protected override void Skill()
    {
        base.Skill();
        if (star == 0)
            GetLocationMultiRangeSkill(attackDamage * 1.5f);
        else if (star == 1)
            GetLocationMultiRangeSkill(attackDamage * 2.2f);
        else if (star == 2)
            GetLocationMultiRangeSkill(attackDamage * 3.5f);
    }

    void GetLocationMultiRangeSkill(float heal)
    {
        List<Tile> _getNeigbor = pathFinding.GetNeighbor(currentTile);
        foreach (var _Neigbor in _getNeigbor)
        {
            Piece _targets = _Neigbor.GetComponent<Piece>();
            if (_targets.isOwned)
                _targets.health += heal;
        }
    }
}
