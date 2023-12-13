using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Buff Datas/United/FaddistBuff1")]
public class FaddistBuff1 : BuffData
{
    List<Piece> faddist = new List<Piece>();
    List<float> status = new List<float>();
    List<string> statusName = new List<string>();
    public override void BattleStartEffect(bool isAdd)
    {
        if (isAdd == true)
        {
            foreach (var _faddist in FieldManager.Instance.myFilePieceList)
            {
                if (_faddist.pieceData.united == PieceData.United.Faddist)
                {
                    faddist.Add(_faddist);
                    int _random = Random.Range(0, 4);
                    int _status = Random.Range(-20, 51);
                    switch(_random)
                    {
                        case 0:
                            statusName.Add("Health"); this.health = _status;
                            break;
                        case 1:
                            statusName.Add("AttackDamage"); this.attackDamage = _status;
                            break;
                        case 2:
                            statusName.Add("Armor"); this.armor = _status;
                            break;
                        case 3:
                            statusName.Add("AttackSpeed"); this.attackSpeed = _status;
                            break;
                    }
                    status.Add(_status);
                    _faddist.pieceData.CalculateBuff(_faddist, this);
                    InitStatus();
                }
            }

        }
        else if(isAdd == false)
        {
            for(int i = 0; i < faddist.Count; i++)
            {
                switch(statusName[i])
                {
                    case "Health":
                        this.health -= status[i];
                        break;
                    case "AttackDamage":
                        this.health -= status[i];
                        break;
                    case "Armor":
                        this.health -= status[i];
                        break;
                    case "AttackSpeed":
                        this.health -= status[i];
                        break;
                }
                faddist[i].pieceData.CalculateBuff(faddist[i], this);
                InitStatus();
            }
            
            statusName.Clear();
            faddist.Clear();
        }
    }

    void InitStatus()
    {
        this.health = 0;
        this.attackDamage = 0;
        this.armor = 0;
        this.attackSpeed = 0;
    }
}