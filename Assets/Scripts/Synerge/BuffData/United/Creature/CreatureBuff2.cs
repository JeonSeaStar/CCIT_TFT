using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Buff Datas/United/CreatureBuff2")]
public class CreatureBuff2 : BuffData
{
    public override void DirectEffect(Piece piece, bool isAdd)
    {
        if (isAdd) piece.pieceData.CalculateBuff(piece, this);
        else if (isAdd == false) piece.pieceData.CalculateBuff(piece, this, false);
    }
}
