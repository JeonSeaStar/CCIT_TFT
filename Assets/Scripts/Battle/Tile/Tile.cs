using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool IsFull
    {
        set
        {
            isFull = value;
            if (isFull) { walkable = false; }
            else { walkable = true; }
        }
        get { return isFull; }
    }
    [SerializeField] private bool isFull = false;
    public bool isReadyTile = false;
    public Piece piece;

    public int listX;
    public int listY;

    public int gridX;
    public int gridY;
    public int gridZ;
    public Tile parent;
    public int gCost, hCost;
    public int fCost { get { return hCost + gCost; } }
    public bool walkable = true;
    public bool myTile = false;

    public GameObject tileEffectPosition;
    public GameObject tileSelectEffect;
    public SpriteRenderer spriteRenderer;
    public Color originColor;
    public Color selectedColor;

    private void Awake()
    {
        if (myTile)
        {
            originColor = spriteRenderer.color;
            selectedColor = spriteRenderer.color;
            selectedColor.a += 245;
        }
    }

    public Tile(int gridX, int gridY, int gridZ)
    {
        this.gridX = gridX;
        this.gridY = gridY;
        this.gridZ = gridZ;
    }

    public void InitTile()
    {
        IsFull = false;
        walkable = true;
        piece = null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (myTile)
        {
            tileSelectEffect.SetActive(true);
            spriteRenderer.color = selectedColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (myTile)
        {
            tileSelectEffect.SetActive(false);
            spriteRenderer.color = originColor;
        }
    }
}
