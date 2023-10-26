using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Buff Datas/Animals/FrogBuff1")]
public class FrogBuff1 : BuffData
{
    public override void DirectEffect(Piece piece, bool isAdd)
    {
        Debug.Log(25);
    }
}
