using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThorPiece : Piece
{
    public GameObject tickEffect;

    public override IEnumerator Attack()
    {
        if (mana >= maxMana && target != null)
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
            AllPieceDamageSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)), (abilityPower * (1 + (abilityPowerCoefficient / 100))) * 0.3f, 5);
        }
        else if (star == 1)
        {
            AllPieceDamageSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)), (abilityPower * (1 + (abilityPowerCoefficient / 100))) * 0.6f, 10);
        }
        else if (star == 2)
        {
            AllPieceDamageSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)), (abilityPower * (1 + (abilityPowerCoefficient / 100))) * 1f, 20);
        }
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    void AllPieceDamageSkill(float damage, float tickDamage, float time)
    {
        if (dead)
            return; 
        if (pieceState == State.DANCE)
            return;

        SkillState();
        SoundManager.instance.Play("FrostyWind/S_Thor", SoundManager.Sound.Effect);
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
                SetTickDamage(tickEffect, tickDamage, time);
            }
        }
    }
    public override void SkillUpdateText()
    {
        if (star == 0)
            pieceData.skillExplain = string.Format("���ϸ��� ������ �ҷ��� ��� ������ {0}�� ���ظ� ������, {1}�� ���� {2}�� ���ظ� ������ �������븦 �����մϴ�.", (abilityPower * (1 + (abilityPowerCoefficient / 100))), 5, 30);
        if (star == 1)
            pieceData.skillExplain = string.Format("���ϸ��� ������ �ҷ��� ��� ������ {0}�� ���ظ� ������, {1}�� ���� {2}�� ���ظ� ������ �������븦 �����մϴ�.", (abilityPower * (1 + (abilityPowerCoefficient / 100))), 10, 60);
        if (star == 2)
            pieceData.skillExplain = string.Format("���ϸ��� ������ �ҷ��� ��� ������ {0}�� ���ظ� ������, {1}�� ���� {2}�� ���ظ� ������ �������븦 �����մϴ�.", (abilityPower * (1 + (abilityPowerCoefficient / 100))), 999, 100);
    }
}
