using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OsirisPiece : Piece
{
    float pieceHealth = 4000f;

    protected override void Attack()
    {
        if (mana <= 75)
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
            FindLeastHealthPiece(120f);
        }
        else if (star == 1)
        {
            FindLeastHealthPiece(250f);
        }
        else if (star == 2)
        {
            FindLeastHealthPiece(400f);
        }
    }

    public void FindLeastHealthPiece(float heal)
    {
        if (fieldManager.myFilePieceList != null)
        {
            for (int i = 0; i < fieldManager.myFilePieceList.Count; i++)
            {
                if (pieceHealth > fieldManager.myFilePieceList[i].health)
                {
                    pieceHealth = fieldManager.myFilePieceList[i].health;
                }
            }
            pieceHealth += heal;
        }
    }
}
