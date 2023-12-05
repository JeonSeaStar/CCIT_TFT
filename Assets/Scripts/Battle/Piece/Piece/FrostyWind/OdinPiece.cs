using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OdinPiece : Piece
{
    public override IEnumerator Attack()
    {
        if (mana >= maxMana)
        {
            StartSkill();
            mana = 0;
            yield return new WaitForSeconds(attackSpeed);
            StartNextBehavior();
        }
        else
        {
            DoAttack();
        }
    }

    public override IEnumerator Skill()
    {
        AllPieceBuffSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)));
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    void AllPieceBuffSkill(float buff)
    {
        List<Piece> _allPiece = fieldManager.myFilePieceList;
        foreach (var _Neigbor in _allPiece)
        {
            Piece _targets = _Neigbor.GetComponent<Piece>();
            if (_targets == null)
            {
                Debug.Log("������");
            }
            else if (_targets.isOwned)
            {
                Instantiate(skillEffects, new Vector3(_targets.transform.position.x, _targets.transform.position.y + 1.5f, _targets.transform.position.z), Quaternion.identity);
                _targets.attackDamage = _targets.attackDamage + buff;
            }
        }
    }
    public override void SkillUpdateText()
    {
        pieceData.skillExplain = string.Format("18���� ��� �� �ϳ��� ����� ��� �Ʊ� �⹰�� ���ݷ��� {0}��ŭ ������ŵ�ϴ�.", (abilityPower * (1 + (abilityPowerCoefficient / 100))));
    }
}
