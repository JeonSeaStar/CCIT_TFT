using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Buff Datas/United/WarMachineBuff2")]
public class WarMachineBuff2 : BuffData
{
    Equipment warMachineEquipment3;
    Equipment warMachineEquipment4;
    public override void DirectEffect(Piece piece, bool isAdd)
    {
        var _fieldManger = ArenaManager.Instance.fieldManagers[0];
        var _buffManager = _fieldManger.buffManager;

        if (isAdd == true)
        {
            warMachineEquipment3 = _buffManager.warMachineEquipments[2];
            warMachineEquipment4 = _buffManager.warMachineEquipments[3];

            _fieldManger.myEquipmentList.Add(warMachineEquipment3);
            _fieldManger.myEquipmentList.Add(warMachineEquipment4);

            //warMachineEquipment3 ��ġ ���� �ʿ�
            //warMachineEquipment4 ��ġ ���� �ʿ�
        }
        else if (isAdd == false)
        {

            if (_fieldManger.myEquipmentList.Contains(warMachineEquipment3)) { _fieldManger.myEquipmentList.Remove(warMachineEquipment3); }
            else if (!_fieldManger.myEquipmentList.Contains(warMachineEquipment3))
            {
                foreach (var warPiece in _fieldManger.myFilePieceList)
                {
                    foreach (var warItem in warPiece.Equipments)
                    {
                        if (warItem.equipmentName == warMachineEquipment3.equipmentName)
                        {
                            Debug.Log("���� ���� ���� ������ ���� ��� ���� �ʿ�");
                            break;
                        }
                    }
                }
            }

            if (_fieldManger.myEquipmentList.Contains(warMachineEquipment4)) _fieldManger.myEquipmentList.Remove(warMachineEquipment4);
            else if (!_fieldManger.myEquipmentList.Contains(warMachineEquipment4))
            {
                foreach (var warPiece in _fieldManger.myFilePieceList)
                {
                    foreach (var warItem in warPiece.Equipments)
                    {
                        if (warItem.equipmentName == warMachineEquipment4.equipmentName)
                        {
                            Debug.Log("���� ���� ���� ������ ���� ��� ���� �ʿ�");
                            break;
                        }
                    }
                }
            }
            warMachineEquipment3 = null;
            warMachineEquipment4 = null;
        }
    }
}