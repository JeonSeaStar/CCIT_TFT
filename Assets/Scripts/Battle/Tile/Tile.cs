using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
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

    public GameObject tileSelectEffect;

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

    public void ActiveTileEffect(bool isActive)
    {
        tileSelectEffect.SetActive(isActive);
    }
}
