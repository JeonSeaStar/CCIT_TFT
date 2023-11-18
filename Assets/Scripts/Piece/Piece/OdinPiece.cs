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
            AllPieceBuffSkill(1.3f);
        }
        else if (star == 1)
        {
            AllPieceBuffSkill(1.6f);
        }
        else if (star == 2)
        {
            AllPieceBuffSkill(1.99f);
        }
    }

    void AllPieceBuffSkill(float buff)
    {
        List<Piece> _allPiece = fieldManager.myFilePieceList;
        foreach (var _Neigbor in _allPiece)
        {
            Piece _targets = _Neigbor.GetComponent<Piece>();
            if(_targets == null)
            {
                Debug.Log("������");
            }
            else if(_targets.isOwned)
            {
                _targets.attackDamage = attackDamage * buff;
            }
        }
    }
}