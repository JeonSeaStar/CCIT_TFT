using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OsirisPiece : Piece
{
    float pieceHealth = 5000f;

    protected override IEnumerator Attack()
    {
        if (mana <= 75)
        {
            Skill();
            mana = 0;
            yield return new WaitForSeconds(attackSpeed);
            StartCoroutine(NextBehavior());
        }
        else
        {
            base.Attack();
        }
    }

    protected override IEnumerator Skill()
    {
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
        yield return new WaitForSeconds(attackSpeed);
        StartCoroutine(NextBehavior());
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
                fieldManager.myFilePieceList[i].health += heal;
            }
        }
    }
}
