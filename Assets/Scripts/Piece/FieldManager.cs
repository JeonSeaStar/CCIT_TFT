//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldManager : MonoBehaviour
{
    public static FieldManager instance;

    public GameObject testPiece;
    public List<PrivatePieceCount> privatePieceCount;

    [Space(10)]
    //Jun
    public bool isBattle = false;
    public enum RoundType 
    {
        NONE = 0,
        WAITING,
        BATTLE ,
        EVENT , 
        DEAD
    };
    public RoundType roundType = RoundType.NONE;

    public GameObject[] waitingZoneHexaIndicators;
    public GameObject[] battleFieldHexaIndicators;

    public int getPieceCount = 0; // 구매해서 가지고 있는 기물 갯수
    public int setPieceCount = 0; // 구매해서 배치한 기물 갯수

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Piece piece = SpawnPiece(testPiece, 0);
            piece.Owned();
        }
    }
    int d = 0;
    public Piece SpawnPiece(GameObject p, int star)
    {
        GameObject pieceGameObject = Instantiate(p, new Vector3(0, 0, 0), Quaternion.identity);
        pieceGameObject.name = "testPiece[" + d + "]";
        pieceGameObject.TryGetComponent(out Piece piece);
        piece.star = star;
        d++;
        return piece;
    }

    public int FindPieceList(Piece piece)
    {
        int listIndex = -1;
        for (int i = 0; i < privatePieceCount.Count; i++)
        {
            if (privatePieceCount[i].pieceName == piece.pieceName)
                listIndex = i;
        }

        return listIndex;
    }

    // Jun
    public void ActiveHexa(bool isbattle)
    {
        if (isbattle == true) // 전투 진행중
        {
            for (int i = 0; i < waitingZoneHexaIndicators.Length; i++)
            {
                waitingZoneHexaIndicators[i].SetActive(isbattle);
            }
        }
        else if(isbattle == false)
        {
             
        }
    }
}

[System.Serializable]
public class PrivatePieceCount
{
    public string pieceName;
    public List<Piece> piecesList;
    public int[] star = new int[3];

    public void PieceCountUp(Piece piece)
    {
        if (FieldManager.instance.privatePieceCount[FieldManager.instance.FindPieceList(piece)].pieceName == pieceName)
        {
            star[piece.star]++;
            piecesList.Add(piece);
            FusionPiece(piece);
        }
    }

    public void PieceCountDown(Piece piece)
    {
        if (FieldManager.instance.privatePieceCount[FieldManager.instance.FindPieceList(piece)].pieceName == pieceName)
        {
            star[piece.star]--;
            piecesList.Remove(piece);
        }
    }

    void FusionPiece(Piece piece)
    {
        int fusionCondition = 1;
        Piece firstFusionTarget = null;
        Piece secondFusionTarget = null;
        bool canFusion = false;

        for (int i = 0; i < piecesList.Count; i++)
        {
            if (piecesList[i] != piece && piecesList[i].star == piece.star)
            {
                if (fusionCondition == 0)
                {
                    secondFusionTarget = piecesList[i];
                    canFusion = true;
                    break;
                }
                else if (fusionCondition == 1)
                {
                    firstFusionTarget = piecesList[i];
                    fusionCondition--;
                }
            }
        }

        if (canFusion)
        {
            piece.DestroyPiece();
            firstFusionTarget.DestroyPiece();
            secondFusionTarget.DestroyPiece();
            Fusion(piece);
        }
    }

    void Fusion(Piece piece)
    {
        Piece p = FieldManager.instance.SpawnPiece(FieldManager.instance.testPiece, ++piece.star);
        p.Owned();
    }
}