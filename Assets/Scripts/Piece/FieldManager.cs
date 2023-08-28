//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldManager : MonoBehaviour
{
    public static FieldManager instance;
    void Awake() => instance = this;

    public GameObject testPiece;
    public List<PrivatePieceCount> privatePieceCount;

    public GameObject[] hexaIndicators;
    public int getPieceCount; // 구매해서 가지고 있는 기물 갯수
    public int setPieceCount; // 배치한 기물 갯수

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Piece piece = SpawnPiece(testPiece);
            piece.Owned();
        }
    }
    int d = 0;
    public Piece SpawnPiece(GameObject p)
    {
        GameObject pieceGameObject = Instantiate(p, new Vector3(0, 0, 0), Quaternion.identity);
        pieceGameObject.name = "testPiece[" + d + "]";
        pieceGameObject.TryGetComponent(out Piece piece);
        d++;
        //privatePieceCount[FindPieceList(piece)].PieceCountUp(piece);
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
}

[System.Serializable]
public class PrivatePieceCount
{
    public string pieceName;
    public List<Piece> PiecesList = new List<Piece>();
    public int PieceCount
    {
        set
        {
            pieceCount = value;
        }
    }
    public int pieceCount;

    public void PieceCountUp(Piece piece)
    {
        if (FieldManager.instance.privatePieceCount[FieldManager.instance.FindPieceList(piece)].pieceName == pieceName)
        {
            pieceCount += piece.pieceGrade;
            PiecesList.Add(piece);
            FusionPiece(piece);
        }
    }

    public void PieceCountDown(Piece piece)
    {
        if (FieldManager.instance.privatePieceCount[FieldManager.instance.FindPieceList(piece)].pieceName == pieceName)
        {
            pieceCount -= piece.pieceGrade;
            PiecesList.Remove(piece);
        }
    }

    void FusionPiece(Piece piece)
    {
        int listIndex = FieldManager.instance.FindPieceList(piece);
        int fusionCondition = 1;
        Piece firstFusionTarget = null;
        Piece secondFusionTarget = null;
        bool canFusion = false;
        for(int i = 0; i < FieldManager.instance.privatePieceCount[listIndex].PiecesList.Count; i++)
        {
            if(piece != FieldManager.instance.privatePieceCount[listIndex].PiecesList[i] && piece.pieceGrade == FieldManager.instance.privatePieceCount[listIndex].PiecesList[i].pieceGrade)
            {
                if (fusionCondition == 1)
                {
                    firstFusionTarget = FieldManager.instance.privatePieceCount[listIndex].PiecesList[i];
                    fusionCondition--;
                }
                else if (fusionCondition == 0)
                {
                    secondFusionTarget = FieldManager.instance.privatePieceCount[listIndex].PiecesList[i];
                    canFusion = true;
                    break;
                }
            }
        }

        if(canFusion)
        {
            firstFusionTarget.DestroyPiece();
            secondFusionTarget.DestroyPiece();
            piece.DestroyPiece();
            Fusion(piece);
        }
    }

    void Fusion(Piece piece)
    {
        Piece p = FieldManager.instance.SpawnPiece(FieldManager.instance.testPiece);
        p.pieceGrade *= 3;
        p.Owned();
        //PieceCountUp(p);
    }
}