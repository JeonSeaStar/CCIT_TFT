using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Protocol;
using BackEnd.Tcp;

public class PieceBuySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public int index;
    public FieldManager fieldManager;
    public PieceData pieceData;
    public Color[] slotColour = new Color[3];
    public Image slotHighlight;
    public Sprite boughtSlot;
    float colourT;

    public Player myPlayer;
    public SessionId myPlayerIndex;

    private void Awake()
    {
        StartCoroutine(MyPlayerFind());
    }

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
        //if (!Bought)
        //    BuyPiece(pieceData);
        if(!bought)
        {
            if(myPlayerIndex == fieldManager.index)
            {
                fieldManager.pieceBuySlot = this;
                fieldManager.pieceData = pieceData;
                //WorldManager.instance.pieceData = fieldManager.pieceData;
                ButtonBuyPiece0();
            }
        }
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
   
    public void BuyPiece()
    {
        Tile targetTile = fieldManager.GetTile();

        if(targetTile != null)
        {
            fieldManager.SpawnPiece(pieceData, 0, targetTile);
            //GameObject pieceObject = Instantiate(piece.piecePrefab, targetTile.transform.position, Quaternion.Euler(-90, 180, 0));
            //targetTile.isFull = true;

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

        targetTile = TileCheck(targetTile, fieldManager.readyTileList);
        if(targetTile != null) { return targetTile; }

        targetTile = TileCheck(targetTile, fieldManager.battleTileList);
        if (targetTile != null) { return targetTile; }
        else return null;
    }

    Tile TileCheck(Tile tile, List<Tile> tileArray)
    {
        foreach (Tile tileObject in tileArray)
        {
            if (!tileObject.IsFull)
            {
                tile = tileObject;
                break;
            }
        }

        return tile;
    }
    #region 서버 메세지
    public void ButtonBuyPiece0()
    {
        //if(index == 0)
        {
            int ButtonCode = 0;
            ButtonCode |= ButtonEventCode.BUYPIECE0;

            SessionId Player = WorldManager.instance.GetMyPlayer();

            ButtonMessage msg;
            msg = new ButtonMessage(ButtonCode, Player);

            if (BackEndMatchManager.GetInstance().IsHost())
            {
                BackEndMatchManager.GetInstance().AddMsgToButtonLocalQueue(msg);
            }
            else
            {
                BackEndMatchManager.GetInstance().SendDataToInGame<ButtonMessage>(msg);
            }
            bought = true;
        }
    }



    #endregion

    //내 플레이어 누구이고 누구의 필드매니저를 받는지
    IEnumerator MyPlayerFind()
    {
        yield return new WaitForSeconds(5f);
        myPlayer = fieldManager.owerPlayerTest;
        myPlayerIndex = fieldManager.index;
    }
}
