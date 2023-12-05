using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThothPiece : Piece
{
    PathFinding pathFinding;
    public override IEnumerator Attack()
    {
        if (mana >= maxMana && target != null)
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
        ProjectionSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)));
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    void ProjectionSkill(float damage)
    {
        if(target != null)
        {
            Instantiate(skillEffects, new Vector3(target.transform.position.x, target.transform.position.y + 0.8f, target.transform.position.z), Quaternion.identity);
            pathFinding = ArenaManager.Instance.fieldManagers[0].pathFinding;
            List<Tile> _getNeigbor = pathFinding.GetSide(currentTile);
            foreach (var _Neigbor in _getNeigbor)
            {
                Piece _targets = _Neigbor.piece;
                if (_targets == null)
                {
                    Debug.Log("������");
                }
                else if (!_targets.isOwned)
                {
                    Damage(_targets, damage);
                }
            }
        }
    }
}
