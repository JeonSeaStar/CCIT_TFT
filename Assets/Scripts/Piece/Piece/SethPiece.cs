using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SethPiece : Piece
{
    PathFinding pathFinding;
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
        if (star == 0)
            GetLocationMultiRangeSkill(attackDamage * 1.45f);
        else if (star == 1)
            GetLocationMultiRangeSkill(attackDamage * 2.1f);
        else if (star == 2)
            GetLocationMultiRangeSkill(attackDamage * 3.5f);
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    void GetLocationMultiRangeSkill(float damage)
    {
        pathFinding = ArenaManager.Instance.fieldManagers[0].pathFinding;
        List<Tile> _getNeigbor = pathFinding.GetStrangeSide(targetTile);
        foreach (var _Neigbor in _getNeigbor)
        {
            Piece _targets = _Neigbor.GetComponent<Piece>();
            if (_targets == null)
            {
                Debug.Log("대상없음");
            }
            else if (!_targets.isOwned)
            {
                Instantiate(skillEffects, _targets.transform.position, Quaternion.identity);
                Damage(_targets, damage);
            }
        }
    }
}
