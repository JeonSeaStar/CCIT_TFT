using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Buff Datas/Animals/CatBuff1")]
public class CatBuff1 : BuffData
{
    public override void Effect()
    {
        buffType = ApplyBuffType.OncePerAttack;
        throw new System.NotImplementedException();
    }
}
