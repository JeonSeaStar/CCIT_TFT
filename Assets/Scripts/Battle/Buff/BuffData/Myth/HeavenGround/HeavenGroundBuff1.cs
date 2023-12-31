using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(menuName = "Scriptable Object/Buff Datas/Myth/HeavenGroundBuff1")]
public class HeavenGroundBuff1 : BuffData
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
        pathFinding = ArenaManager.Instance.fieldManagers[0].pathFinding;
        while(true)
        {
            yield return new WaitForSeconds(2f);
            List<Piece> heavenGroundPieces = new List<Piece>();
            foreach (var piece in ArenaManager.Instance.fieldManagers[0].myFilePieceList)
            {
                if (piece.pieceData.myth == PieceData.Myth.HeavenGround && piece.gameObject.activeSelf == true)
                    heavenGroundPieces.Add(piece);
            }

            foreach (var tilePiece in heavenGroundPieces)
            {
                int _tileGridX = tilePiece.currentTile.listX;
                int _tileGridY = tilePiece.currentTile.listY;

                Piece _angelPiece = pathFinding.grid[_tileGridY].tile[_tileGridX].piece;
                if(_angelPiece.isOwned) _angelPiece.pieceData.CalculateBuff(_angelPiece, this);
                if (_angelPiece.health > _angelPiece.pieceData.health[_angelPiece.star]) _angelPiece.health = _angelPiece.pieceData.health[_angelPiece.star];

                if(_tileGridX != 0)
                {
                    Piece _leftPiece = pathFinding.grid[_tileGridY].tile[_tileGridX - 1].piece;
                    var tile = pathFinding.grid[_tileGridY].tile[_tileGridX - 1].transform.GetChild(0);
                    tile.gameObject.SetActive(true);
                    tile.GetComponent<SpriteRenderer>().color = new Color(14, 255, 0, 255);
                    tile.GetComponent<SpriteRenderer>().DOFade(0, 0.5f).SetEase(Ease.OutQuad);
                    if (_leftPiece != null && _leftPiece.isOwned)
                    {
                        _leftPiece.pieceData.CalculateBuff(_leftPiece, this);
                        if (_leftPiece.health > _leftPiece.pieceData.health[_leftPiece.star]) _leftPiece.health = _leftPiece.pieceData.health[_leftPiece.star];
                    }
                }

                if (_tileGridX != 6)
                {
                    Piece _rightPiece = pathFinding.grid[_tileGridY].tile[_tileGridX + 1].piece;
                    var tile = pathFinding.grid[_tileGridY].tile[_tileGridX + 1].transform.GetChild(0);
                    tile.gameObject.SetActive(true);
                    tile.GetComponent<SpriteRenderer>().color = new Color(14, 255, 0, 255);
                    tile.GetComponent<SpriteRenderer>().DOFade(0, 0.5f).SetEase(Ease.OutQuad);
                    if (_rightPiece != null && _rightPiece.isOwned)
                    {
                        _rightPiece.pieceData.CalculateBuff(_rightPiece, this);
                        if (_rightPiece.health > _rightPiece.pieceData.health[_rightPiece.star]) _rightPiece.health = _rightPiece.pieceData.health[_rightPiece.star];
                    }
                }
            }
        }
    }
}
