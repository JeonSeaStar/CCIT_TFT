using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Synerge
{
    public static void MythGreatMoutain(bool isPlus)
    {
        FieldManager _fm = ArenaManager.instance.fm[0];

        int _count = _fm.mythActiveCount[PieceData.Myth.GreatMountain];
        
    }

    public static void MythFrostyWind(int count,List<Piece> enemyFilePieceList)
    {

    }

    public static IEnumerator MythSandKingdom()
    {
        yield return new WaitForSeconds(2f); //Temporary Value 2f
    }

    public static void MythHeavenGround(Piece piece, int frogCount, bool isPlus)
    {
        //Heaven Piece Abnormal Condition
    }
    public static IEnumerator MythHeavenGround()
    {
        yield return new WaitForSeconds(2f); //Temporary Value 2f
    }

    public static void MythBurningGround()
    {

    }


    public static void AnimalFrog(Piece piece, int frogCount,bool isPlus)
    {
        var fm = ArenaManager.instance.fm[0];

        if(isPlus)
        {
            switch(frogCount)
            {
                case 2:
                    piece.damageRise = piece.attackDamage * 0.1f;
                    Debug.Log("개구리(종족) 시너지가 증가하여 - 2 -가 발생합니다.");
                    break;
                case 4:
                    Debug.Log("개구리(종족) 시너지가 증가하여 - 4 -가 발생합니다.");
                    break;
                case 6:
                    Debug.Log("개구리(종족) 시너지가 증가하여 - 6 -가 발생합니다.");
                    break;
            }
        }
        else
        {
            switch (frogCount)
            {
                case 2:
                    Debug.Log("개구리(종족) 시너지가 감소하여 - 2 -가 발생합니다.");
                    break;
                case 4:
                    Debug.Log("개구리(종족) 시너지가 감소하여 - 4 -가 발생합니다.");
                    break;
                case 6:
                    Debug.Log("개구리(종족) 시너지가 감소하여 - 6 -가 발생합니다.");
                    break;
            }
        }
    }
}
