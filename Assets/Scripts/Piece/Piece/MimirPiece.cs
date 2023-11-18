using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MimirPiece : Piece
{
    float pieceHealth = 5000f;
    
    protected override void Attack()
    {
        if (mana <= 90)
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
            FindLeastHealthPiece(150f);
        }
        else if (star == 1)
        {
            FindLeastHealthPiece(175f);
        }
        else if (star == 2)
        {
            FindLeastHealthPiece(230f);
        }
    }

    public void FindLeastHealthPiece(float heal)
    {
        if (fieldManager.myFilePieceList != null)
        {
            for (int i = 0; i < fieldManager.myFilePieceList.Count; i++)
            {
                if(pieceHealth > fieldManager.myFilePieceList[i].health)
                {
                    pieceHealth = fieldManager.myFilePieceList[i].health;
                }
                fieldManager.myFilePieceList[i].health += heal;
            }
        }
    }
}
