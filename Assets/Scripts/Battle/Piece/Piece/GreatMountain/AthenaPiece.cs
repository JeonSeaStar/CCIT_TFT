using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AthenaPiece : Piece
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
        GetLocationMultiRangeSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)));
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    void GetLocationMultiRangeSkill(float damage)
    {
        SoundManager.instance.Play("GreatMountain/S_Athena", SoundManager.Sound.Effect);
        pathFinding = ArenaManager.Instance.fieldManagers[0].pathFinding;
        List<Tile> _getNeigbor = pathFinding.GetNeighbor(currentTile);
        foreach (var _Neigbor in _getNeigbor)
        {
            Piece _targets = _Neigbor.piece;
            if (_targets == null)
            {
                Debug.Log("������");
            }
            else if (_targets.isOwned == false)
            {
                Instantiate(skillEffects, _targets.transform.position, Quaternion.identity);
                Damage(_targets, damage);
            }
        }
    }
    public override void SkillUpdateText()
    {
        pieceData.skillExplain = string.Format("���� ���� ���� ������ ��Ÿ�� ���鿡�� {0}�� ���ظ� �����ϴ�.", (abilityPower * (1 + (abilityPowerCoefficient / 100))));
    }
}
