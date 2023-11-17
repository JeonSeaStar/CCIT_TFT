using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OdinPiece : Piece
{
    protected override void Attack()
    {
        if (mana <= 99)
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
        {
            AllPieceHealSkill(1.3f);
        }
        else if (star == 1)
        {
            AllPieceHealSkill(1.6f);
        }
        else if (star == 2)
        {
            AllPieceHealSkill(1.99f);
        }
    }

    void AllPieceHealSkill(float buff)
    {
        List<Piece> _allPiece = fieldManager.enemyFilePieceList;
        foreach (var _Neigbor in _allPiece)
        {
            Piece _targets = _Neigbor.GetComponent<Piece>();
            if(_targets.isOwned)
                _targets.attackDamage = attackDamage * buff;
        }
    }
}
