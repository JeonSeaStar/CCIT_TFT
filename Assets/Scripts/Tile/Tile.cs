using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField]
    private Vector2 index; // row , column
    [SerializeField]
    private Vector2 targetIndex; // �����Ҷ� ���� �� Ÿ�� �迭

    public bool isFull = false;
    public bool isReadyTile = false;
    public GameObject piece;
    public Node node;

    public void Awake()
    {

    }
}
