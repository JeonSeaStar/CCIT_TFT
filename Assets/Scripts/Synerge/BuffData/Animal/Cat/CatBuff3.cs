using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Buff Datas/Animals/CatBuff3")]
public class CatBuff3 : BuffData
{
    public override void CoroutineEffect()
    {
        FieldManager.Instance.StartCoroutine(Cat());
    }

    IEnumerator Cat()
    {
        yield return new WaitForSeconds(3f);

        foreach(var cat in FieldManager.Instance.myFilePieceList)
        {
            if(cat.pieceData.animal == PieceData.Animal.Cat && cat.gameObject.activeSelf == true)
            {
                cat.invincible = true; 
            }
        }
    }
}

