using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Buff Datas/Animals/DogBuff1")]
public class DogBuff1 : BuffData
{
    bool isOnce;
    int endCount;

    PathFinding pathFinding;
    Piece captainDog;

    public override void DirectEffect(Piece piece, bool isAdd)
    {
        if (isAdd)
        {
            if (isOnce == false)
            {
                List<Piece> dogs = new List<Piece>();
                foreach (var dog in ArenaManager.Instance.fieldManagers[0].myFilePieceList)
                {
                    if (dog.pieceData.animal == PieceData.Animal.Dog) dogs.Add(dog);
                }

                dogs.OrderBy(p => p.star).ToList(); dogs.Reverse();
                if (dogs[0].star == dogs[1].star) // 등급 우선순위
                {
                    dogs.Clear();
                    foreach(var dog in ArenaManager.Instance.fieldManagers[0].myFilePieceList)
                    {
                        if (dog.star == dogs[0].star) dogs.Add(dog);
                    }
                    dogs.OrderBy(p => p.Equipments.Count).ToList(); dogs.Reverse();
                    
                    if (dogs[0].Equipments.Count == dogs[1].Equipments.Count) // 아이템 소지
                    {
                        dogs.Clear();
                        foreach (var dog in ArenaManager.Instance.fieldManagers[0].myFilePieceList)
                        {
                            if (dog.Equipments.Count == dogs[0].Equipments.Count) dogs.Add(dog);
                        }
                        dogs.OrderBy(p => p.attackDamage).ToList(); dogs.Reverse();

                        if (dogs[0].attackDamage == dogs[1].attackDamage) // 공격력
                        {
                            dogs.Clear();
                            foreach (var dog in ArenaManager.Instance.fieldManagers[0].myFilePieceList)
                            {
                                if (dog.attackDamage == dogs[0].Equipments.Count) dogs.Add(dog);
                            }
                            dogs.OrderBy(p => p.armor).ToList(); dogs.Reverse();
                        }
                    }
                }
                captainDog = dogs[0];
                int _star = captainDog.star;
                captainDog.attackDamage += captainDog.pieceData.attackDamage[_star] * 0.1f;


                endCount++;
                isOnce = true;
            }

            if (endCount == ArenaManager.Instance.fieldManagers[0].animalActiveCount[PieceData.Animal.Dog])
            {
                endCount = 0;
                isOnce = false;
            }
        }
        else if (isAdd == false)
        {
            int _star = captainDog.star;
            captainDog.attackDamage -= captainDog.pieceData.attackDamage[_star] * 0.1f;
            captainDog = null;
        }
    }

    Piece leftPiece;
    Piece rightPiece;
    public override void BattleStartEffect(bool isAdd)
    {
        pathFinding = ArenaManager.Instance.fieldManagers[0].pathFinding;

        if (isAdd)
        {
            int _tileGridX = captainDog.currentTile.listX;
            int _tileGridY = captainDog.currentTile.listY;

            if (_tileGridX != 0)
            {
                Piece _leftPiece = pathFinding.grid[_tileGridY].tile[_tileGridX - 1].piece;
                if (_leftPiece != null) 
                {
                    leftPiece = _leftPiece;
                    _leftPiece.armor += _leftPiece.pieceData.armor[_leftPiece.star] * 0.05f; 
                }
            }

            if (_tileGridX != 6)
            {
                Piece _rightPiece = pathFinding.grid[_tileGridY].tile[_tileGridX + 1].piece;
                if (_rightPiece != null) 
                {
                    rightPiece = _rightPiece;
                    _rightPiece.armor += _rightPiece.pieceData.armor[_rightPiece.star] * 0.05f; 
                }
            }
        }
        else if (isAdd == false)
        {
            if (leftPiece != null) leftPiece.armor -= leftPiece.pieceData.armor[leftPiece.star];
            if (rightPiece != null) rightPiece.armor -= rightPiece.pieceData.armor[rightPiece.star];
            leftPiece = null; rightPiece = null;
        }
    }
}
