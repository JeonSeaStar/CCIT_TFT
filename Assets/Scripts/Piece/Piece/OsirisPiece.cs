using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OsirisPiece : Piece
{
    float pieceHealth = 5000f;

    public override IEnumerator Attack()
    {
        if (mana <= 75)
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
        StartNextBehavior();
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
                Instantiate(skillEffects, fieldManager.myFilePieceList[i].transform.position, Quaternion.identity);
                fieldManager.myFilePieceList[i].health += heal;
            }
        }
    }
}
