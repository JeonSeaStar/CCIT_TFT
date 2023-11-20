using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeimdallrPiece : Piece
{
    PathFinding pathFinding;
    public override IEnumerator Attack()
    {
        if (mana >= 110)
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
            GetLocationMultiRangeSkill(attackDamage * 1.5f);
        else if (star == 1)
            GetLocationMultiRangeSkill(attackDamage * 2.2f);
        else if (star == 2)
            GetLocationMultiRangeSkill(attackDamage * 3.5f);
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
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
            else if (_targets.isOwned)
            {
                Instantiate(skillEffects, _targets.transform.position, Quaternion.identity);
                _targets.health += heal;
            }

        }
    }
}
