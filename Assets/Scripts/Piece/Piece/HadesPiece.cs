using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HadesPiece : Piece
{
    PathFinding pathFinding;
    protected override void Attack()
    {
        if (mana >= 100)
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
        {
            GetLocationMultiRangeSkill(200f);
            this.shield = 350f;
        }
        else if (star == 1)
        {
            GetLocationMultiRangeSkill(350f);
            this.shield = 450f;
        }
        else if (star == 2)
        {
            GetLocationMultiRangeSkill(500f);
            this.shield = 600f;
        }
    }

    void GetLocationMultiRangeSkill(float damage)
    {
        pathFinding = ArenaManager.Instance.fieldManagers[0].pathFinding;
        List<Tile> _getNeigbor = pathFinding.GetNeighbor(currentTile);
        foreach (var _Neigbor in _getNeigbor)
        {
            Piece _targets = _Neigbor.piece;
            if(_targets == null)
            {
                Debug.Log("������");
            }
            else if (!_targets.isOwned)
            {
                _targets.SkillDamage(damage);
            }
        }
    }
}