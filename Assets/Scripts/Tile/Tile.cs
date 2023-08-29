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

    public void Awake()
    {

    }
}
