using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GriffinPiece : Piece
{
    PathFinding pathFinding;
    protected override void Attack()
    {
        if (mana <= 100)
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
            GetLocationMultiRangeSkill(attackDamage * 1.8f);
        else if (star == 1)
            GetLocationMultiRangeSkill(attackDamage * 2.7f);
        else if (star == 2)
            GetLocationMultiRangeSkill(attackDamage * 4f);
    }

    void GetLocationMultiRangeSkill(float damage)
    {
        pathFinding = ArenaManager.Instance.fieldManagers[0].pathFinding;
        List<Tile> _getNeigbor = pathFinding.GetFront(currentTile);
        foreach (var _Neigbor in _getNeigbor)
        {
            Piece _targets = _Neigbor.GetComponent<Piece>();
            _targets.Damage(damage);
        }
    }
}
