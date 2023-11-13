using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MimirPiece : Piece
{
    float pieceHealth = 4000f;
    protected override void Skill()
    {
        base.Skill();
        if (star == 0)
        {
            if (mana == 100)
                FindLeastHealthPiece(150f);
        }
        else if (star == 1)
        {
            if (mana == 100)
                FindLeastHealthPiece(175f);
        }
        else if (star == 2)
        {
            if (mana == 100)
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
            }
            pieceHealth += heal;
        }

    }
}
