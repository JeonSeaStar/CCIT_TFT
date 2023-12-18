using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JormungandPiece : Piece
{
    Tile skillCheckTile;
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
        if (star == 0)
        {
            GetLocationMultiRangeSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)), 6);
        }
        else if (star == 1)
        {
            GetLocationMultiRangeSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)), 6);
        }
        else if (star == 2)
        {
            GetLocationMultiRangeSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)), 10);
        }
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    public void GetLocationMultiRangeSkill(float damage, int time)
    {
        if (dead)
            return;
        SkillState();
        SoundManager.instance.Play("FrostyWind/S_Jormungand", SoundManager.Sound.Effect);
        skillCheckTile = target.currentTile;
        pathFinding = FieldManager.Instance.pathFinding;
        Instantiate(skillEffects, target.currentTile.transform.position, Quaternion.identity);
        StartCoroutine(FindNeighbor(damage, time));
    }

    IEnumerator FindNeighbor(float damage, int time)
    {
        if(pieceState == State.DANCE)
        {
            yield break;
        }
        for (int i = 0; i < time; i++)
        {
            List<Tile> _getNeigbor = pathFinding.GetNeighbor(skillCheckTile);
            _getNeigbor.Add(skillCheckTile);
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
            yield return new WaitForSeconds(1f);
        }
    }
    public override void SkillUpdateText()
    {
        if (star == 0)
            pieceData.skillExplain = string.Format("������ �꼺���� ����� {0}�� ���� {1}�� ���ظ� �ִ� �꼺���븦 �����մϴ�.", 6 ,(abilityPower * (1 + (abilityPowerCoefficient / 100))));
        else if (star == 1)
            pieceData.skillExplain = string.Format("������ �꼺���� ����� {0}�� ���� {1}�� ���ظ� �ִ� �꼺���븦 �����մϴ�.", 6 ,(abilityPower * (1 + (abilityPowerCoefficient / 100))));
        else if (star == 2)
            pieceData.skillExplain = string.Format("������ �꼺���� ����� {0}�� ���� {1}�� ���ظ� �ִ� �꼺���븦 �����մϴ�.", 10 ,(abilityPower * (1 + (abilityPowerCoefficient / 100))));
        
    }
}
