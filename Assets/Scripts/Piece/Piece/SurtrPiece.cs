using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurtrPiece : Piece
{
    protected override void Skill()
    {
        base.Skill();
        if (star == 0)
        {
            if (mana == 100)
            {
                //아직 실드 처리가 안되어 있음 따라서 베리어 달고 체력까일 필요 있음
            }
        }
        else if (star == 1)
        {
            if (mana == 100)
            {

            }
        }
        else if (star == 2)
        {
            if (mana == 100)
            {

            }
        }
    }
}
