using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField]
    private Vector2 index; // row , column
    [SerializeField]
    private Vector2 targetIndex; // 원정할때 가게 될 타일 배열

    public bool isFull = false;
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

    public void Awake()
    {

    }
}
