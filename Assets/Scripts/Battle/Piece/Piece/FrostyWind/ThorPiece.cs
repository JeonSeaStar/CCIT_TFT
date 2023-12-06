using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThorPiece : Piece
{
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
            SkillState();
            AllPieceDamageSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)));
            StartCoroutine(AllPieceDamageTimeSkill(30f, 5f));
        }
        else if (star == 1)
        {
            SkillState();
            AllPieceDamageSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)));
            StartCoroutine(AllPieceDamageTimeSkill(60f, 10f));
        }
        else if (star == 2)
        {
            SkillState();
            AllPieceDamageSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)));
            StartCoroutine(AllPieceDamageTimeSkill(90f, 30f));
        }
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    IEnumerator AllPieceDamageTimeSkill(float damage, float time)
    {
        if (pieceState == State.DANCE)
        {
            yield break;
        }
        for (int i = 0; i < time; i++)
        {
            Debug.Log(i);
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
            yield return new WaitForSeconds(1f);
        }
    }

    void AllPieceDamageSkill(float damage)
    {
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
