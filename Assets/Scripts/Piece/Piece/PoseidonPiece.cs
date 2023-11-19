using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoseidonPiece : Piece
{
    public List<Tile> tiles;
    public override IEnumerator Attack()
    {
        if (mana <= 80)
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
        {
            DamageAllTile(attackDamage);
        }
        else if (star == 1)
        {
            DamageAllTile(attackDamage * 1.5f);
        }
        else if (star == 2)
        {
            DamageAllTile(attackDamage * 3.75f);
        }
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    void DamageAllTile(float damage)
    {
        List<Tile> _getHalf = tiles;
        foreach (var _Neigbor in _getHalf)
        {
            Piece _targets = _Neigbor.GetComponent<Piece>();
            if (_targets == null)
            {
                Debug.Log("대상없음");
            }
            else if (!_targets.isOwned)
            {
                _targets.SkillDamage(damage);
            }
        }
    }
}
