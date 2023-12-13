using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Buff Datas/Myth/HeavenGroundBuff2")]
public class HeavenGroundBuff2 : BuffData
{
    public override void DirectEffect(Piece piece, bool isAdd)
    {
        piece.immune = (isAdd) ? true : false;
    }

    public override void CoroutineEffect()
    {
        FieldManager.Instance.StartCoroutine(HeavenGround());
    }

    PathFinding pathFinding;
    IEnumerator HeavenGround()
    {
        pathFinding = FieldManager.Instance.pathFinding;
        while (true)
        {
            yield return new WaitForSeconds(1f);
            List<Piece> heavenGroundPieces = new List<Piece>();
            foreach (var piece in FieldManager.Instance.myFilePieceList)
            {
                if (piece.pieceData.myth == PieceData.Myth.HeavenGround && piece.gameObject.activeSelf == true)
                    heavenGroundPieces.Add(piece);
            }

            foreach (var tilePiece in heavenGroundPieces)
            {
                int _tileGridX = tilePiece.currentTile.listX;
                int _tileGridY = tilePiece.currentTile.listY;

                Piece _angelPiece = pathFinding.grid[_tileGridY].tile[_tileGridX].piece.GetComponent<Piece>();
                if (_angelPiece.isOwned)
                {
                    _angelPiece.pieceData.CalculateBuff(_angelPiece, this);
                    if (_angelPiece.health > _angelPiece.pieceData.health[_angelPiece.star]) _angelPiece.health = _angelPiece.pieceData.health[_angelPiece.star];
                }

                List<Tile> _getNeigbor = pathFinding.GetNeighbor(pathFinding.grid[_tileGridY].tile[_tileGridX]);
                foreach(var _Neigbor in _getNeigbor)
                {
                    if (_Neigbor.piece.isOwned)
                    {
                        Piece _NeigborPiece = _Neigbor.GetComponent<Piece>();
                        _NeigborPiece.pieceData.CalculateBuff(_NeigborPiece, this);
                        if (_NeigborPiece.health > _NeigborPiece.pieceData.health[_NeigborPiece.star]) _NeigborPiece.health = _NeigborPiece.pieceData.health[_NeigborPiece.star];
                    }
                }
            }
        }
    }
}
