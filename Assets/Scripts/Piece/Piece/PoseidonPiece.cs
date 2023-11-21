using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoseidonPiece : Piece
{
    public override IEnumerator Attack()
    {
        if (mana >= 80)
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
            AllPieceDamageSkill(attackDamage);
        }
        else if (star == 1)
        {
            AllPieceDamageSkill(attackDamage * 1.5f);
        }
        else if (star == 2)
        {
            AllPieceDamageSkill(attackDamage * 3.75f);
        }
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    void AllPieceDamageSkill(float damage)
    {
        List<Piece> _allPiece = fieldManager.enemyFilePieceList;
        foreach (var _Neigbor in _allPiece)
        {
            Piece _targets = _Neigbor.GetComponent<Piece>();
            if (_targets == null)
            {
                Debug.Log("대상없음");
            }
            else
            {
                Instantiate(skillEffects, _targets.transform.position, Quaternion.identity);
                Damage(_targets, damage);
            }
        }
    }
}
