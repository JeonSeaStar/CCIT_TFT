using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool isFull
    {
        set
        {
            IsFull = value;
            if (IsFull) { walkable = false; }
            else { walkable = true; }
        }
        get { return IsFull; }
    }
    public bool IsFull = false;
    public bool isReadyTile = false;
    public GameObject piece;

    public int listX;
    public int listY;

    public int gridX;
    public int gridY;
    public int gridZ;
    public Tile parent;
    public int gCost, hCost;
    public int fCost { get { return hCost + gCost; } }
    public bool walkable = true;

    public Tile(int gridX, int gridY, int gridZ)
    {
        this.gridX = gridX;
        this.gridY = gridY;
        this.gridZ = gridZ;
    }
}
