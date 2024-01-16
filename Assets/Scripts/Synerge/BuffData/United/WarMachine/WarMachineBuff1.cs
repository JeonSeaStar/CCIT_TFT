using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Buff Datas/United/WarMachineBuff1")]
public class WarMachineBuff1 : BuffData
{
    Equipment warMachineEquipment1;
    Equipment warMachineEquipment2;
    public override void DirectEffect(Piece piece, bool isAdd)
    {
        var _fieldManger = FieldManager.Instance;
        var _buffManager = _fieldManger.buffManager;

        if (isAdd == true)
        {
            warMachineEquipment1 = _buffManager.warMachineEquipments[0];
            warMachineEquipment2 = _buffManager.warMachineEquipments[1];

            _fieldManger.myEquipmentList.Add(warMachineEquipment1);
            _fieldManger.myEquipmentList.Add(warMachineEquipment2);

            //warMachineEquipment1 ��ġ ���� �ʿ�
            //warMachineEquipment2 ��ġ ���� �ʿ�
        }
        else if (isAdd == false)
        {
            if (_fieldManger.myEquipmentList.Contains(warMachineEquipment1)) { _fieldManger.myEquipmentList.Remove(warMachineEquipment1); }
            else if (!_fieldManger.myEquipmentList.Contains(warMachineEquipment1)) 
            {
                foreach(var warPiece in _fieldManger.myFilePieceList)
                {
                    foreach(var warItem in warPiece.Equipments)
                    {
                        if (warItem.equipmentName == warMachineEquipment1.equipmentName)
                        { 
                            Debug.Log("���� ���� ���� ������ ���� ��� ���� �ʿ�");
                            break;
                        }
                    }
                }
            }

            if (_fieldManger.myEquipmentList.Contains(warMachineEquipment2)) _fieldManger.myEquipmentList.Remove(warMachineEquipment2);
            else if (!_fieldManger.myEquipmentList.Contains(warMachineEquipment2)) 
            {
                foreach (var warPiece in _fieldManger.myFilePieceList)
                {
                    foreach (var warItem in warPiece.Equipments)
                    {
                        if (warItem.equipmentName == warMachineEquipment2.equipmentName)
                        {
                            Debug.Log("���� ���� ���� ������ ���� ��� ���� �ʿ�");
                            break;
                        }
                    }
                }
            }
            warMachineEquipment1 = null;
            warMachineEquipment2 = null;
        }
    }
}