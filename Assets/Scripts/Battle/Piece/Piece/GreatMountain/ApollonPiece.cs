using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApollonPiece : Piece
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
        GetLocationMultiRangeSkill(abilityPower);
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    void GetLocationMultiRangeSkill(float time)
    {
        if (dead)
            return;
        SkillState();
        pathFinding = FieldManager.Instance.pathFinding;
        List<Tile> _getNeigbor = pathFinding.GetNeighbor(currentTile);
        foreach (var _Neigbor in _getNeigbor)
        {
            Piece _targets = _Neigbor.piece;
            if (_targets == null)
            {
                Debug.Log("��� ����");
            }
            else if (!_targets.isOwned)
            {
                SoundManager.instance.Play("GreatMountain/S_Apollon", SoundManager.Sound.Effect);
                Instantiate(skillEffects, _targets.transform.position, Quaternion.identity);
                target.SetStun(time);
            }
        }
    }
    public override void SkillUpdateText()
    {
        pieceData.skillExplain = string.Format("������ 1ĭ ������ �� �⹰���� {0}�� ���� ���� ���·� ����� ���� �����մϴ�.", abilityPower);
    }
}

