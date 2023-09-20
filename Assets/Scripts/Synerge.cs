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
                    Debug.Log("������(����) �ó����� �����Ͽ� - 2 -�� �߻��մϴ�.");
                    break;
                case 4:
                    Debug.Log("������(����) �ó����� �����Ͽ� - 4 -�� �߻��մϴ�.");
                    break;
                case 6:
                    Debug.Log("������(����) �ó����� �����Ͽ� - 6 -�� �߻��մϴ�.");
                    break;
            }
        }
        else
        {
            switch (frogCount)
            {
                case 2:
                    Debug.Log("������(����) �ó����� �����Ͽ� - 2 -�� �߻��մϴ�.");
                    break;
                case 4:
                    Debug.Log("������(����) �ó����� �����Ͽ� - 4 -�� �߻��մϴ�.");
                    break;
                case 6:
                    Debug.Log("������(����) �ó����� �����Ͽ� - 6 -�� �߻��մϴ�.");
                    break;
            }
        }
    }
}
