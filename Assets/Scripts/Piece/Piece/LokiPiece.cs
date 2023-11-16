using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LokiPiece : Piece
{
    public Tile[] firstLineTiles;
    Piece[] firstLinePieces;
    Piece[] targets;
    int randomCount;
    protected override void Attack()
    {
        if (mana <= 90)
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
            RandomCapability(1.3f);
        }
        else if (star == 1)
        {
            RandomCapability(1.6f);
        }
        else if (star == 2)
        {
            RandomCapability(1.9f);
        }
    }

    void GetAdbilityTarget() //라운드 시작시 받기
    {
        for (int i = 0; i < firstLineTiles.Length; i++)
        {
            firstLinePieces[i] = firstLineTiles[i].piece;
            if (firstLinePieces[i].isOwned)
            {
                targets[i] = firstLinePieces[i];
            }
        }
    }
    void RandomCapability(float abilityPower)
    {
        randomCount = Random.Range(0, 6);
        if(randomCount == 1)
        {
            //어떤 능력치 사용할 건지 넣어주어야 함
        }
        else if(randomCount == 2)
        {

        }
        else if(randomCount == 3)
        {

        }
        else if(randomCount == 4)
        {

        }
        else if(randomCount == 5)
        {

        }
        else if(randomCount == 6)
        {

        }
    }
}
