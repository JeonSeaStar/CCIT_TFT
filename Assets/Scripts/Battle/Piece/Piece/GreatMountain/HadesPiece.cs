using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HadesPiece : Piece
{
    PathFinding pathFinding;
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
        SkillState();
        if (star == 0)
        {
            GetLocationMultiRangeSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)));
            this.shield = 350f;
        }
        else if (star == 1)
        {
            GetLocationMultiRangeSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)));
            this.shield = 450f;
        }
        else if (star == 2)
        {
            GetLocationMultiRangeSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)));
            this.shield = 600f;
        }
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    void GetLocationMultiRangeSkill(float damage)
    {
        SoundManager.instance.Play("GreatMountain/S_Hades", SoundManager.Sound.Effect);
        pathFinding = ArenaManager.Instance.fieldManagers[0].pathFinding;
        List<Tile> _getNeigbor = pathFinding.GetNeighbor(currentTile);
        foreach (var _Neigbor in _getNeigbor)
        {
            Piece _targets = _Neigbor.piece;
            if (_targets == null)
            {
                Debug.Log("������");
            }
            else if (!_targets.isOwned)
            {
                Instantiate(skillEffects, _targets.transform.position, Quaternion.identity);
                Damage(_targets, damage);
            }
        }
    }
    public override void SkillUpdateText()
    {
        if (star == 0)
            pieceData.skillExplain = string.Format("�ֺ� ���鿡�� {0}�� ���ظ� ������ {1}�� ���ظ� ����ϴ� ��ȣ���� ����ϴ�.", (abilityPower * (1 + (abilityPowerCoefficient / 100))), 350);
        else if (star == 1)
            pieceData.skillExplain = string.Format("�ֺ� ���鿡�� {0}�� ���ظ� ������ {1}�� ���ظ� ����ϴ� ��ȣ���� ����ϴ�.", (abilityPower * (1 + (abilityPowerCoefficient / 100))), 450);
        else if (star == 2)
            pieceData.skillExplain = string.Format("�ֺ� ���鿡�� {0}�� ���ظ� ������ {1}�� ���ظ� ����ϴ� ��ȣ���� ����ϴ�.", (abilityPower * (1 + (abilityPowerCoefficient / 100))), 600);
    }
}
