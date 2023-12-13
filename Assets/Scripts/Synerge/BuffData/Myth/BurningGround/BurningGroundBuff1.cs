using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Buff Datas/Myth/BurningGroundBuff1")]
public class BurningGroundBuff1 : BuffData
{
    Dictionary<Piece, int> burningGroungPiece = new Dictionary<Piece, int>();
    public override void DirectEffect(Piece piece, bool isAdd)
    {
        if (burningGroungPiece.ContainsKey(piece)) burningGroungPiece[piece] = burningGroungPiece[piece] + 1;
        else burningGroungPiece.Add(piece, 1);

        int _star = piece.star;

        piece.health += piece.pieceData.health[_star] * 0.2f;
        if (piece.health > piece.pieceData.health[_star]) piece.health = piece.pieceData.health[_star];

        float attackPlus = piece.pieceData.attackDamage[_star] * 0.15f;
        piece.attackDamage += attackPlus;
        
    }

    public override void BattleStartEffect(bool isAdd)
    {
        if (isAdd) FieldManager.Instance.AddBattleStartEffect(BattleStartEffect);
        else if (!isAdd)
        {
            foreach (var _piece in FieldManager.Instance.myFilePieceList)
            {
                if (_piece.pieceData.myth == PieceData.Myth.BurningGround && burningGroungPiece.ContainsKey(_piece))
                {
                    for (int i = 0; i < burningGroungPiece[_piece]; i++)
                    {
                        int _star = _piece.star;
                        _piece.attackDamage -= _piece.pieceData.attackDamage[_star] * 0.15f;
                    }
                }
            }
            burningGroungPiece.Clear();
            FieldManager.Instance.RemoveBattleStartEffect(BattleStartEffect);
        }
    }
}
