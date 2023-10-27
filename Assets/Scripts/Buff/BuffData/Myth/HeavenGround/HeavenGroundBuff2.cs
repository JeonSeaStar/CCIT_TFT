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
        ArenaManager.Instance.fieldManagers[0].StartCoroutine(HeavenGround());
    }

    PathFinding pathFinding;
    IEnumerator HeavenGround()
    {
        pathFinding = GameObject.Find("PathFinding").GetComponent<PathFinding>();
        List<Piece> heavenGroundPieces = new List<Piece>();
        foreach (var piece in ArenaManager.Instance.fieldManagers[0].myFilePieceList)
        {
            if (piece.pieceData.myth == PieceData.Myth.HeavenGround) heavenGroundPieces.Add(piece);
        }
        while (true)
        {
            yield return new WaitForSeconds(1f);

            foreach (var tilePiece in heavenGroundPieces)
            {
                int _tileGridX = tilePiece.currentTile.listX;
                int _tileGridY = tilePiece.currentTile.listY;

                Piece _angelPiece = pathFinding.grid[_tileGridX].tile[_tileGridY].piece.GetComponent<Piece>();
                if (_angelPiece.isOwned)
                {
                    _angelPiece.pieceData.CalculateBuff(_angelPiece, this);
                    if (_angelPiece.health > _angelPiece.pieceData.health[_angelPiece.star]) _angelPiece.health = _angelPiece.pieceData.health[_angelPiece.star];
                }

                List<Tile> _getNeigbor = pathFinding.GetNeighbor(pathFinding.grid[_tileGridX].tile[_tileGridY]);
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
