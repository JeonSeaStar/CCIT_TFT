using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class PieceBuySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public PieceData pieceData;
    public Color[] slotColour = new Color[3];
    public Image slotHighlight;
    public Sprite boughtSlot;
    float colourT;
    bool bought;
    public TextMeshProUGUI pieceName;
    public TextMeshProUGUI pieceCost;

    public void InitSlot(PieceData data)
    {
        slotHighlight.sprite = null;
        bought = false;
        slotHighlight.color = slotColour[0];

        pieceData = data;
        pieceName.text = data.pieceName;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!bought)
        {
            StopColourLerp();
            StartCoroutine("ColourLerp", slotColour[1]);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!bought)
        {
            StopColourLerp();
            StartCoroutine("ColourLerp", slotColour[0]);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
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
        bought = true;
        //print(piece.pieceName);
        slotHighlight.color = slotColour[2];
        slotHighlight.sprite = boughtSlot;
    }
}
