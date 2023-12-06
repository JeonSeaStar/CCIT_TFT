using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoseidonPiece : Piece
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
        AllPieceDamageSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)));
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    void AllPieceDamageSkill(float damage)
    {
        SoundManager.instance.Play("GreatMountain/S_Poseidon", SoundManager.Sound.Effect);
        List<Piece> _allPiece = fieldManager.enemyFilePieceList;
        foreach (var _Neigbor in _allPiece)
        {
            Piece _targets = _Neigbor.GetComponent<Piece>();
            if (_targets == null)
            {
                Debug.Log("������");
            }
            else
            {
                Instantiate(skillEffects, _targets.transform.position, Quaternion.identity);
                Damage(_targets, damage);
            }
        }
    }
    public override void SkillUpdateText()
    {
        pieceData.skillExplain = string.Format("������ ���� {0}�� ���ظ� ������ �ĵ��� �θ��ϴ�.", (abilityPower * (1 + (abilityPowerCoefficient / 100))));
    }
}
