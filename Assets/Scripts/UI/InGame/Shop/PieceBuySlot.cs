using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class PieceBuySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public FieldManager fieldManager;
    public PieceData pieceData;
    public Color[] slotColour = new Color[3];
    public Image slotHighlight;
    public Sprite boughtSlot;
    float colourT;
    bool Bought
    {
        get { return bought; }
        set
        {
            bought = value;
            if(bought)
            {
                StopColourLerp();
                slotHighlight.color = slotColour[2];
                slotHighlight.sprite = boughtSlot;
            }
        }
    }
    bool bought;
    public TextMeshProUGUI pieceName;
    public TextMeshProUGUI pieceCost;

    public void InitSlot(PieceData data)
    {
        slotHighlight.sprite = null;
        Bought = false;
        slotHighlight.color = slotColour[0];

        pieceData = data;
        pieceName.text = data.pieceName;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!Bought)
        {
            StopColourLerp();
            StartCoroutine("ColourLerp", slotColour[1]);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!Bought)
        {
            StopColourLerp();
            StartCoroutine("ColourLerp", slotColour[0]);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!Bought)
            BuyPiece(pieceData);
    }

    IEnumerator ColourLerp(Color targetColour)
    {
        colourT += 0.1f;
        slotHighlight.color = Color.Lerp(slotHighlight.color, targetColour, colourT);
        yield return new WaitForSeconds(0.02f);

        if (slotHighlight.color != targetColour)
            StartCoroutine("ColourLerp", targetColour);
        else
            StopColourLerp();
    }

    void StopColourLerp()
    {
        StopCoroutine("ColourLerp");
        colourT = 0;
    }

    void BuyPiece(PieceData piece)
    {
        Tile targetTile = GetTile();

        if(targetTile != null)
        {
            GameObject pieceObject = Instantiate(piece.piecePrefab, targetTile.transform.position, Quaternion.Euler(-90, 180, 0));
            targetTile.isFull = true;

            Bought = true;
        }
        else
        {
            print("타일이 가득 참");
        }
    }

    Tile GetTile()
    {
        Tile targetTile = null;

        targetTile = TileCheck(targetTile, fieldManager.readyZoneHexaIndicators);
        if(targetTile != null) { return targetTile; }

        targetTile = TileCheck(targetTile, fieldManager.battleFieldHexaIndicators);
        if (targetTile != null) { return targetTile; }
        else return null;
    }

    Tile TileCheck(Tile tile, GameObject[] tileArray)
    {
        foreach (GameObject tileObject in tileArray)
        {
            if (!tileObject.GetComponentInParent<Tile>().isFull)
            {
                tile = tileObject.GetComponentInParent<Tile>();
                break;
            }
        }

        return tile;
    }
}
