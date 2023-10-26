using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Buff Datas/Myth/FrostyWindBuff1")]
public class FrostyWindBuff1 : BuffData
{

    public override void CoroutineEffect()
    {
        //ArenaManager.Instance.fieldManagers[0].StartCoroutine(Test());
    }

    //IEnumerator Test()
    //{

    //    //while()
    //    //{
            
    //    //    yield return new WaitForSeconds(1f);
    //    //}
    //    //yield return null;
    //    //yield return new WaitForSeconds(3f);

    //    //int _count = Random.Range(1, 4);
    //    //List<Piece> enemyList = ArenaManager.Instance.fieldManagers[0].enemyFilePieceList;
    //    //Piece[] enemyPiece = new Piece[_count];

    //    //for(int i = 0; i < _count; i++)
    //    //{
    //    //    enemyPiece[i] = enemyList[Random.Range(0, enemyList.Count)];
    //    //    enemyList.Remove(enemyPiece[i]);
    //    //}

    //    //ArenaManager.Instance.fieldManagers[0].StartCoroutine(Test());
    //}
}
