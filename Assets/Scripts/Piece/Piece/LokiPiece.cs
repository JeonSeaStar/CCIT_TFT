using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LokiPiece : Piece
{
    public Tile[] firstLineTiles;
    Piece[] firstLinePieces;
    Piece[] targets;
    int randomCount;
    public override IEnumerator Attack()
    {
        if (mana <= 60)
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
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    void GetAdbilityTarget() //라운드 시작시 받기
    {
        for (int i = 0; i < firstLineTiles.Length; i++)
        {
            firstLinePieces[i] = firstLineTiles[i].piece;
            if (firstLinePieces[i] == null)
            {
                Debug.Log("대상없음");
            }
            else if (firstLinePieces[i].isOwned)
            {
                targets[i] = firstLinePieces[i];
            }
        }
    }

    void RandomCapability(float percentage)
    {//체력, 공격력, 공속, 꽝
        randomCount = Random.Range(0, 5);
        if (randomCount == 0)
        {
            for (int i = 0; i < targets.Length; i++)
            {
                targets[i].health = targets[i].health * percentage;
            }
        }
        else if (randomCount == 1)
        {
            for (int i = 0; i < targets.Length; i++)
            {
                targets[i].attackDamage = targets[i].attackDamage * percentage;
            }
        }
        else if (randomCount == 2)
        {
            for (int i = 0; i < targets.Length; i++)
            {
                targets[i].attackSpeed = targets[i].attackSpeed - (percentage % 3f);
            }
        }
        else if (randomCount == 3)
        {
            for (int i = 0; i < targets.Length; i++)
            {
                targets[i].health = targets[i].health;
            }
        }
        else if (randomCount == 4)
        {
            for (int i = 0; i < targets.Length; i++)
            {
                targets[i].attackDamage = targets[i].attackDamage;
            }
        }
        else if (randomCount == 5)
        {
            for (int i = 0; i < targets.Length; i++)
            {
                targets[i].attackSpeed = targets[i].attackSpeed;
            }
        }
    }
}
